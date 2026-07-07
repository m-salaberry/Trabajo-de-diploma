using System;

namespace Services.Domain
{
    /// <summary>
    /// Constants for system permissions (by form/module).
    /// Each permission grants access to a specific form or module in the application.
    /// Provides type-safe permission names and prevents typos.
    /// </summary>
    public static class PermissionNames
    {
        // ============================================
        // MANAGEMENT MODULES
        // ============================================
        
        /// <summary>
        /// Permission to access the User Management module.
        /// Grants full access to create, edit, delete, and view users.
        /// </summary>
        public const string UserManagement = "UserManagement";

        /// <summary>
        /// Permission to access the Permission Management module.
        /// Grants full access to assign permissions and manage roles.
        /// </summary>
        public const string PermissionManagement = "PermissionManagement";

        // ============================================
        // CATALOG MANAGMENT MODULES
        // ============================================

        /// <summary>
        /// Permission to access the Item and Category Management module.
        /// Grants access to manage product categories and item details.
        /// </summary>
        public const string ItemCategoryManagment = "ItemCategoryManagment";

        /// <summary>
        /// Permission to access the Supplier Management module.
        /// Grants access to manage supplier information and contacts.
        /// </summary>
        public const string SupplierManagment = "SupplierManagment";

        /// <summary>
        /// Permission to access the Product Builder module.
        /// Grants access to create and manage products.
        /// </summary>
        public const string ProductBuilder = "ProductBuilder";

        // ============================================
        // INVENTORY & PURCHASING MODULES
        // ============================================

        /// <summary>
        /// Permission to access the Stock Management module.
        /// Grants full access to manage stock levels.
        /// </summary>
        public const string StockManagment = "StockManagment";

        /// <summary>
        /// Permission to access the Purchase module.
        /// Grants full access to create and manage purchase orders.
        /// </summary>
        public const string PurchaseManagement = "PurchaseManagement";

        // ============================================
        // REPORTS & ANALYTICS MODULES
        // ============================================

        /// <summary>
        /// Permission to access the Reports module.
        /// Grants access to view and generate reports.
        /// </summary>
        public const string Analytics = "Analytics";

        // ============================================
        // SYSTEM CONFIGURATION MODULES
        // ============================================

        /// <summary>
        /// Permission to access the System Configuration module.
        /// Grants access to system settings and configuration.
        /// </summary>
        public const string SystemConfiguration = "SystemConfiguration";

        /// <summary>
        /// Permission to access the System Logs module.
        /// Grants access to view system logs and audit trails.
        /// </summary>
        public const string SystemLogs = "SystemLogs"; 

    }

}
