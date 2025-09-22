using BLL.Implementations;
using Microsoft.Identity.Client;
using Services.Domain;

namespace UI
{
    public partial class frmLogIn : Form
    {
        internal LoginService _loginService = new LoginService();
        internal UserService _userService = new UserService();
        public frmLogIn()
        {
            InitializeComponent();
            
        }

        private void checkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkShowPassword.Checked)
            {
                showPassword();
            }
            else
            {
                hidePassword();
            }
        }

        private void showPassword()
        {
            passwordtxt.PasswordChar = '\0';
        }

        private void hidePassword()
        {
            passwordtxt.PasswordChar = '*';
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            try
            {
                User loginUser = new User
                {
                    Name = usernametxt.Text,
                    Password = passwordtxt.Text
                };

                if(_loginService.Authenticate(loginUser.Name, loginUser.Password))
                {
                    User currentUser = _userService.GetByName(loginUser.Name);
                    this.Hide();
                    frmMain mainForm = new frmMain(currentUser);
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    throw new Exception("An error has ocurred during the login process.\n"+"If the problem continues contact with the support team");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
