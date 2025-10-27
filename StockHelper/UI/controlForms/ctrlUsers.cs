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
            loadActiveUsers();
        }

        private void loadActiveUsers()
        {
            List<User> _users = UserService.Instance().GetAllActive();
            foreach (User user in _users)
            {
                dgvActiveUsers.Rows.Add(user.Name, user.Password, user.Role);
            }
            dgvActiveUsers.Refresh();
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
                newUserForm.ShowDialog();
                newUserForm.BringToFront();

            }
            catch (Exception ex)
            {
                throw new MySystemException(ex.Message, "");
            }
        }
    }
}
