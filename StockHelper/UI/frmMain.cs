using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Services.Domain;
using Services.Implementations;
using Services.Contracts.Logs;
using UI.secondaryForms;
using UI.Helpers;
using UI.controlForms;

namespace UI
{
    public partial class frmMain : Form
    {
        private PermissionService _permissionService;
        private User currentUser;
        private static frmMain _instance;

        private frmMain(User logedUser)
        {
            InitializeComponent();

            _permissionService = PermissionService.Instance();
            currentUser = logedUser;

            ResetMainPanelSize();
            this.CenterToScreen();
            this.Text = $"StockHelper - Logged in as: {currentUser.Name}";

            // Configure UI based on user permissions
            ConfigurePermissions();

            Logger.Current.Info($"Main form initialized for user '{currentUser.Name}'");
        }

        public static frmMain GetInstance(User logedUser = null!)
        {
            if (_instance == null || _instance.IsDisposed)
            {
                _instance = new frmMain(logedUser);
            }
            return _instance;
        }

        /// <summary>
        /// Configures menu visibility and state based on user permissions.
        /// This method is called during form initialization to hide/show menu items
        /// according to what the current user is allowed to access.
        /// Main menus are only visible if user has at least one sub-menu permission.
        /// </summary>
        private void ConfigurePermissions()
        {
            // ============================================
            // CONFIGURE USERS AND PERMISSIONS MENU
            // ============================================
            
            // Check permissions for sub-menu items
            bool hasUserManagement = currentUser.HasPermission(PermissionNames.UserManagement);
            bool hasPermissionManagement = currentUser.HasPermission(PermissionNames.PermissionManagement);
            
            // Configure sub-menu items visibility
            tsmUsers.Visible = hasUserManagement;
            tsmPerms.Visible = hasPermissionManagement;
            
            // Show parent menu only if at least one sub-menu is visible
            tsmUserAndPerms.Visible = hasUserManagement || hasPermissionManagement;

            // ============================================
            // CONFIGURE CATALOG MANAGEMENT MENU
            // ============================================
            
            // Check permissions for sub-menu items
            bool hasItemCategoryManagment = currentUser.HasPermission(PermissionNames.ItemCategoryManagment);
            bool hasSupplierManagment = currentUser.HasPermission(PermissionNames.SupplierManagment);
            bool hasProductBuilder = currentUser.HasPermission(PermissionNames.ProductBuilder);
            
            // Configure sub-menu items visibility
            tsmItemsAndCategories.Visible = hasItemCategoryManagment;
            tsmProviders.Visible = hasSupplierManagment;
            tsmProductBuilder.Visible = hasProductBuilder;
            
            // Show parent menu only if at least one sub-menu is visible
            tsmCatalogManagment.Visible = hasItemCategoryManagment || 
                                          hasSupplierManagment || 
                                          hasProductBuilder;

            // ============================================
            // CONFIGURE INVENTORY AND PURCHASING MENU
            // ============================================
            
            // Check permissions for sub-menu items
            bool hasStockManagment = currentUser.HasPermission(PermissionNames.StockManagment);
            bool hasPurchaseManagement = currentUser.HasPermission(PermissionNames.PurchaseManagement);
            bool hasInventoryReports = currentUser.HasPermission(PermissionNames.InventoryReports);
            
            // Configure sub-menu items visibility
            tsmStockManagment.Visible = hasStockManagment;
            tsmOrders.Visible = hasPurchaseManagement;
            tsmAnalytics.Visible = hasInventoryReports;
            
            // Show parent menu only if at least one sub-menu is visible
            tsmInventoryAndPurchasing.Visible = hasStockManagment || 
                                                hasPurchaseManagement || 
                                                hasInventoryReports;

            // ============================================
            // LOGGING
            // ============================================
            
            Logger.Current.Info($"Menu configured for user '{currentUser.Name}' - " +
                $"Visible Menus: " +
                $"UserAndPerms={tsmUserAndPerms.Visible} (Users={tsmUsers.Visible}, Perms={tsmPerms.Visible}), " +
                $"Catalog={tsmCatalogManagment.Visible} (Items={tsmItemsAndCategories.Visible}, Providers={tsmProviders.Visible}, Builder={tsmProductBuilder.Visible}), " +
                $"Inventory={tsmInventoryAndPurchasing.Visible} (Stock={tsmStockManagment.Visible}, Orders={tsmOrders.Visible}, Analytics={tsmAnalytics.Visible})");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Get all atomic permissions using extension method
            List<Patent> userPermissions = currentUser.GetAllAtomicPermissions();

            // Detailed logging only in test environment
            if (NativeMethods.testEnvironment)
            {
                Console.WriteLine($"\n=== User Permissions for '{currentUser.Name}' ===");
                Console.WriteLine($"Total permissions: {userPermissions.Count}");

                List<Family> roles = currentUser.GetRoles();
                Console.WriteLine($"Roles: {string.Join(", ", roles.Select(r => r.Name))}");

                Console.WriteLine("\nPermissions:");
                foreach (var permission in userPermissions)
                {
                    Console.WriteLine($"  - {permission.Name}");
                }
                Console.WriteLine("=====================================\n");
            }
            else
            {
                // Simple log in production
                Logger.Current.Info(
                    $"User '{currentUser.Name}' loaded main form with {userPermissions.Count} permissions");
            }
        }

