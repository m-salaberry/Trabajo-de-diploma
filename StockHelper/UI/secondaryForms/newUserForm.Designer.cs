namespace UI.secondaryForms
{
    partial class newUserForm
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
            txtUsername = new TextBox();
            lbUsername = new Label();
            label1 = new Label();
            txtPassword = new TextBox();
            label2 = new Label();
            cbRoleSelector = new ComboBox();
            ckbActiveUser = new CheckBox();
            label3 = new Label();
            txtRepeatedPassword = new TextBox();
            btnSaveUser = new Button();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(200, 40);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(257, 23);
            txtUsername.TabIndex = 0;
            // 
            // lbUsername
            // 
            lbUsername.AutoSize = true;
            lbUsername.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lbUsername.Location = new Point(79, 31);
            lbUsername.Name = "lbUsername";
            lbUsername.Size = new Size(115, 30);
            lbUsername.TabIndex = 1;
            lbUsername.Text = "Username:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(85, 82);
            label1.Name = "label1";
            label1.Size = new Size(109, 30);
            label1.TabIndex = 2;
            label1.Text = "Password:";
            label1.Click += label1_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(200, 91);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(257, 23);
            txtPassword.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(78, 184);
            label2.Name = "label2";
            label2.Size = new Size(116, 30);
            label2.TabIndex = 4;
            label2.Text = "Select role:";
            label2.Click += label2_Click;
            // 
            // cbRoleSelector
            // 
            cbRoleSelector.FormattingEnabled = true;
            cbRoleSelector.Location = new Point(200, 193);
            cbRoleSelector.Name = "cbRoleSelector";
            cbRoleSelector.Size = new Size(257, 23);
            cbRoleSelector.TabIndex = 5;
            // 
            // ckbActiveUser
            // 
            ckbActiveUser.AutoSize = true;
            ckbActiveUser.Checked = true;
            ckbActiveUser.CheckState = CheckState.Checked;
            ckbActiveUser.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ckbActiveUser.Location = new Point(200, 232);
            ckbActiveUser.Name = "ckbActiveUser";
            ckbActiveUser.Size = new Size(112, 25);
            ckbActiveUser.TabIndex = 6;
            ckbActiveUser.Text = "Active User";
            ckbActiveUser.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.Location = new Point(18, 133);
            label3.Name = "label3";
            label3.Size = new Size(181, 30);
            label3.TabIndex = 7;
            label3.Text = "Repeat Password:";
            // 
            // txtRepeatedPassword
            // 
            txtRepeatedPassword.Location = new Point(197, 142);
            txtRepeatedPassword.Name = "txtRepeatedPassword";
            txtRepeatedPassword.Size = new Size(257, 23);
            txtRepeatedPassword.TabIndex = 8;
            // 
            // btnSaveUser
            // 
            btnSaveUser.Location = new Point(177, 272);
            btnSaveUser.Name = "btnSaveUser";
            btnSaveUser.Size = new Size(117, 46);
            btnSaveUser.TabIndex = 9;
            btnSaveUser.Text = "Save";
            btnSaveUser.UseVisualStyleBackColor = true;
            btnSaveUser.Click += btnSaveUser_Click;
            // 
            // newUserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 342);
            Controls.Add(btnSaveUser);
            Controls.Add(txtRepeatedPassword);
            Controls.Add(label3);
            Controls.Add(ckbActiveUser);
            Controls.Add(cbRoleSelector);
            Controls.Add(label2);
            Controls.Add(txtPassword);
            Controls.Add(label1);
            Controls.Add(lbUsername);
            Controls.Add(txtUsername);
            Name = "newUserForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUsername;
        private Label lbUsername;
        private Label label1;
        private TextBox txtPassword;
        private Label label2;
        private ComboBox cbRoleSelector;
        private CheckBox ckbActiveUser;
        private Label label3;
        private TextBox txtRepeatedPassword;
        private Button btnSaveUser;
    }
}