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
    public partial class ctrlUsers : UserControl
    {
        public ctrlUsers()
        {
            InitializeComponent();
            LoadUsers();
        }

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
                }
                if (!user.IsActive)
                {
                    dgvDisabledUsers.Rows.Add(user.Name, user.Password, user.Role);
                }
            }
            dgvActiveUsers.Refresh();
            dgvDisabledUsers.Refresh();
        }

        public void RefreshUserList()
        {
            LoadUsers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            frmMain.GetInstance().ResetMainPanelSize();
            this.Dispose();
        }

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
            catch (Exception ex)
            {
                throw new MySystemException(ex.Message, "");
            }
        }

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
            catch (Exception ex)
            {
                throw new MySystemException(ex.Message, "");
            }
        }
    }
}
