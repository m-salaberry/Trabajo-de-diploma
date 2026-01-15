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
    public partial class modUserForm : TranslatableForm
    {
        UserService userService = UserService.Instance();
        PermissionService permissionService = PermissionService.Instance();
        LanguageService lang = LanguageService.GetInstance;
        User userToModify = null;

        public event EventHandler UserUpdated;
        public modUserForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadUserDropdown();
            LoadRoleDropdown();
            ActivateOrDeactivateControls();
        }



        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                {
                    return;
                }
                userToModify.Name = txtUsername.Text;
                userToModify.Password = txtPassword.Text;
                userToModify.Role = cbRoleSelector.Text;
                userToModify.IsActive = ckbActiveUser.Checked;

                userService.Update(userToModify);
                MessageBox.Show(
                    lang.Translate("User modified successfully"),
                    lang.Translate("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ClearForm();
                userToModify = null;
                ActivateOrDeactivateControls();
                LoadUserDropdown();
                UserUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred while modifying the user: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred while modifying the user: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show(
                    lang.Translate("Username is required"),
                    lang.Translate("Validation Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (txtUsername.Text.Length < 3)
            {
                MessageBox.Show(
                    lang.Translate("Username must be at least 3 characters"),
                    lang.Translate("Validation Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(
                    lang.Translate("Password is required"),
                    lang.Translate("Validation Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbRoleSelector.Text))
            {
                MessageBox.Show(
                    lang.Translate("Role is required"),
                    lang.Translate("Validation Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cbRoleSelector.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtRepeatedPassword.Text = "";

        }

        private void LoadRoleDropdown()
        {
            List<string> roles = permissionService.GetAllFamilies().Select(r => r.Name).ToList();
            cbRoleSelector.DataSource = roles;
        }

        private void LoadUserDropdown()
        {
            List<string> users = userService.GetAll().Select(u => u.Name).ToList();
            cbUsers.DataSource = users;
        }

        public override void ApplyTranslations()
        {
            lblSelectUser.Text = lang.Translate("Select user:");
            lbUsername.Text = lang.Translate("Username:");
            lblPassword.Text = lang.Translate("Password:");
            lblRePassword.Text = lang.Translate("Repeat Password:");
            lblRole.Text = lang.Translate("Select role:");
            ckbActiveUser.Text = lang.Translate("Active User");
            btnSaveUser.Text = lang.Translate("Save");
        }

        private void btnLoadUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbUsers.Text))
            {
                MessageBox.Show(
                    lang.Translate("Please enter a username to load."),
                    lang.Translate("Input Required"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            try
            {
                userToModify = userService.GetByName(cbUsers.Text);
                if (userToModify == null)
                {
                    MessageBox.Show(
                        lang.Translate("User not found."),
                        lang.Translate("Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                ActivateOrDeactivateControls();
                txtUsername.Text = userToModify.Name;
                txtPassword.Text = userToModify.Password;
                txtRepeatedPassword.Text = userToModify.Password;
                cbRoleSelector.Text = userToModify.Role;
                ckbActiveUser.Checked = userToModify.IsActive;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred while loading the user: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ActivateOrDeactivateControls()
        {
            if (userToModify == null)
            {
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;
                txtRepeatedPassword.Enabled = false;
                cbRoleSelector.Enabled = false;
                ckbActiveUser.Enabled = false;
            }
            else
            {
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
                txtRepeatedPassword.Enabled = true;
                cbRoleSelector.Enabled = true;
                ckbActiveUser.Enabled = true;
            }
        }
    }
}
