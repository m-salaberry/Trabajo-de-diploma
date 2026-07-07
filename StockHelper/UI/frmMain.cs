using BLL.Implementations;
using Services.Contracts.Logs;
using Services.Domain;
using Services.Implementations;
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
using UI.controlForms;
using UI.Helpers;
using UI.Implementations;
using UI.secondaryForms;

namespace UI
{
    public partial class frmMain : TranslatableForm
    {
        private PermissionService _permissionService;
        private User currentUser;
        private static frmMain _instance;

        /// <summary>True when this form was closed via the Log Out action (as opposed to app exit).</summary>
        public bool LoggedOut { get; private set; }

        /// <summary>
        /// Private constructor that initializes the main form for the logged-in user.
        /// </summary>
        private frmMain(User logedUser)
        {
            InitializeComponent();

            _permissionService = PermissionService.Instance();
            currentUser = logedUser;

            ResetMainPanelSize();
            this.CenterToScreen();

            var lang = LanguageService.GetInstance;
            this.Text = $"{lang.Translate("StockHelper - Logged in as:")} {currentUser.Name}";

            ConfigurePermissions();

            Logger.Current.Info($"Main form initialized for user '{currentUser.Name}'");
        }

        /// <summary>
        /// Gets the singleton instance of the main form.
        /// </summary>
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
            bool hasSystemLogs = currentUser.HasPermission(PermissionNames.SystemLogs);
            tsmLogs.Visible = hasSystemLogs;

            bool hasUserManagement = currentUser.HasPermission(PermissionNames.UserManagement);
            bool hasPermissionManagement = currentUser.HasPermission(PermissionNames.PermissionManagement);

            tsmUsers.Visible = hasUserManagement;
            tsmPerms.Visible = hasPermissionManagement;

            tsmUserAndPerms.Visible = hasUserManagement || hasPermissionManagement;

            bool hasItemCategoryManagment = currentUser.HasPermission(PermissionNames.ItemCategoryManagment);
            bool hasSupplierManagment = currentUser.HasPermission(PermissionNames.SupplierManagment);
            bool hasProductBuilder = currentUser.HasPermission(PermissionNames.ProductBuilder);

            tsmItemsAndCategories.Visible = hasItemCategoryManagment;
            tsmProviders.Visible = hasSupplierManagment;
            tsmProductBuilder.Visible = hasProductBuilder;

            tsmCatalogManagment.Visible = hasItemCategoryManagment ||
                                          hasSupplierManagment ||
                                          hasProductBuilder;

            bool hasStockManagment = currentUser.HasPermission(PermissionNames.StockManagment);
            bool hasPurchaseManagement = currentUser.HasPermission(PermissionNames.PurchaseManagement);
            bool hasAnalytics = currentUser.HasPermission(PermissionNames.Analytics);

            tsmStockManagment.Visible = hasStockManagment;
            tsmOrders.Visible = hasPurchaseManagement;
            tsmPurchase.Visible = hasPurchaseManagement;
            tsmAnalytics.Visible = hasAnalytics;

            tsmInventoryAndPurchasing.Visible = hasStockManagment ||
                                                hasPurchaseManagement ||
                                                hasAnalytics;

            Logger.Current.Info($"Menu configured for user '{currentUser.Name}' - " +
                $"Visible Menus: " +
                $"UserAndPerms={tsmUserAndPerms.Visible} (Users={tsmUsers.Visible}, Perms={tsmPerms.Visible}), " +
                $"Catalog={tsmCatalogManagment.Visible} (Items={tsmItemsAndCategories.Visible}, Providers={tsmProviders.Visible}, Builder={tsmProductBuilder.Visible}), " +
                $"Inventory={tsmInventoryAndPurchasing.Visible} (Stock={tsmStockManagment.Visible}, Orders={tsmOrders.Visible}, Purchase={tsmPurchase.Visible}, Analytics={tsmAnalytics.Visible})");
        }

