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

namespace UI.secondaryForms
{
    public partial class newUserForm : Form
    {
        UserService userService = UserService.Instance();
        PermissionService permissionService = PermissionService.Instance();

        public event EventHandler UserCreated;
        public newUserForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            LoadRoleDropdown();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            try
            {
                User user = new User
                {
                    Name = txtUsername.Text,
                    Password = (txtPassword.Text == txtRepeatedPassword.Text) ? txtPassword.Text : throw new Exception("Both passwords must be identicals"),
                    IsActive = ckbActiveUser.Checked,
                    Role = cbRoleSelector.Text,
                };
                userService.Insert(user);

                MessageBox.Show($"The user '{user.Name}' was created succesfully");
                Logger.Current.Info($"The user '{user.Name}' was created succesfully");

                UserCreated?.Invoke(this, EventArgs.Empty);

                txtUsername.Text = "";
                txtPassword.Text = "";
                txtRepeatedPassword.Text = "";
            }
            catch (MySystemException ex)
            {
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRoleDropdown()
        {
            List<string> roles = permissionService.GetAllFamilies().Select(r => r.Name).ToList();
            cbRoleSelector.DataSource = roles;
        }
    }
}
