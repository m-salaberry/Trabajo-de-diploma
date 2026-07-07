using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Services.Implementations;
using Services.Domain;
using Services.Contracts.CustomsException;
using Services.Contracts.Logs;
using UI.Implementations;

namespace UI.secondaryForms
{
    public partial class newUserForm : TranslatableForm
    {
        UserService userService = UserService.Instance();
        PermissionService permissionService = PermissionService.Instance();
        LanguageService lang = LanguageService.GetInstance;

        public event EventHandler UserCreated;
        
        /// <summary>
        /// Initialize the new-user form, center it and load the role dropdown.
        /// </summary>
        public newUserForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadRoleDropdown();
        }

        /// <summary>
        /// Handle the click on label1. No action is performed.
        /// </summary>
        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handle the click on label2. No action is performed.
        /// </summary>
        private void label2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Validate the form, build a new user from the input fields, persist it through the user
        /// service, raise <see cref="UserCreated"/> and clear the form.
        /// </summary>
        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (!ValidateForm())
                {
                    return;
                }
                
                User user = new User
                {
                    Name = txtUsername.Text.Trim(),
                    Password = txtPassword.Text,
                    IsActive = ckbActiveUser.Checked,
                    Role = cbRoleSelector.Text,
                };
                
                userService.Insert(user);

                MessageBox.Show(
                    string.Format(lang.Translate("The user '{0}' was created successfully"), user.Name),
                    lang.Translate("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Logger.Current.Info($"The user '{user.Name}' was created successfully");
                
                UserCreated?.Invoke(this, EventArgs.Empty);
                
                ClearForm();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred while creating the user: ") + ex.Message, 
                    lang.Translate("Error"), 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message, 
                    lang.Translate("Error"), 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Validate that the username, password (including confirmation) and role meet the required
        /// rules, showing a warning for the first failing field.
        /// </summary>
        /// <returns><c>true</c> when all inputs are valid; otherwise <c>false</c>.</returns>
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

            if (txtPassword.Text.Length < 4)
            {
                MessageBox.Show(
                    lang.Translate("Password must be at least 4 characters"), 
                    lang.Translate("Validation Error"), 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text != txtRepeatedPassword.Text)
            {
                MessageBox.Show(
                    lang.Translate("Both passwords must be identical"), 
                    lang.Translate("Validation Error"), 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                txtRepeatedPassword.Focus();
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

        /// <summary>
        /// Clear the username and password input fields.
        /// </summary>
        private void ClearForm()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtRepeatedPassword.Text = "";
        }

        /// <summary>
        /// Populate the role selector with the names of all permission families.
        /// </summary>
        private void LoadRoleDropdown()
        {
            List<string> roles = permissionService.GetAllFamilies().Select(r => r.Name).ToList();
            cbRoleSelector.DataSource = roles;
        }

        /// <summary>
        /// Apply the current language translations to the form labels and buttons.
        /// </summary>
        public override void ApplyTranslations()
        {
            lbUsername.Text = lang.Translate("Username:");
            lblPassword.Text = lang.Translate("Password:");
            lblRePassword.Text = lang.Translate("Repeat Password:");
            lblRole.Text = lang.Translate("Select role:");
            ckbActiveUser.Text = lang.Translate("Active User");
            btnSaveUser.Text = lang.Translate("Save");
        }
    }
}
