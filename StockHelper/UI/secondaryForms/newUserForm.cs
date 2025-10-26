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

namespace UI.secondaryForms
{
    public partial class newUserForm : Form
    {
        UserService userService = UserService.Instance();
        public newUserForm()
        {
            InitializeComponent();
            this.CenterToScreen();
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
                    Password = (txtPassword.Text == txtRepeatedPassword.Text) ? txtPassword.Text : throw new Exception(""),
                };
                userService.Insert(user);
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
    }
}
