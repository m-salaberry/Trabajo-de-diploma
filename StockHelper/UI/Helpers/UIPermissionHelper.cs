using System;
using System.Windows.Forms;
using Services.Domain;
using Services.Contracts.Logs;

namespace UI.Helpers
{
    /// <summary>
    /// Helper class for managing UI elements based on user permissions.
    /// Simplifies showing/hiding and enabling/disabling UI controls.
    /// </summary>
    public static class UIPermissionHelper
    {
        /// <summary>
        /// Shows or hides a control based on user permission.
        /// </summary>
        /// <param name="control">The control to show/hide</param>
        /// <param name="user">The current user</param>
        /// <param name="requiredPermission">The permission required to show the control</param>
        public static void ShowIfHasPermission(Control control, User user, string requiredPermission)
        {
            if (control == null)
            {
                Logger.Current.Warning($"UIPermissionHelper: Control is null for permission '{requiredPermission}'");
                return;
            }

            if (user == null)
            {
                Logger.Current.Warning($"UIPermissionHelper: User is null for permission '{requiredPermission}'");
                control.Visible = false;
                return;
            }

            control.Visible = user.HasPermission(requiredPermission);
            
            Logger.Current.Debug($"Control '{control.Name}' visibility set to {control.Visible} for permission '{requiredPermission}'");
        }

        /// <summary>
        /// Enables or disables a control based on user permission.
        /// </summary>
        /// <param name="control">The control to enable/disable</param>
        /// <param name="user">The current user</param>
        /// <param name="requiredPermission">The permission required to enable the control</param>
        public static void EnableIfHasPermission(Control control, User user, string requiredPermission)
        {
            if (control == null)
            {
                Logger.Current.Warning($"UIPermissionHelper: Control is null for permission '{requiredPermission}'");
                return;
            }

            if (user == null)
            {
                Logger.Current.Warning($"UIPermissionHelper: User is null for permission '{requiredPermission}'");
                control.Enabled = false;
                return;
            }

            control.Enabled = user.HasPermission(requiredPermission);
            
            Logger.Current.Debug($"Control '{control.Name}' enabled set to {control.Enabled} for permission '{requiredPermission}'");
        }

        /// <summary>
        /// Shows or hides a ToolStripMenuItem based on user permission.
        /// </summary>
        /// <param name="menuItem">The menu item to show/hide</param>
        /// <param name="user">The current user</param>
        /// <param name="requiredPermission">The permission required to show the menu item</param>
        public static void ShowMenuItemIfHasPermission(ToolStripMenuItem menuItem, User user, string requiredPermission)
        {
            if (menuItem == null)
            {
                Logger.Current.Warning($"UIPermissionHelper: MenuItem is null for permission '{requiredPermission}'");
                return;
            }

            if (user == null)
            {
                Logger.Current.Warning($"UIPermissionHelper: User is null for permission '{requiredPermission}'");
                menuItem.Visible = false;
                return;
            }

            menuItem.Visible = user.HasPermission(requiredPermission);
            
            Logger.Current.Debug($"MenuItem '{menuItem.Text}' visibility set to {menuItem.Visible} for permission '{requiredPermission}'");
        }

        /// <summary>
        /// Enables or disables a ToolStripMenuItem based on user permission.
        /// </summary>
        /// <param name="menuItem">The menu item to enable/disable</param>
        /// <param name="user">The current user</param>
        /// <param name="requiredPermission">The permission required to enable the menu item</param>
        public static void EnableMenuItemIfHasPermission(ToolStripMenuItem menuItem, User user, string requiredPermission)
        {
            if (menuItem == null)
            {
                Logger.Current.Warning($"UIPermissionHelper: MenuItem is null for permission '{requiredPermission}'");
                return;
            }

            if (user == null)
            {
                Logger.Current.Warning($"UIPermissionHelper: User is null for permission '{requiredPermission}'");
                menuItem.Enabled = false;
                return;
            }

            menuItem.Enabled = user.HasPermission(requiredPermission);
            
            Logger.Current.Debug($"MenuItem '{menuItem.Text}' enabled set to {menuItem.Enabled} for permission '{requiredPermission}'");
        }