        private void tsmUsers_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access User Management");

            // Verify permission before opening the module
            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.UserManagement,
                "User Management"))
            {
                return; // Error message already shown by helper
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened User Management module");

            ctrlUsers userManagement = new ctrlUsers();
            showContent(userManagement);
        }

        private void tsmPerms_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Permission Management");

            // Verify permission before opening the module
            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.PermissionManagement,
                "Permission Management"))
            {
                return; // Error message already shown by helper
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Permission Management module");

            ctrlPermsissions permissionManagement = new ctrlPermsissions();
            showContent(permissionManagement);
        }

        private void tsmItemsAndCategories_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Items and Categories");
            
            // Verify permission before opening the module
            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.ItemCategoryManagment,
                "Items and Categories"))
            {
                return; // Error message already shown by helper
            }
            
            Logger.Current.Info($"User '{currentUser.Name}' opened Items and Categories module");
            ctrlItemsAndCategories itemsAndCategories = new ctrlItemsAndCategories();
            showContent(itemsAndCategories);
        }

        private void tsmProviders_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Providers");
            
            // Verify permission before opening the module
            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.SupplierManagment,
                "Providers"))
            {
                return; // Error message already shown by helper
            }
            
            Logger.Current.Info($"User '{currentUser.Name}' opened Providers module");
            ctrlProviders providers = new ctrlProviders();
            showContent(providers);
        }

        private void tsmProductBuilder_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Product Builder");
            
            // Verify permission before opening the module
            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.ProductBuilder,
                "Product Builder"))
            {
                return; // Error message already shown by helper
            }
            
            Logger.Current.Info($"User '{currentUser.Name}' opened Product Builder module");
            // TODO: Implement ctrlProductBuilder when ready
            MessageBox.Show("Product Builder module coming soon!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsmStockManagment_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Stock Management");
            
            // Verify permission before opening the module
            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.StockManagment,
                "Stock Management"))
            {
                return; // Error message already shown by helper
            }
            
            Logger.Current.Info($"User '{currentUser.Name}' opened Stock Management module");
            // TODO: Implement ctrlStockManagement when ready
            MessageBox.Show("Stock Management module coming soon!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsmOrders_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Orders");
            
            // Verify permission before opening the module
            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.PurchaseManagement,
                "Orders"))
            {
                return; // Error message already shown by helper
            }
            
            Logger.Current.Info($"User '{currentUser.Name}' opened Orders module");
            // TODO: Implement ctrlOrders when ready
            MessageBox.Show("Orders module coming soon!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsmAnalytics_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Analytics");
            
            // Verify permission before opening the module
            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.InventoryReports,
                "Analytics"))
            {
                return; // Error message already shown by helper
            }
            
            Logger.Current.Info($"User '{currentUser.Name}' opened Analytics module");
            // TODO: Implement ctrlAnalytics when ready
            MessageBox.Show("Analytics module coming soon!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void showContent(UserControl newContent)
        {
            this.MinimumSize = new Size(0, 0);
            panelContainerMain.Controls.Clear();
            newContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelContainerMain.Controls.Add(newContent);
            newContent.BringToFront();
        }

        public void ResetMainPanelSize()
        {
            this.MinimumSize = new Size(800, 600);
        }

        /// <summary>
        /// Gets the current logged-in user.
        /// </summary>
        public User CurrentUser => currentUser;
    }
}
