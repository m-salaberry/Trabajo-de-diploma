using DAL.Contracts;
using DAL.Implementations;
using Domain;
using Domain.Enums;
using Services.Contracts.CustomsException;
using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Implementations
{
    public class PurchaseOrderService : GenericBllService<PurchaseOrder, Guid>
    {
        /// <summary>
        /// Initializes a new instance with the specified purchase order repository.
        /// </summary>
        private PurchaseOrderService(PurchaseOrderRepository repository) : base(repository) { }

        private static PurchaseOrderService _instance = null;

        /// <summary>
        /// Returns the singleton instance of PurchaseOrderService.
        /// </summary>
        public static PurchaseOrderService Instance()
        {
            if (_instance == null)
            {
                _instance = new PurchaseOrderService(new PurchaseOrderRepository());
            }
            return _instance;
        }

        // ========================================
        // OVERRIDE METHODS WITH BUSINESS VALIDATIONS
        // ========================================

        /// <summary>
        /// Inserts a new purchase order. Validates category consistency between provider and items, and auto-sets status and issued date.
        /// </summary>
        public override void Insert(PurchaseOrder entity)
        {
            ValidatePurchaseOrder(entity);
            ValidateReplacementOrder(entity.ReplacementOrder);

            // Validate that the provider's category matches the items' categories
            var replacementOrder = ReplacementOrderService.Instance().GetById(entity.ReplacementOrder.Id);
            var providerCategoryId = replacementOrder.Provider.Category.Id;

            foreach (var row in replacementOrder.OrderRows)
            {
                if (row.Item.Category.Id != providerCategoryId)
                    throw new MySystemException(
                        $"Item '{row.Item.Name}' belongs to category '{row.Item.Category.Name}' but the provider supplies category '{replacementOrder.Provider.Category.Name}'. All items must match the provider's category.",
                        "BLL");
            }

            // Auto-set status and issued date
            entity.Status = PurchaseOrderStatus.SentToProvider;
            entity.IssuedDate = DateTime.Now;

            base.Insert(entity);
            Logger.Current.Info($"[AUDIT] PurchaseOrder Created - ID: {entity.Id}, ReplacementOrder: '{replacementOrder.ReplacementOrderNumber}', Status: '{entity.Status}'");
        }

        /// <summary>
        /// Updates an existing purchase order after validating its existence.
        /// </summary>
        public override void Update(PurchaseOrder entity)
        {
            ValidatePurchaseOrder(entity);

            if (!Exists(entity.Id))
                throw new MySystemException($"PurchaseOrder with ID {entity.Id} does not exist.", "BLL");

            base.Update(entity);
            Logger.Current.Info($"[AUDIT] PurchaseOrder Updated - ID: {entity.Id}, Status: '{entity.Status}'");
        }

        /// <summary>
        /// Deletes a purchase order by ID. Only cancelled orders can be deleted.
        /// </summary>
        public override void Delete(Guid id)
        {
            var po = GetById(id);
            if (po == null)
                throw new MySystemException($"PurchaseOrder with ID {id} does not exist.", "BLL");

            // Only allow deletion of cancelled orders
            if (po.Status == PurchaseOrderStatus.SentToProvider)
                throw new MySystemException(
                    $"No se puede eliminar la Orden de Compra {id} porque ya ha sido enviada al proveedor.",
                    "BLL");

            if (po.Status == PurchaseOrderStatus.BillReceived)
                throw new MySystemException(
                    $"No se puede eliminar la Orden de Compra {id} porque ya se ha recibido la factura.",
                    "BLL");

            base.Delete(id);
            Logger.Current.Info($"[AUDIT] PurchaseOrder Deleted - ID: {id}");
        }

        // ========================================
        // PUBLIC METHODS
        // ========================================

        /// <summary>
        /// Marks a purchase order as received, attaches the bill, and updates stock for each order row item.
        /// </summary>
        public void ReceiveOrder(Guid purchaseOrderId, string billFilePath, decimal totalAmount)
        {
            var po = GetById(purchaseOrderId);
            if (po == null)
                throw new MySystemException($"PurchaseOrder with ID {purchaseOrderId} does not exist.", "BLL");

            if (po.Status != PurchaseOrderStatus.SentToProvider)
                throw new MySystemException(
                    $"Cannot receive PurchaseOrder {purchaseOrderId} because its current status is '{po.Status}'. Only orders with status '{PurchaseOrderStatus.SentToProvider}' can be received.",
                    "BLL");

            po.Status = PurchaseOrderStatus.BillReceived;
            po.BillFilePath = billFilePath;
            po.TotalAmount = totalAmount;
            base.Update(po);

            // Update stock for each OrderRow
            foreach (var row in po.ReplacementOrder.OrderRows)
            {
                ItemService.Instance().AddStock(row.Item.Id, row.Quantity);
            }

            Logger.Current.Info($"[AUDIT] PurchaseOrder Received - ID: {purchaseOrderId}, Bill: '{billFilePath}', Total: {totalAmount}, Stock updated for {po.ReplacementOrder.OrderRows.Count} items");
        }

        /// <summary>
        /// Cancels a purchase order. Only orders with status SentToProvider can be cancelled.
        /// </summary>
        public void CancelOrder(Guid purchaseOrderId)
        {
            var po = GetById(purchaseOrderId);
            if (po == null)
                throw new MySystemException($"PurchaseOrder with ID {purchaseOrderId} does not exist.", "BLL");

            if (po.Status != PurchaseOrderStatus.SentToProvider)
                throw new MySystemException(
                    $"Cannot cancel PurchaseOrder {purchaseOrderId} because its current status is '{po.Status}'. Only orders with status '{PurchaseOrderStatus.SentToProvider}' can be cancelled.",
                    "BLL");

            po.Status = PurchaseOrderStatus.Cancelled;
            base.Update(po);

            Logger.Current.Info($"[AUDIT] PurchaseOrder Cancelled - ID: {purchaseOrderId}");
        }

        /// <summary>
        /// Returns all purchase orders linked to the specified replacement order.
        /// </summary>
        public IEnumerable<PurchaseOrder> GetPurchaseOrdersByReplacementOrder(Guid replacementOrderId)
        {
            return GetAll().Where(po => po.ReplacementOrder.Id == replacementOrderId);
        }

        /// <summary>
        /// Returns all non-cancelled purchase orders for the specified provider.
        /// </summary>
        public IEnumerable<PurchaseOrder> GetActiveOrdersByProvider(Guid providerId)
        {
            return GetAll().Where(po =>
                po.ReplacementOrder.Provider.Id == providerId
                && po.Status != PurchaseOrderStatus.Cancelled);
        }

        // ========================================
        // PRIVATE VALIDATION METHODS
        // ========================================

        /// <summary>
        /// Validates that the purchase order entity is not null.
        /// </summary>
        private void ValidatePurchaseOrder(PurchaseOrder entity)
        {
            if (entity == null)
                throw new MySystemException("PurchaseOrder cannot be null.", "BLL");
        }

        /// <summary>
        /// Validates that the replacement order is not null and exists in the system.
        /// </summary>
        private void ValidateReplacementOrder(ReplacementOrder replacementOrder)
        {
            if (replacementOrder == null)
                throw new MySystemException("PurchaseOrder must be linked to a ReplacementOrder.", "BLL");

            if (!ReplacementOrderService.Instance().Exists(replacementOrder.Id))
                throw new MySystemException($"ReplacementOrder with ID {replacementOrder.Id} does not exist in the system.", "BLL");
        }
    }
}
