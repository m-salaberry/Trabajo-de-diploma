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

namespace UI.secondaryForms
{
    public partial class deleteRoleForm : Form
    {
        PermissionService _permissionService = PermissionService.Instance();
        LanguageService lang = LanguageService.GetInstance;
        List<Family> roles = null;

        public event EventHandler RoleDeleted;

        public deleteRoleForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadRolesToCheckedList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<Family> rolesToDelete = clbRoles.CheckedItems
                    .Cast<string>()
                    .Select(roleName => roles.FirstOrDefault(r => r.Name == roleName))
                    .Where(r => r != null)
                    .ToList()!;
                if (rolesToDelete.Count < 1)
                {
                    MessageBox.Show(
                        lang.Translate("Please select at least one role to delete."),
                        lang.Translate("No Role Selected"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                foreach (var role in rolesToDelete)
                {
                    _permissionService.Delete(role.Id);
                }
                MessageBox.Show(
                    lang.Translate("Selected roles have been deleted successfully."),
                    lang.Translate("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                RoleDeleted?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (MySystemException msg)
            {
                MessageBox.Show(
                    string.Format(lang.Translate("Operation failed: {0}"), lang.Translate(msg.Message)),
                    lang.Translate("Operation Failed"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(lang.Translate("An error occurred: {0}"), ex.Message),
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadRolesToCheckedList()
        {
            roles = _permissionService.GetAllFamilies();
            clbRoles.Items.Clear();
            foreach (var role in roles)
            {
                clbRoles.Items.Add(role.Name);
            }
        }

    }
}
