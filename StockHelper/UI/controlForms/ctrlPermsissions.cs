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
    public partial class ctrlPermsissions : TranslatableUserControls
    {
        List<Family> roles = null;
        PermissionService _permissionService = PermissionService.Instance();
        LanguageService lang = LanguageService.GetInstance;

        /// <summary>
        /// Initializes the control, loads the roles and displays them in the roles grid.
        /// </summary>
        public ctrlPermsissions()
        {
            InitializeComponent();
            LoadRoles();
            LoadRolesToGrid();
        }

        /// <summary>
        /// Removes this control from its parent, resets the main panel size and disposes the control.
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            frmMain.GetInstance().ResetMainPanelSize();
            this.Dispose();
        }

        /// <summary>
        /// Loads all role families from the permission service into the roles collection.
        /// </summary>
        private void LoadRoles()
        {
            roles = _permissionService.GetAllFamilies();
        }

        /// <summary>
        /// Binds the roles collection to the grid, hiding the Id and Children columns and translating the name header.
        /// </summary>
        private void LoadRolesToGrid()
        {
            dgvRoles.DataSource = null;
            dgvRoles.DataSource = roles;
            dgvRoles.Columns["Id"].Visible = false;
            dgvRoles.Columns["Children"].Visible = false;
            dgvRoles.Columns["Name"].HeaderText = lang.Translate("Role Name");
            dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoles.Refresh();
        }

        /// <summary>
        /// Opens the new role dialog and refreshes the roles list when a role is created.
        /// </summary>
        private void btnNewRole_Click(object sender, EventArgs e)
        {
            try
            {
                newRoleForm newRoleForm = new newRoleForm();
                // Subscribe to the RoleCreated event
                newRoleForm.RoleCreated += (s, ev) => RefreshRoleList();
                newRoleForm.ShowDialog();
                newRoleForm.BringToFront();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Reloads the roles from the service and refreshes the roles grid.
        /// </summary>
        public void RefreshRoleList()
        {
            LoadRoles();
            LoadRolesToGrid();
        }

        /// <summary>
        /// Applies the current language translations to the buttons, labels and roles grid.
        /// </summary>
        public override void ApplyTranslations()
        {
            // Translate buttons
            btnNewRole.Text = lang.Translate("New Role");
            btnModifyRole.Text = lang.Translate("Modify Role");
            btnDeleteRole.Text = lang.Translate("Delete Role");

            // Translate labels
            lbRoles.Text = lang.Translate("Roles:");

            // Refresh grid to apply translations to column headers
            if (roles != null)
            {
                LoadRolesToGrid();
            }
        }

        /// <summary>
        /// Opens the modify role dialog and refreshes the roles list when a role is modified.
        /// </summary>
        private void btnModifyRole_Click(object sender, EventArgs e)
        {
            try
            {
                modRoleForm modRoleForm = new modRoleForm();
                // Subscribe to the RolModded event
                modRoleForm.RoleModded += (s, ev) => RefreshRoleList();
                modRoleForm.ShowDialog();
                modRoleForm.BringToFront();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens the delete role dialog and refreshes the roles list when a role is deleted.
        /// </summary>
        private void btnDeleteRole_Click(object sender, EventArgs e)
        {
            try
            {
                deleteRoleForm deleteRoleForm = new deleteRoleForm();
                // Subscribe to the RoleDeleted event
                deleteRoleForm.RoleDeleted += (s, ev) => RefreshRoleList();
                deleteRoleForm.ShowDialog();
                deleteRoleForm.BringToFront();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

    }
}
