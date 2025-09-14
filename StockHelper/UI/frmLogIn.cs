namespace UI
{
    public partial class frmLogIn : Form
    {
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

        }
    }
}
