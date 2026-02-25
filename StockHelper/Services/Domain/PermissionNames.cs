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
        public const string Reports = "Reports";

        /// <summary>
        /// Permission to access the Inventory Reports module.
        /// Grants access to stock and inventory reports.
        /// </summary>
        public const string InventoryReports = "InventoryReports";

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

        /// <summary>
        /// Permission to access the Database Backup module.
        /// Grants access to backup and restore database.
        /// </summary>
        public const string DatabaseBackup = "DatabaseBackup";

    }

    /// <summary>
    /// Constants for system roles (Families).
    /// Each role is a collection of permissions grouped by job function.
    /// Provides type-safe role names.
    /// </summary>
    public static class RoleNames
    {
        /// <summary>
        /// Administrator role - Full system access to all modules.
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        /// Manager role - Management level access to most operational modules.
        /// </summary>
        public const string Manager = "Manager";

        /// <summary>
        /// Salesperson role - Access to sales and customer-related modules.
        /// </summary>
        public const string Salesperson = "Salesperson";

        /// <summary>
        /// Inventory Manager role - Full access to inventory and stock modules.
        /// </summary>
        public const string InventoryManager = "InventoryManager";

        /// <summary>
        /// Cashier role - Access to Point of Sale and basic sales operations.
        /// </summary>
        public const string Cashier = "Cashier";

        /// <summary>
        /// Auditor role - Read-only access to reports and logs.
        /// </summary>
        public const string Auditor = "Auditor";

        /// <summary>
        /// Warehouse Operator role - Access to warehouse and stock control.
        /// </summary>
        public const string WarehouseOperator = "WarehouseOperator";

        /// <summary>
        /// Accountant role - Access to financial reports and pricing.
        /// </summary>
        public const string Accountant = "Accountant";

        /// <summary>
        /// Guest role - Minimal read-only access (e.g., Dashboard only).
        /// </summary>
        public const string Guest = "Guest";
    }
}
