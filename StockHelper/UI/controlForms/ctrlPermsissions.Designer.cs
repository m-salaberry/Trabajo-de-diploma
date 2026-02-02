namespace UI.secondaryForms
{
    partial class ctrlPermsissions
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            dgvRoles = new DataGridView();
            btnNewRole = new Button();
            btnModifyRole = new Button();
            btnDeleteRole = new Button();
            btnClose = new Button();
            lbRoles = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
            SuspendLayout();
            // 
            // dgvRoles
            // 
            dgvRoles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRoles.Location = new Point(31, 39);
            dgvRoles.MultiSelect = false;
            dgvRoles.Name = "dgvRoles";
            dgvRoles.ReadOnly = true;
            dgvRoles.RowHeadersVisible = false;
            dgvRoles.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvRoles.ShowCellToolTips = false;
            dgvRoles.Size = new Size(375, 265);
            dgvRoles.TabIndex = 1;
            // 
            // btnNewRole
            // 
            btnNewRole.Location = new Point(31, 310);
            btnNewRole.Name = "btnNewRole";
            btnNewRole.Size = new Size(120, 26);
            btnNewRole.TabIndex = 2;
            btnNewRole.Text = "New Role";
            btnNewRole.UseVisualStyleBackColor = true;
            btnNewRole.Click += btnNewRole_Click;
            // 
            // btnModifyRole
            // 
            btnModifyRole.Location = new Point(158, 310);
            btnModifyRole.Name = "btnModifyRole";
            btnModifyRole.Size = new Size(120, 26);
            btnModifyRole.TabIndex = 3;
            btnModifyRole.Text = "Modify";
            btnModifyRole.UseVisualStyleBackColor = true;
            btnModifyRole.Click += btnModifyRole_Click;
            // 
            // btnDeleteRole
            // 
            btnDeleteRole.Location = new Point(285, 310);
            btnDeleteRole.Name = "btnDeleteRole";
            btnDeleteRole.Size = new Size(120, 26);
            btnDeleteRole.TabIndex = 4;
            btnDeleteRole.Text = "Delete";
            btnDeleteRole.UseVisualStyleBackColor = true;
            btnDeleteRole.Click += btnDeleteRole_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(399, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(37, 24);
            btnClose.TabIndex = 5;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // lbRoles
            // 
            lbRoles.AutoSize = true;
            lbRoles.Font = new Font("Segoe UI Semibold", 13.75F, FontStyle.Bold);
            lbRoles.Location = new Point(31, 11);
            lbRoles.Name = "lbRoles";
            lbRoles.Size = new Size(62, 25);
            lbRoles.TabIndex = 12;
            lbRoles.Text = "Roles:";
            // 
            // ctrlPermsissions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            Controls.Add(lbRoles);
            Controls.Add(btnClose);
            Controls.Add(btnDeleteRole);
            Controls.Add(btnModifyRole);
            Controls.Add(btnNewRole);
            Controls.Add(dgvRoles);
            Name = "ctrlPermsissions";
            Size = new Size(439, 348);
            ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox checkedListBox1;
        private DataGridView dgvRoles;
        private Button btnNewRole;
        private Button btnModifyRole;
        private Button btnDeleteRole;
        private Button btnClose;
        private Label lbRoles;
    }
}
