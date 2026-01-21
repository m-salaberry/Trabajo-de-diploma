namespace UI.secondaryForms
{
    partial class newRoleForm
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
            clbPermissions = new CheckedListBox();
            btnSaveNew = new Button();
            txtRoleName = new TextBox();
            SuspendLayout();
            // 
            // clbPermissions
            // 
            clbPermissions.CheckOnClick = true;
            clbPermissions.FormattingEnabled = true;
            clbPermissions.Location = new Point(12, 60);
            clbPermissions.Name = "clbPermissions";
            clbPermissions.Size = new Size(308, 256);
            clbPermissions.TabIndex = 0;
            // 
            // btnSaveNew
            // 
            btnSaveNew.Location = new Point(119, 322);
            btnSaveNew.Name = "btnSaveNew";
            btnSaveNew.Size = new Size(94, 48);
            btnSaveNew.TabIndex = 1;
            btnSaveNew.Text = "Save";
            btnSaveNew.UseVisualStyleBackColor = true;
            btnSaveNew.Click += btnSaveNew_Click;
            // 
            // txtRoleName
            // 
            txtRoleName.Location = new Point(51, 31);
            txtRoleName.Name = "txtRoleName";
            txtRoleName.PlaceholderText = "Role name";
            txtRoleName.Size = new Size(231, 23);
            txtRoleName.TabIndex = 2;
            // 
            // newRoleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(332, 382);
            Controls.Add(txtRoleName);
            Controls.Add(btnSaveNew);
            Controls.Add(clbPermissions);
            Name = "newRoleForm";
            Text = "newRoleForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox clbPermissions;
        private Button btnSaveNew;
        private TextBox txtRoleName;
    }
}