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
        /// <summary>
        /// Initializes a new instance of the login form and its designer components.
        /// </summary>
        public frmLogIn()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Applies the current language translations to all controls on the form.
        /// </summary>
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

        /// <summary>
        /// Shows or hides the password text depending on the checkbox state.
        /// </summary>
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

        /// <summary>
        /// Makes the password text visible by clearing the password mask character.
        /// </summary>
        private void showPassword()
        {
            passwordtxt.PasswordChar = '\0';
        }

        /// <summary>
        /// Masks the password text using the '*' password character.
        /// </summary>
        private void hidePassword()
        {
            passwordtxt.PasswordChar = '*';
        }

        /// <summary>
        /// Validates the entered credentials, authenticates the user and opens the main window,
        /// handling logout and application-exit flows as well as any login errors.
        /// </summary>
        private void loginbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateCredentials(out string validationMessage))
                {
                    MessageBox.Show(validationMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                User loginUser = new User
                {
                    Name = usernametxt.Text.Trim(),
                    Password = passwordtxt.Text
                };

                if(_loginService.Authenticate(loginUser.Name, loginUser.Password))
                {
                    User currentUser = _userService.GetByName(loginUser.Name);
                    Logger.Current.Info($"User {currentUser.Name} has logged in successfully.");

                    BLL.Implementations.BackupService.Instance().RunDailyBackupIfNeeded();

                    this.Hide();
                    frmMain mainForm = frmMain.GetInstance(currentUser);
                    mainForm.ShowDialog();

                    if (mainForm.LoggedOut)
                    {
                        mainForm.Dispose();
                        usernametxt.Clear();
                        passwordtxt.Clear();
                        checkShowPassword.Checked = false;
                        this.Show();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    throw new Exception("An error has ocurred during the login process.\n"+"If the problem continues contact with the support team");
                }

            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Error, "Login error", ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Validates username and password before authentication.
        /// </summary>
        /// <param name="validationMessage">Message describing validation failure</param>
        /// <returns>True if validation passes, false otherwise</returns>
        private bool ValidateCredentials(out string validationMessage)
        {
            validationMessage = string.Empty;
            var lang = LanguageService.GetInstance;

            if (string.IsNullOrWhiteSpace(usernametxt.Text))
            {
                validationMessage = lang.Translate("Username cannot be empty");
                usernametxt.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(passwordtxt.Text))
            {
                validationMessage = lang.Translate("Password cannot be empty");
                passwordtxt.Focus();
                return false;
            }

            if (usernametxt.Text.Trim().Length < 3)
            {
                validationMessage = lang.Translate("Username must be at least 3 characters long");
                usernametxt.Focus();
                return false;
            }

            if (usernametxt.Text.Trim().Length > 50)
            {
                validationMessage = lang.Translate("Username cannot exceed 50 characters");
                usernametxt.Focus();
                return false;
            }

            if (passwordtxt.Text.Length < 4)
            {
                validationMessage = lang.Translate("Password must be at least 4 characters long");
                passwordtxt.Focus();
                return false;
            }

            if (passwordtxt.Text.Length > 100)
            {
                validationMessage = lang.Translate("Password cannot exceed 100 characters");
                passwordtxt.Focus();
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(usernametxt.Text.Trim(), @"^[a-zA-Z0-9._-]+$"))
            {
                validationMessage = lang.Translate("Username contains invalid characters. Only letters, numbers, dots, underscores and hyphens are allowed");
                usernametxt.Focus();
                return false;
            }

            usernametxt.Text = usernametxt.Text.Trim();

            Logger.Current.Debug($"Credentials validation passed for user: {usernametxt.Text}");
            return true;
        }
    }
}
