namespace UI.secondaryForms
{
    partial class modRoleForm
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
            btnSaveNew = new Button();
            clbPermissions = new CheckedListBox();
            cbRoles = new ComboBox();
            SuspendLayout();
            // 
            // btnSaveNew
            // 
            btnSaveNew.Location = new Point(119, 322);
            btnSaveNew.Name = "btnSaveNew";
            btnSaveNew.Size = new Size(94, 48);
            btnSaveNew.TabIndex = 3;
            btnSaveNew.Text = "Save";
            btnSaveNew.UseVisualStyleBackColor = true;
            btnSaveNew.Click += btnSaveNew_Click;
            // 
            // clbPermissions
            // 
            clbPermissions.CheckOnClick = true;
            clbPermissions.FormattingEnabled = true;
            clbPermissions.Location = new Point(12, 60);
            clbPermissions.Name = "clbPermissions";
            clbPermissions.Size = new Size(308, 256);
            clbPermissions.TabIndex = 2;
            // 
            // cbRoles
            // 
            cbRoles.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRoles.ImeMode = ImeMode.NoControl;
            cbRoles.Location = new Point(51, 31);
            cbRoles.Name = "cbRoles";
            cbRoles.Size = new Size(231, 23);
            cbRoles.TabIndex = 4;
            cbRoles.SelectedIndexChanged += cbRoles_SelectedIndexChanged;
            // 
            // modRoleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(332, 382);
            Controls.Add(cbRoles);
            Controls.Add(btnSaveNew);
            Controls.Add(clbPermissions);
            Name = "modRoleForm";
            Text = "Modify Role";
            ResumeLayout(false);
        }

        #endregion

        private Button btnSaveNew;
        private CheckedListBox clbPermissions;
        private ComboBox cbRoles;
    }
}