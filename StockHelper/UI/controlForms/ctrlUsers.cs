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
    public partial class ctrlUsers : TranslatableUserControls
    {
        LanguageService lang = LanguageService.GetInstance;

        /// <summary>
        /// Initializes the control and loads the active and disabled users into their grids.
        /// </summary>
        public ctrlUsers()
        {
            InitializeComponent();
            LoadUsers();
        }

        /// <summary>
        /// Loads all users from the service, placing active users and disabled users into their respective grids.
        /// </summary>
        private void LoadUsers()
        {
            dgvActiveUsers.Rows.Clear();
            dgvDisabledUsers.Rows.Clear();

            List<User> _users = UserService.Instance().GetAll();
            foreach (User user in _users)
            {
                if (user.IsActive)
                {
                    dgvActiveUsers.Rows.Add(user.Name, user.Password, user.Role);
                    dgvActiveUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                if (!user.IsActive)
                {
                    dgvDisabledUsers.Rows.Add(user.Name, user.Password, user.Role);
                    dgvDisabledUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            dgvActiveUsers.Refresh();
            dgvDisabledUsers.Refresh();
        }

        /// <summary>
        /// Reloads the active and disabled user grids.
        /// </summary>
        public void RefreshUserList()
        {
            LoadUsers();
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
        /// Opens the new user dialog and refreshes the user list when a user is created.
        /// </summary>
        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            try
            {
                newUserForm newUserForm = new newUserForm();
                // Subscribe to the UserCreated event
                newUserForm.UserCreated += (s, ev) => RefreshUserList();
                newUserForm.ShowDialog();
                newUserForm.BringToFront();
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
        /// Opens the modify user dialog and refreshes the user list when a user is updated.
        /// </summary>
        private void btnModUser_Click(object sender, EventArgs e)
        {
            try
            {
                modUserForm modUserForm = new modUserForm();
                // Subscribe to the UserCreated event
                modUserForm.UserUpdated += (s, ev) => RefreshUserList();
                modUserForm.ShowDialog();
                modUserForm.BringToFront();
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
        /// Handles the control's Load event; currently performs no additional initialization.
        /// </summary>
        private void ctrlUsers_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Applies the current language translations to the buttons, labels and the active and disabled user grid columns.
        /// </summary>
        public override void ApplyTranslations()
        {
            // Translate buttons
            btnAddNewUser.Text = lang.Translate("New User");
            btnModUser.Text = lang.Translate("Modify User");

            // Translate labels
            lbActiveUsers.Text = lang.Translate("Active Users");
            lbDisablesUser.Text = lang.Translate("Disabled Users");

            // Translate DataGridView columns for Active Users
            if (dgvActiveUsers.Columns["UserName"] != null)
                dgvActiveUsers.Columns["UserName"].HeaderText = lang.Translate("Name");
            if (dgvActiveUsers.Columns["Password"] != null)
                dgvActiveUsers.Columns["Password"].HeaderText = lang.Translate("Password");
            if (dgvActiveUsers.Columns["Role"] != null)
                dgvActiveUsers.Columns["Role"].HeaderText = lang.Translate("Role");

            // Translate DataGridView columns for Disabled Users
            if (dgvDisabledUsers.Columns["DisableUserName"] != null)
                dgvDisabledUsers.Columns["DisableUserName"].HeaderText = lang.Translate("Name");
            if (dgvDisabledUsers.Columns["DisableUserPassword"] != null)
                dgvDisabledUsers.Columns["DisableUserPassword"].HeaderText = lang.Translate("Password");
            if (dgvDisabledUsers.Columns["DisableUserRole"] != null)
                dgvDisabledUsers.Columns["DisableUserRole"].HeaderText = lang.Translate("Role");

            // Refresh grids to apply translations to column headers
            dgvActiveUsers.Refresh();
            dgvDisabledUsers.Refresh();
        }
    }
}
