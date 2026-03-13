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
    public class OrderRowService : GenericBllService<OrderRow, Guid>
    {
        private readonly OrderRowRepository _typedRepository;

        /// <summary>
        /// Initializes a new instance with the specified order row repository.
        /// </summary>
        private OrderRowService(OrderRowRepository repository) : base(repository)
        {
            _typedRepository = repository;
        }

        private static OrderRowService _instance = null;

        /// <summary>
        /// Returns the singleton instance of OrderRowService.
        /// </summary>
        public static OrderRowService Instance()
        {
            if (_instance == null)
            {
                _instance = new OrderRowService(new OrderRowRepository());
            }
            return _instance;
        }

        // ========================================
        // OVERRIDE METHODS WITH BUSINESS VALIDATIONS
        // ========================================

        /// <summary>
        /// Inserts a new order row. If a row with the same item already exists in the order, merges by summing quantities.
        /// </summary>
        public override void Insert(OrderRow entity)
        {
            ValidateOrderRow(entity);
            ValidateItem(entity.Item);
            ValidateQuantity(entity.Quantity);
            ValidateReplacementOrder(entity.ReplacementOrder);

            // Anti-duplicity: if a row with the same Item already exists in this order, sum quantities
            var existingRows = GetRowsByReplacementOrderId(entity.ReplacementOrder.Id);
            var duplicate = existingRows.FirstOrDefault(r => r.Item.Id == entity.Item.Id);
            if (duplicate != null)
            {
                duplicate.Quantity += entity.Quantity;
                base.Update(duplicate);
                Logger.Current.Info($"[AUDIT] OrderRow merged - Item '{entity.Item.Name}' quantity added to existing row {duplicate.Id} in ReplacementOrder {entity.ReplacementOrder.Id}");
                return;
            }

            base.Insert(entity);
            Logger.Current.Info($"[AUDIT] OrderRow Created - ID: {entity.Id}, Item: '{entity.Item.Name}', Quantity: {entity.Quantity}, ReplacementOrder: {entity.ReplacementOrder.Id}");
        }

        /// <summary>
        /// Updates an existing order row after validating its item and quantity.
        /// </summary>
        public override void Update(OrderRow entity)
        {
            ValidateOrderRow(entity);
            ValidateItem(entity.Item);
            ValidateQuantity(entity.Quantity);

            if (!Exists(entity.Id))
                throw new MySystemException($"OrderRow with ID {entity.Id} does not exist.", "BLL");

            base.Update(entity);
            Logger.Current.Info($"[AUDIT] OrderRow Updated - ID: {entity.Id}, Item: '{entity.Item.Name}', Quantity: {entity.Quantity}");
        }

        /// <summary>
        /// Deletes an order row by ID after verifying it exists.
        /// </summary>
        public override void Delete(Guid id)
        {
            var row = GetById(id);
            if (row == null)
                throw new MySystemException($"OrderRow with ID {id} does not exist.", "BLL");

            base.Delete(id);
            Logger.Current.Info($"[AUDIT] OrderRow Deleted - ID: {id}, Item: '{row.Item.Name}', Quantity: {row.Quantity}");
        }

        // ========================================
        // PUBLIC METHODS
        // ========================================

        /// <summary>
        /// Returns all order rows belonging to the specified replacement order.
        /// </summary>
        public IEnumerable<OrderRow> GetRowsByReplacementOrderId(Guid replacementOrderId)
        {
            return _typedRepository.GetByReplacementOrderId(replacementOrderId);
        }

        /// <summary>
        /// Returns true if the specified item is referenced in any non-cancelled, non-received purchase order.
        /// </summary>
        public bool IsItemInPendingOrders(Guid itemId)
        {
            var rowsWithItem = GetAll().Where(r => r.Item.Id == itemId);
            if (!rowsWithItem.Any())
                return false;

            var allPurchaseOrders = PurchaseOrderService.Instance().GetAll();

            foreach (var row in rowsWithItem)
            {
                var linkedPOs = allPurchaseOrders
                    .Where(po => po.ReplacementOrder.Id == row.ReplacementOrder.Id
                              && po.Status != PurchaseOrderStatus.Cancelled
                              && po.Status != PurchaseOrderStatus.BillReceived);

                if (linkedPOs.Any())
                    return true;
            }

            return false;
        }

        // ========================================
        // PRIVATE VALIDATION METHODS
        // ========================================

        /// <summary>
        /// Validates that the order row entity is not null.
        /// </summary>
        private void ValidateOrderRow(OrderRow entity)
        {
            if (entity == null)
                throw new MySystemException("OrderRow cannot be null.", "BLL");
        }

        /// <summary>
        /// Validates that the item is not null and exists in the system.
        /// </summary>
        private void ValidateItem(Item item)
        {
            if (item == null)
                throw new MySystemException("OrderRow must have an Item assigned.", "BLL");

            if (!ItemService.Instance().Exists(item.Id))
                throw new MySystemException($"Item with ID {item.Id} does not exist in the system.", "BLL");
        }

        /// <summary>
        /// Validates that the quantity is greater than zero.
        /// </summary>
        private void ValidateQuantity(decimal quantity)
        {
            if (quantity <= 0)
                throw new MySystemException("OrderRow quantity must be greater than zero.", "BLL");
        }

        /// <summary>
        /// Validates that the replacement order reference is not null.
        /// </summary>
        private void ValidateReplacementOrder(ReplacementOrder replacementOrder)
        {
            if (replacementOrder == null)
                throw new MySystemException("OrderRow must be associated with a ReplacementOrder.", "BLL");
        }
    }
}