        /// <summary>
        /// Shows or hides a button based on user permission.
        /// </summary>
        /// <param name="button">The button to show/hide</param>
        /// <param name="user">The current user</param>
        /// <param name="requiredPermission">The permission required to show the button</param>
        public static void ShowButtonIfHasPermission(Button button, User user, string requiredPermission)
        {
            ShowIfHasPermission(button, user, requiredPermission);
        }

        /// <summary>
        /// Enables or disables a button based on user permission.
        /// </summary>
        /// <param name="button">The button to enable/disable</param>
        /// <param name="user">The current user</param>
        /// <param name="requiredPermission">The permission required to enable the button</param>
        public static void EnableButtonIfHasPermission(Button button, User user, string requiredPermission)
        {
            EnableIfHasPermission(button, user, requiredPermission);
        }

        /// <summary>
        /// Configures multiple controls at once based on permissions.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="configurations">Array of tuples (control, permission, action)</param>
        public static void ConfigureControls(User user, params (Control control, string permission, PermissionAction action)[] configurations)
        {
            if (configurations == null || configurations.Length == 0)
            {
                return;
            }

            foreach (var (control, permission, action) in configurations)
            {
                switch (action)
                {
                    case PermissionAction.ShowHide:
                        ShowIfHasPermission(control, user, permission);
                        break;
                    case PermissionAction.EnableDisable:
                        EnableIfHasPermission(control, user, permission);
                        break;
                    case PermissionAction.Both:
                        ShowIfHasPermission(control, user, permission);
                        EnableIfHasPermission(control, user, permission);
                        break;
                }
            }
        }

        /// <summary>
        /// Checks if user can access a form and shows error message if not.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="requiredPermission">The permission required to access the form</param>
        /// <param name="formName">The name of the form (for error message)</param>
        /// <returns>True if user has permission, false otherwise</returns>
        public static bool CanAccessForm(User user, string requiredPermission, string formName = "this form")
        {
            if (user == null)
            {
                MessageBox.Show(
                    "User session is invalid. Please log in again.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (!user.HasPermission(requiredPermission))
            {
                Logger.Current.Warning($"User '{user.Name}' attempted to access '{formName}' without permission '{requiredPermission}'");
                
                MessageBox.Show(
                    $"You do not have permission to access {formName}.\n\n" +
                    $"Required permission: {requiredPermission}",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                
                return false;
            }

            Logger.Current.Info($"User '{user.Name}' accessed '{formName}' with permission '{requiredPermission}'");
            return true;
        }

        /// <summary>
        /// Checks if user can perform an action and shows error message if not.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="requiredPermission">The permission required to perform the action</param>
        /// <param name="actionName">The name of the action (for error message)</param>
        /// <returns>True if user has permission, false otherwise</returns>
        public static bool CanPerformAction(User user, string requiredPermission, string actionName = "this action")
        {
            if (user == null)
            {
                MessageBox.Show(
                    "User session is invalid. Please log in again.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (!user.HasPermission(requiredPermission))
            {
                Logger.Current.Warning($"User '{user.Name}' attempted to perform '{actionName}' without permission '{requiredPermission}'");
                
                MessageBox.Show(
                    $"You do not have permission to perform {actionName}.\n\n" +
                    $"Required permission: {requiredPermission}",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                
                return false;
            }

            Logger.Current.Debug($"User '{user.Name}' performed '{actionName}' with permission '{requiredPermission}'");
            return true;
        }
    }

    /// <summary>
    /// Defines the action to take on a control based on permission.
    /// </summary>
    public enum PermissionAction
    {
        /// <summary>
        /// Show or hide the control.
        /// </summary>
        ShowHide,

        /// <summary>
        /// Enable or disable the control.
        /// </summary>
        EnableDisable,

        /// <summary>
        /// Both show/hide and enable/disable.
        /// </summary>
        Both
    }
}
