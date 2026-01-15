namespace UI.secondaryForms
{
    partial class modUserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSaveUser = new Button();
            txtRepeatedPassword = new TextBox();
            lblRePassword = new Label();
            ckbActiveUser = new CheckBox();
            cbRoleSelector = new ComboBox();
            lblRole = new Label();
            txtPassword = new TextBox();
            lblPassword = new Label();
            lbUsername = new Label();
            txtUsername = new TextBox();
            cbUsers = new ComboBox();
            lblSelectUser = new Label();
            btnLoadUser = new Button();
            SuspendLayout();
            // 
            // btnSaveUser
            // 
            btnSaveUser.Location = new Point(179, 313);
            btnSaveUser.Name = "btnSaveUser";
            btnSaveUser.Size = new Size(117, 46);
            btnSaveUser.TabIndex = 21;
            btnSaveUser.Text = "Save";
            btnSaveUser.UseVisualStyleBackColor = true;
            btnSaveUser.Click += btnSaveUser_Click;
            // 
            // txtRepeatedPassword
            // 
            txtRepeatedPassword.Location = new Point(199, 183);
            txtRepeatedPassword.Name = "txtRepeatedPassword";
            txtRepeatedPassword.Size = new Size(257, 23);
            txtRepeatedPassword.TabIndex = 17;
            // 
            // lblRePassword
            // 
            lblRePassword.AutoSize = true;
            lblRePassword.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblRePassword.Location = new Point(20, 174);
            lblRePassword.Name = "lblRePassword";
            lblRePassword.Size = new Size(181, 30);
            lblRePassword.TabIndex = 18;
            lblRePassword.Text = "Repeat Password:";
            // 
            // ckbActiveUser
            // 
            ckbActiveUser.AutoSize = true;
            ckbActiveUser.Checked = true;
            ckbActiveUser.CheckState = CheckState.Checked;
            ckbActiveUser.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbActiveUser.Location = new Point(202, 273);
            ckbActiveUser.Name = "ckbActiveUser";
            ckbActiveUser.Size = new Size(112, 25);
            ckbActiveUser.TabIndex = 20;
            ckbActiveUser.Text = "Active User";
            ckbActiveUser.UseVisualStyleBackColor = true;
            // 
            // cbRoleSelector
            // 
            cbRoleSelector.FormattingEnabled = true;
            cbRoleSelector.Location = new Point(202, 234);
            cbRoleSelector.Name = "cbRoleSelector";
            cbRoleSelector.Size = new Size(257, 23);
            cbRoleSelector.TabIndex = 19;
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblRole.Location = new Point(80, 225);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(116, 30);
            lblRole.TabIndex = 16;
            lblRole.Text = "Select role:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(202, 132);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(257, 23);
            txtPassword.TabIndex = 15;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblPassword.Location = new Point(87, 123);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(109, 30);
            lblPassword.TabIndex = 14;
            lblPassword.Text = "Password:";
            // 
            // lbUsername
            // 
            lbUsername.AutoSize = true;
            lbUsername.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lbUsername.Location = new Point(81, 72);
            lbUsername.Name = "lbUsername";
            lbUsername.Size = new Size(115, 30);
            lbUsername.TabIndex = 13;
            lbUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(202, 81);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(257, 23);
            txtUsername.TabIndex = 12;
            // 
            // cbUsers
            // 
            cbUsers.FormattingEnabled = true;
            cbUsers.Location = new Point(202, 23);
            cbUsers.Name = "cbUsers";
            cbUsers.Size = new Size(163, 23);
            cbUsers.TabIndex = 22;
            // 
            // lblSelectUser
            // 
            lblSelectUser.AutoSize = true;
            lblSelectUser.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSelectUser.Location = new Point(87, 21);
            lblSelectUser.Name = "lblSelectUser";
            lblSelectUser.Size = new Size(110, 25);
            lblSelectUser.TabIndex = 23;
            lblSelectUser.Text = "Select user:";
            // 
            // btnLoadUser
            // 
            btnLoadUser.Location = new Point(371, 23);
            btnLoadUser.Name = "btnLoadUser";
            btnLoadUser.Size = new Size(75, 23);
            btnLoadUser.TabIndex = 24;
            btnLoadUser.Text = "Load";
            btnLoadUser.UseVisualStyleBackColor = true;
            btnLoadUser.Click += btnLoadUser_Click;
            // 
            // modUserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 375);
            Controls.Add(btnLoadUser);
            Controls.Add(lblSelectUser);
            Controls.Add(cbUsers);
            Controls.Add(btnSaveUser);
            Controls.Add(txtRepeatedPassword);
            Controls.Add(lblRePassword);
            Controls.Add(ckbActiveUser);
            Controls.Add(cbRoleSelector);
            Controls.Add(lblRole);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(lbUsername);
            Controls.Add(txtUsername);
            Name = "modUserForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSaveUser;
        private TextBox txtRepeatedPassword;
        private Label lblRePassword;
        private CheckBox ckbActiveUser;
        private ComboBox cbRoleSelector;
        private Label lblRole;
        private TextBox txtPassword;
        private Label lblPassword;
        private Label lbUsername;
        private TextBox txtUsername;
        private ComboBox cbUsers;
        private Label lblSelectUser;
        private Button btnLoadUser;
    }
}