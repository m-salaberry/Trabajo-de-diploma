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
        public modRoleForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadPermissionsToCheckedList();
            LoadComboBoxWithRoles();
        }

        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            try
            {
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

                // Trigger the RoleCreated event
                RoleModded?.Invoke(this, EventArgs.Empty);

                // Clear form
                ClearForm();

                // Reload roles in combo box
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

        private void LoadPermissionsToCheckedList()
        {
            permissions = _permissionService.GetAllPatents();
            clbPermissions.Items.Clear();
            foreach (var perm in permissions)
            {
                clbPermissions.Items.Add(perm.Name);
            }
        }

        private void LoadComboBoxWithRoles()
        {
            cbRoles.Items.Clear();
            roles = _permissionService.GetAllFamilies();
            foreach (var role in roles)
            {
                cbRoles.Items.Add(role.Name);
            }
        }

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
