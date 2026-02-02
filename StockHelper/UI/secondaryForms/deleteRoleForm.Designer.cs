namespace UI.secondaryForms
{
    partial class deleteRoleForm
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
            clbRoles = new CheckedListBox();
            btnDelete = new Button();
            SuspendLayout();
            // 
            // clbRoles
            // 
            clbRoles.CheckOnClick = true;
            clbRoles.FormattingEnabled = true;
            clbRoles.Location = new Point(12, 12);
            clbRoles.Name = "clbRoles";
            clbRoles.Size = new Size(308, 292);
            clbRoles.TabIndex = 0;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(51, 313);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(231, 23);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // deleteRoleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(332, 347);
            Controls.Add(btnDelete);
            Controls.Add(clbRoles);
            Name = "deleteRoleForm";
            Text = "Delete Role(s)";
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox clbRoles;
        private Button btnDelete;
    }
}