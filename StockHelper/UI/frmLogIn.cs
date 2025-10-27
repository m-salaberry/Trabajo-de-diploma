using Services.Implementations;
using Microsoft.Identity.Client;
using Services.Domain;
using Services.Contracts.Logs;
using UI.Implementations;

namespace UI
{
    public partial class frmLogIn : TranslatableForm
    {
        internal LoginService _loginService = new LoginService();
        internal UserService _userService = UserService.Instance();
        public frmLogIn()
        {
            InitializeComponent();
            
        }

        public override void ApplyTranslations()
        {
            var lang = LanguageService.GetInstance;
            this.Text = lang.Translate("Log In");
            gBoxLogin.Text = lang.Translate("Enter your username and password to continue");
            usernametxt.PlaceholderText = lang.Translate("Username");
            passwordtxt.PlaceholderText = lang.Translate("Password");
            checkShowPassword.Text = lang.Translate("Show");
            loginbtn.Text = lang.Translate("Log In");
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
                    Logger.Current.Info($"User {currentUser.Name} has logged in successfully.");
                    this.Hide();
                    frmMain mainForm = frmMain.GetInstance(currentUser);
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
