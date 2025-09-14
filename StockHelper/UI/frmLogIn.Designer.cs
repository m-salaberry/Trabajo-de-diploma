namespace UI
{
    partial class frmLogIn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            usernametxt = new TextBox();
            gBoxLogin = new GroupBox();
            checkShowPassword = new CheckBox();
            loginbtn = new Button();
            passwordtxt = new TextBox();
            appTitle = new Label();
            gBoxLogin.SuspendLayout();
            SuspendLayout();
            // 
            // usernametxt
            // 
            usernametxt.Location = new Point(89, 22);
            usernametxt.Name = "usernametxt";
            usernametxt.PlaceholderText = "Username";
            usernametxt.Size = new Size(178, 23);
            usernametxt.TabIndex = 0;
            // 
            // gBoxLogin
            // 
            gBoxLogin.BackColor = SystemColors.Menu;
            gBoxLogin.BackgroundImageLayout = ImageLayout.None;
            gBoxLogin.Controls.Add(checkShowPassword);
            gBoxLogin.Controls.Add(loginbtn);
            gBoxLogin.Controls.Add(passwordtxt);
            gBoxLogin.Controls.Add(usernametxt);
            gBoxLogin.FlatStyle = FlatStyle.Popup;
            gBoxLogin.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gBoxLogin.Location = new Point(12, 154);
            gBoxLogin.Name = "gBoxLogin";
            gBoxLogin.Size = new Size(356, 183);
            gBoxLogin.TabIndex = 1;
            gBoxLogin.TabStop = false;
            gBoxLogin.Text = "Enter you username and password to continue:";
            // 
            // checkShowPassword
            // 
            checkShowPassword.AutoSize = true;
            checkShowPassword.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkShowPassword.Location = new Point(273, 73);
            checkShowPassword.Name = "checkShowPassword";
            checkShowPassword.Size = new Size(53, 17);
            checkShowPassword.TabIndex = 3;
            checkShowPassword.Text = "Show";
            checkShowPassword.UseVisualStyleBackColor = true;
            checkShowPassword.CheckedChanged += checkShowPassword_CheckedChanged;
            // 
            // loginbtn
            // 
            loginbtn.Location = new Point(89, 110);
            loginbtn.Name = "loginbtn";
            loginbtn.Size = new Size(178, 24);
            loginbtn.TabIndex = 2;
            loginbtn.Text = "Log In";
            loginbtn.UseVisualStyleBackColor = true;
            loginbtn.Click += loginbtn_Click;
            // 
            // passwordtxt
            // 
            passwordtxt.Location = new Point(89, 67);
            passwordtxt.Name = "passwordtxt";
            passwordtxt.PasswordChar = '*';
            passwordtxt.PlaceholderText = "Password";
            passwordtxt.Size = new Size(178, 23);
            passwordtxt.TabIndex = 1;
            // 
            // appTitle
            // 
            appTitle.AutoSize = true;
            appTitle.Font = new Font("Unispace", 27.7499962F, FontStyle.Bold, GraphicsUnit.Point, 0);
            appTitle.Location = new Point(62, 50);
            appTitle.Name = "appTitle";
            appTitle.Size = new Size(272, 44);
            appTitle.TabIndex = 2;
            appTitle.Text = "StockHelper";
            // 
            // frmLogIn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(380, 347);
            Controls.Add(appTitle);
            Controls.Add(gBoxLogin);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "frmLogIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Log In";
            gBoxLogin.ResumeLayout(false);
            gBoxLogin.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox usernametxt;
        private GroupBox gBoxLogin;
        private TextBox passwordtxt;
        private Button loginbtn;
        private Label appTitle;
        private CheckBox checkShowPassword;
    }
}
