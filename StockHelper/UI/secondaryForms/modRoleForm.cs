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
using Services.Domain;
using System.Collections;
using Services.Contracts.CustomsException;
using UI.Implementations;

namespace UI.secondaryForms
{
    public partial class modRoleForm : TranslatableForm
    {
        PermissionService _permissionService = PermissionService.Instance();
        LanguageService lang = LanguageService.GetInstance;
        List<Patent> permissions = null;
        List<Family> roles = null;
        public event EventHandler RoleModded;
        /// <summary>
        /// Initialize the modify-role form, center it, load the permissions checked list and the
        /// roles combo box.
        /// </summary>
        public modRoleForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadPermissionsToCheckedList();
            LoadComboBoxWithRoles();
        }

        /// <summary>
        /// Validate that a role and at least one permission are selected, build the role from the
        /// checked permissions, persist the update, raise <see cref="RoleModded"/>, clear the form
        /// and reload the roles combo box.
        /// </summary>
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbRoles.SelectedItem == null)
                {
                    MessageBox.Show(
                        lang.Translate("No Role Selected"),
                        lang.Translate("Validation Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                if (clbPermissions.CheckedItems.Count == 0)
                {
                    MessageBox.Show(
                        lang.Translate("At Least One Permission Required"),
                        lang.Translate("Validation Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                Family roleToMod = new Family();
                roleToMod.Name = roles.FirstOrDefault(r => r.Name == cbRoles.SelectedItem.ToString())!.Name;
                roleToMod.Id = roles.FirstOrDefault(r => r.Name == cbRoles.SelectedItem.ToString())!.Id;

                foreach (var checkedItem in clbPermissions.CheckedItems)
                {
                    roleToMod.AddChild(permissions.FirstOrDefault(p => p.Name == checkedItem.ToString())!);
                }

                _permissionService.Update(roleToMod);

                MessageBox.Show(
                    string.Format(lang.Translate("The role '{0}' was modified successfully"), roleToMod.Name),
                    lang.Translate("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                RoleModded?.Invoke(this, EventArgs.Empty);

                ClearForm();

                LoadComboBoxWithRoles();
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
        /// Load all patents from the permission service and populate the permissions checked list.
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
        /// Load all families from the permission service and populate the roles combo box with their names.
        /// </summary>
        private void LoadComboBoxWithRoles()
        {
            cbRoles.Items.Clear();
            roles = _permissionService.GetAllFamilies();
            foreach (var role in roles)
            {
                cbRoles.Items.Add(role.Name);
            }
        }

        /// <summary>
        /// Reset the permissions checked list and check the patents that belong to the given role.
        /// </summary>
        /// <param name="roleId">Identifier of the role whose patents should be checked.</param>
        private void LoadPermissionsForRole(Guid roleId)
        {
            for(int i = 0; i < clbPermissions.Items.Count; i++)
            {
                clbPermissions.SetItemChecked(i, false);
            }
            clbPermissions.ClearSelected();
            IEnumerable<Patent> perms = new List<Patent>();
            try
            {
                perms = _permissionService.GetFamilyPatents(roleId);
            }
            catch (MySystemException ex)
            {
                ex.Handler();
            }
            catch(Exception ex)
            {
                MessageBox.Show(lang.Translate("unexpected_error") + ": " + ex.Message, lang.Translate("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var item in perms)
            {
                if (clbPermissions.Items.Contains(item.Name))
                {
                    int index = clbPermissions.Items.IndexOf(item.Name);
                    clbPermissions.SetItemChecked(index, true);
                }
            }
        }

        /// <summary>
        /// Load the permissions of the newly selected role into the checked list.
        /// </summary>
        private void cbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRoles.SelectedItem != null)
            {
                Family selectedRole = roles.FirstOrDefault(r => r.Name == cbRoles.SelectedItem.ToString())!;
                if (selectedRole != null)
                {
                    LoadPermissionsForRole(selectedRole.Id);
                }
            }
        }

        /// <summary>
        /// Clear the selected role and uncheck all permissions in the checked list.
        /// </summary>
        private void ClearForm()
        {
            cbRoles.SelectedIndex = -1;
            for (int i = 0; i < clbPermissions.Items.Count; i++)
            {
                clbPermissions.SetItemChecked(i, false);
            }
        }
    }
}
