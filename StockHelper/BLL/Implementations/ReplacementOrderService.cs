using DAL.Contracts;
using DAL.Implementations;
using Domain;
using Services.Contracts.CustomsException;
using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Implementations
{
    public class ReplacementOrderService : GenericBllService<ReplacementOrder, Guid>
    {
        private readonly ReplacementOrderRepository _typedRepository;

        /// <summary>
        /// Initializes a new instance with the specified replacement order repository.
        /// </summary>
        private ReplacementOrderService(ReplacementOrderRepository repository) : base(repository)
        {
            _typedRepository = repository;
        }

        private static ReplacementOrderService _instance = null;

        /// <summary>
        /// Returns the singleton instance of ReplacementOrderService.
        /// </summary>
        public static ReplacementOrderService Instance()
        {
            if (_instance == null)
            {
                _instance = new ReplacementOrderService(new ReplacementOrderRepository());
            }
            return _instance;
        }

        // ========================================
        // OVERRIDE METHODS WITH BUSINESS VALIDATIONS
        // ========================================

        /// <summary>
        /// Inserts a new replacement order with its rows. Generates the order number and persists each row via OrderRowService.
        /// </summary>
        public override void Insert(ReplacementOrder entity)
        {
            ValidateReplacementOrder(entity);
            ValidateProvider(entity.Provider);
            ValidateOrderRows(entity.OrderRows);

            // Generate replacement order number
            int seqNumber = _typedRepository.GetNextSequentialNumber(entity.Provider.Id);
            entity.ReplacementOrderNumber = entity.GenerateReplacementOrderNumber(seqNumber);

            // Insert the header first
            base.Insert(entity);

            // Insert each OrderRow via OrderRowService
            foreach (var row in entity.OrderRows)
            {
                row.ReplacementOrder = entity;
                OrderRowService.Instance().Insert(row);
            }

            Logger.Current.Info($"[AUDIT] ReplacementOrder Created - ID: {entity.Id}, Number: '{entity.ReplacementOrderNumber}', Provider: '{entity.Provider.Name}', Rows: {entity.OrderRows.Count}");
        }

        /// <summary>
        /// Updates a replacement order and synchronizes its order rows (inserts new, updates existing).
        /// </summary>
        public override void Update(ReplacementOrder entity)
        {
            ValidateReplacementOrder(entity);
            ValidateOrderRows(entity.OrderRows);

            if (!Exists(entity.Id))
                throw new MySystemException($"ReplacementOrder with ID {entity.Id} does not exist.", "BLL");

            base.Update(entity);

            // Persist each OrderRow change
            foreach (var row in entity.OrderRows)
            {
                row.ReplacementOrder = entity;
                if (OrderRowService.Instance().Exists(row.Id))
                    OrderRowService.Instance().Update(row);
                else
                    OrderRowService.Instance().Insert(row);
            }

            Logger.Current.Info($"[AUDIT] ReplacementOrder Updated - ID: {entity.Id}, Number: '{entity.ReplacementOrderNumber}'");
        }

        /// <summary>
        /// Deletes a replacement order by ID. Blocks deletion if a purchase order is linked.
        /// </summary>
        public override void Delete(Guid id)
        {
            var order = GetById(id);
            if (order == null)
                throw new MySystemException($"ReplacementOrder with ID {id} does not exist.", "BLL");

            // Block deletion if a PurchaseOrder is linked
            var linkedPO = PurchaseOrderService.Instance().GetAll()
                .FirstOrDefault(po => po.ReplacementOrder.Id == id);

            if (linkedPO != null)
                throw new MySystemException(
                    $"Cannot delete ReplacementOrder '{order.ReplacementOrderNumber}' because it is linked to PurchaseOrder {linkedPO.Id}.",
                    "BLL");

            base.Delete(id);
            Logger.Current.Info($"[AUDIT] ReplacementOrder Deleted - ID: {id}, Number: '{order.ReplacementOrderNumber}'");
        }

        // ========================================
        // PUBLIC METHODS
        // ========================================

        /// <summary>
        /// Returns all replacement orders associated with the specified provider.
        /// </summary>
        public IEnumerable<ReplacementOrder> GetReplacementOrdersByProvider(Guid providerId)
        {
            return GetAll().Where(ro => ro.Provider.Id == providerId);
        }

        // ========================================
        // PRIVATE VALIDATION METHODS
        // ========================================

        /// <summary>
        /// Validates that the replacement order entity is not null.
        /// </summary>
        private void ValidateReplacementOrder(ReplacementOrder entity)
        {
            if (entity == null)
                throw new MySystemException("ReplacementOrder cannot be null.", "BLL");
        }

        /// <summary>
        /// Validates that the provider is not null and exists in the system.
        /// </summary>
        private void ValidateProvider(Provider provider)
        {
            if (provider == null)
                throw new MySystemException("ReplacementOrder must have a Provider assigned.", "BLL");

            if (!ProviderService.Instance().Exists(provider.Id))
                throw new MySystemException($"Provider with ID {provider.Id} does not exist in the system.", "BLL");
        }

        /// <summary>
        /// Validates that the order rows list is not null or empty.
        /// </summary>
        private void ValidateOrderRows(List<OrderRow> orderRows)
        {
            if (orderRows == null || !orderRows.Any())
                throw new MySystemException("ReplacementOrder must have at least one OrderRow.", "BLL");
        }
    }
}
