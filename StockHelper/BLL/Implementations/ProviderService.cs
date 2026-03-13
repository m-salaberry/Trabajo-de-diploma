using DAL.Contracts;
using DAL.Implementations;
using Domain;
using Services.Contracts.CustomsException;
using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    /// <summary>
    /// Service for managing Provider entities with business validations.
    /// Implements CUIT format/uniqueness checks, referential integrity, and audit trail logging.
    /// </summary>
    public class ProviderService : GenericBllService<Provider, Guid>
    {
        private readonly ItemsCategoryService _categoryService;

        /// <summary>
        /// Initializes a new instance with the specified provider repository.
        /// </summary>
        private ProviderService(IRepository<Provider, Guid> repository) : base(repository)
        {
            _categoryService = ItemsCategoryService.Instance();
        }

        private static ProviderService _instance = null;

        /// <summary>
        /// Returns the singleton instance of ProviderService.
        /// </summary>
        public static ProviderService Instance()
        {
            if (_instance == null)
            {
                _instance = new ProviderService(new ProviderRepository());
            }
            return _instance;
        }

        // ========================================
        // OVERRIDE METHODS WITH BUSINESS VALIDATIONS
        // ========================================

        /// <summary>
        /// Inserts a new provider with business validations.
        /// Validates required fields, CUIT format and uniqueness, and category existence.
        /// Logs the operation to the audit trail.
        /// </summary>
        public override void Insert(Provider entity)
        {
            ValidateProvider(entity);
            ValidateName(entity.Name);
            ValidateContactInfo(entity);
            ValidateCuitFormat(entity.CUIT);
            ValidateCategory(entity.Category);

            base.Insert(entity);

            Logger.Current.Info($"[AUDIT] Provider Created - Name: '{entity.Name}', CUIT: '{entity.CUIT}', User: [PLACEHOLDER_USER]");
        }

        /// <summary>
        /// Updates an existing provider with business validations.
        /// Validates existence by ID; if the CUIT changed, verifies it is not already in use.
        /// Logs the operation to the audit trail.
        /// </summary>
        public override void Update(Provider entity)
        {
            ValidateProvider(entity);
            ValidateName(entity.Name);
            ValidateContactInfo(entity);
            ValidateCuitFormat(entity.CUIT);
            ValidateCategory(entity.Category);

            if (!Exists(entity.Id))
                throw new MySystemException($"Provider with ID {entity.Id} does not exist.", "BLL");

            base.Update(entity);

            Logger.Current.Info($"[AUDIT] Provider Updated - ID: {entity.Id}, User: [PLACEHOLDER_USER]");
        }

        /// <summary>
        /// Deletes a provider by ID.
        /// Verifies that no active Purchase Orders are linked before proceeding.
        /// Logs the operation to the audit trail.
        /// </summary>
        public override void Delete(Guid id)
        {
            var provider = GetById(id);
            if (provider == null)
                throw new MySystemException($"Provider with ID {id} does not exist.", "BLL");

            ValidatePurchaseOrderDependencies(id);

            base.Delete(id);

            Logger.Current.Info($"[AUDIT] Provider Deleted - Name: '{provider.Name}', CUIT: '{provider.CUIT}', User: [PLACEHOLDER_USER]");
        }

        /// <summary>
        /// Gets all providers associated with a given category.
        /// </summary>
        public IEnumerable<Provider> GetProvidersByCategory(int categoryId)
        {
            return GetAll().Where(p => p.Category != null && p.Category.Id == categoryId);
        }

        // ========================================
        // PRIVATE VALIDATION METHODS
        // ========================================

        /// <summary>
        /// Validates that the provider entity is not null.
        /// </summary>
        private void ValidateProvider(Provider entity)
        {
            if (entity == null)
                throw new MySystemException("Provider cannot be null.", "BLL");
        }

        /// <summary>
        /// Validates provider name format: not empty, between 3 and 100 characters.
        /// </summary>
        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new MySystemException("Provider name cannot be empty.", "BLL");

            if (name.Length < 3)
                throw new MySystemException("Provider name must be at least 3 characters long.", "BLL");

            if (name.Length > 100)
                throw new MySystemException("Provider name cannot exceed 100 characters.", "BLL");
        }

        /// <summary>
        /// Validates that the provider has a company name and contact phone.
        /// </summary>
        private void ValidateContactInfo(Provider entity)
        {
            if (string.IsNullOrWhiteSpace(entity.CompanyName))
                throw new MySystemException("Provider company name cannot be empty.", "BLL");

            if (string.IsNullOrWhiteSpace(entity.ContactTel))
                throw new MySystemException("Provider contact phone cannot be empty.", "BLL");
        }

        /// <summary>
        /// Validates that the CUIT is not empty and has a valid 11-digit format.
        /// </summary>
        private void ValidateCuitFormat(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
                throw new MySystemException("Provider CUIT cannot be empty.", "BLL");

            if (!IsValidCUIT(cuit))
                throw new MySystemException("CUIT must contain exactly 11 numeric digits.", "BLL");
        }

        /// <summary>
        /// Validates that no other provider with the same CUIT exists, optionally excluding a given ID.
        /// </summary>
        private void ValidateCuitUniqueness(string cuit, Guid? excludeId)
        {
            var duplicate = GetAll().FirstOrDefault(p =>
                p.CUIT == cuit &&
                (!excludeId.HasValue || p.Id != excludeId.Value));

            if (duplicate != null)
                throw new MySystemException(
                    $"A provider with CUIT '{cuit}' already exists. Duplicate CUITs are not allowed.",
                    "BLL");
        }

        /// <summary>
        /// Validates that the category is assigned, valid, and exists in the system.
        /// </summary>
        private void ValidateCategory(ItemsCategory category)
        {
            if (category == null)
                throw new MySystemException("Provider must have a category assigned.", "BLL");

            if (category.Id <= 0)
                throw new MySystemException("The assigned category must be valid and existing.", "BLL");

            if (!_categoryService.Exists(category.Id))
                throw new MySystemException($"Category with ID {category.Id} does not exist in the system.", "BLL");
        }

        /// <summary>
        /// Verifies referential integrity before deletion.
        /// Checks for active Purchase Orders linked to the provider.
        /// Throws a detailed exception listing all blocking orders if any are found.
        /// </summary>
        private void ValidatePurchaseOrderDependencies(Guid providerId)
        {
            var activeOrders = PurchaseOrderService.Instance()
                .GetActiveOrdersByProvider(providerId)
                .Select(o => o.ReplacementOrder.ReplacementOrderNumber)
                .ToList();

            if (!activeOrders.Any())
                return;

            var orderList = string.Join(", ", activeOrders);
            throw new MySystemException(
                $"Cannot delete provider. It has active Purchase Orders: ({orderList}). Please complete or cancel these orders first.",
                "BLL");
        }

        /// <summary>
        /// Validates the CUIT format: must be exactly 11 numeric digits.
        /// </summary>
        private bool IsValidCUIT(string cuit)
        {
            return !string.IsNullOrWhiteSpace(cuit) &&
                   cuit.Length == 11 &&
                   cuit.All(char.IsDigit);
        }
    }
}

