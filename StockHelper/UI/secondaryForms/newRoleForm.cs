using Services.Contracts.CustomsException;
using Services.Domain;
using Services.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Implementations;

namespace UI.secondaryForms
{
    public partial class newRoleForm : TranslatableForm
    {
        PermissionService _permissionService = PermissionService.Instance();
        LanguageService lang = LanguageService.GetInstance;
        List<Patent> permissions = null;
        
        public event EventHandler RoleCreated;
        
        /// <summary>
        /// Initializes the form and loads the available permissions into the checked list.
        /// </summary>
        public newRoleForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadPermissionsToCheckedList();
        }

        /// <summary>
        /// Validates the form, builds a new role with the checked permissions, persists it,
        /// raises the RoleCreated event and clears the form.
        /// </summary>
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                {
                    return;
                }
                Family newRole = new Family
                {
                    Name = txtRoleName.Text,
                };
                foreach (var checkedItem in clbPermissions.CheckedItems)
                {
                    var perm = permissions.FirstOrDefault(p => p.Name == checkedItem.ToString());
                    if (perm != null)
                    {
                        newRole.AddChild(perm);
                    }
                }

                _permissionService.Insert(newRole);
                
                MessageBox.Show(
                    string.Format(lang.Translate("The role '{0}' was created successfully"), newRole.Name),
                    lang.Translate("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                
                // Trigger the RoleCreated event
                RoleCreated?.Invoke(this, EventArgs.Empty);
                
                // Clear form
                ClearForm();
            }
            catch (MySystemException ex)
            {
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("Error Creating Role") + ": " + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads all available patents (permissions) and lists their names in the checked list box.
        /// </summary>
        private void LoadPermissionsToCheckedList()
        {
            permissions = _permissionService.GetAllPatents();
            clbPermissions.Items.Clear();
            foreach (var perm in permissions)
            {
                clbPermissions.Items.Add(perm.Name);
            }
        }

        /// <summary>
        /// Validates that a role name is entered and at least one permission is checked.
        /// </summary>
        /// <returns>True if the form is valid; otherwise false.</returns>
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtRoleName.Text))
            {
                MessageBox.Show(
                    lang.Translate("Role Name Required"),
                    lang.Translate("Validation Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
            if (clbPermissions.CheckedItems.Count == 0)
            {
                MessageBox.Show(
                    lang.Translate("At Least One Permission Required"),
                    lang.Translate("Validation Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Clears the role name field and unchecks every permission in the list.
        /// </summary>
        private void ClearForm()
        {
            txtRoleName.Text = "";
            for (int i = 0; i < clbPermissions.Items.Count; i++)
            {
                clbPermissions.SetItemChecked(i, false);
            }
        }
    }
}