        /// <summary>
        /// Handles the form load event, logging user permissions.
        /// </summary>
        private void frmMain_Load(object sender, EventArgs e)
        {
            List<Patent> userPermissions = currentUser.GetAllAtomicPermissions();

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
                Logger.Current.Info(
                    $"User '{currentUser.Name}' loaded main form with {userPermissions.Count} permissions");
            }
        }

        /// <summary>
        /// Opens the User Management module.
        /// </summary>
        private void tsmUsers_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access User Management");

            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.UserManagement,
                "User Management"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened User Management module");

            ctrlUsers userManagement = new ctrlUsers();
            showContent(userManagement);
        }

        /// <summary>
        /// Opens the Permission Management module.
        /// </summary>
        private void tsmPerms_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Permission Management");

            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.PermissionManagement,
                "Permission Management"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Permission Management module");

            ctrlPermsissions permissionManagement = new ctrlPermsissions();
            showContent(permissionManagement);
        }

        /// <summary>
        /// Opens the Items and Categories module.
        /// </summary>
        private void tsmItemsAndCategories_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Items and Categories");

            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.ItemCategoryManagment,
                "Items and Categories"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Items and Categories module");
            ctrlItemsAndCategories itemsAndCategories = new ctrlItemsAndCategories();
            showContent(itemsAndCategories);
        }

        /// <summary>
        /// Opens the Providers module.
        /// </summary>
        private void tsmProviders_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Providers");

            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.SupplierManagment,
                "Providers"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Providers module");
            ctrlProviders providers = new ctrlProviders();
            showContent(providers);
        }

        /// <summary>
        /// Opens the Product Builder module.
        /// </summary>
        private void tsmProductBuilder_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Product Builder");

            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.ProductBuilder,
                "Product Builder"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Product Builder module");
            ctrlProductBuilder productBuilder = new ctrlProductBuilder();
            showContent(productBuilder);
        }

        /// <summary>
        /// Opens the Stock Management module.
        /// </summary>
        private void tsmStockManagment_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Stock Management");

            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.StockManagment,
                "Stock Management"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Stock Management module");
            ctrlStock stockManagement = new ctrlStock();
            showContent(stockManagement);
        }

        /// <summary>
        /// Opens the Replacement Orders module.
        /// </summary>
        private void tsmOrders_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Replacement Orders");

            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.PurchaseManagement,
                "Replacement Orders"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Replacement Orders module");
            ctrlOrders orders = new ctrlOrders();
            showContent(orders);
        }

        /// <summary>
        /// Opens the Purchase Orders module.
        /// </summary>
        private void tsmPurchase_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Purchase Orders");

            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.PurchaseManagement,
                "Purchase Orders"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Purchase Orders module");
            ctrlPurchase purchase = new ctrlPurchase();
            showContent(purchase);
        }

        /// <summary>
        /// Opens the Analytics module.
        /// </summary>
        private void tsmAnalytics_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Analytics");

            if (!UIPermissionHelper.CanAccessForm(
                currentUser,
                PermissionNames.Analytics,
                "Analytics"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Analytics module");
            ctrlAnalytics analytics = new ctrlAnalytics();
            showContent(analytics);
        }

        /// <summary>
        /// Opens the Configuration module. Accessible to all users without permission checks.
        /// </summary>
        private void tsmConfiguration_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' opened Configuration module");
            ctrlConfiguration configuration = new ctrlConfiguration();
            showContent(configuration);
        }

        /// <summary>
        /// Opens the Logs module. Accessible to all users without permission checks.
        /// </summary>
        private void tsmLogs_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' opened Logs module");
            ctrlLogs logs = new ctrlLogs();
            showContent(logs);
        }

        /// <summary>
        /// Logout the current user and return to the login form.
        /// </summary>
        private void tsmLogOut_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser?.Name}' logged out");
            LoggedOut = true;
            _instance = null;
            this.Close();
        }

        /// <summary>
        /// Displays a UserControl in the main content panel.
        /// </summary>
        private void showContent(UserControl newContent)
        {
            this.MinimumSize = new Size(0, 0);
            while (panelContainerMain.Controls.Count > 0)
            {
                Control old = panelContainerMain.Controls[0];
                panelContainerMain.Controls.Remove(old);
                old.Dispose();
            }
            newContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelContainerMain.Controls.Add(newContent);
            newContent.BringToFront();
        }

        /// <summary>
        /// Resets the main panel to its default minimum size.
        /// </summary>
        public void ResetMainPanelSize()
        {
            this.MinimumSize = new Size(800, 600);
        }

        /// <summary>
        /// Gets the current logged-in user.
        /// </summary>
        public User CurrentUser => currentUser;

        /// <summary>
        /// Applies the current language translations to all translatable controls in the main form.
        /// </summary>
        public override void ApplyTranslations()
        {
            var lang = LanguageService.GetInstance;

            this.Text = $"{lang.Translate("StockHelper - Logged in as:")} {currentUser.Name}";

            tsmSystem.Text = lang.Translate("System");
            tsmConfiguration.Text = lang.Translate("Configuration");
            tsmLogs.Text = lang.Translate("Logs");

            tsmUserAndPerms.Text = lang.Translate("Users and Permissions");
            tsmUsers.Text = lang.Translate("Users Managment");
            tsmPerms.Text = lang.Translate("Permissions Managment");

            tsmCatalogManagment.Text = lang.Translate("Catalog Managment");
            tsmItemsAndCategories.Text = lang.Translate("Items and Categories");
            tsmProviders.Text = lang.Translate("Providers");
            tsmProductBuilder.Text = lang.Translate("Product Builder");

            tsmInventoryAndPurchasing.Text = lang.Translate("Inventory and Purchasing");
            tsmStockManagment.Text = lang.Translate("Stock Managment");
            tsmOrders.Text = lang.Translate("Replacement Orders");
            tsmPurchase.Text = lang.Translate("Purchase Orders");
            tsmAnalytics.Text = lang.Translate("Analytics");
        }
    }
}
