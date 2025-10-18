namespace UI.secondaryForms
{
    partial class ctrlUsers
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
            btnClose = new Button();
            btnAddNewUser = new Button();
            dataGridView1 = new DataGridView();
            btnModUser = new Button();
            dataGridView2 = new DataGridView();
            lbDisablesUser = new Label();
            lbActiveUsers = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Location = new Point(518, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(37, 24);
            btnClose.TabIndex = 0;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnAddNewUser
            // 
            btnAddNewUser.Location = new Point(89, 379);
            btnAddNewUser.Name = "btnAddNewUser";
            btnAddNewUser.Size = new Size(113, 40);
            btnAddNewUser.TabIndex = 1;
            btnAddNewUser.Text = "Add User";
            btnAddNewUser.UseVisualStyleBackColor = true;
            btnAddNewUser.Click += btnAddNewUser_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(15, 51);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(261, 314);
            dataGridView1.TabIndex = 5;
            // 
            // btnModUser
            // 
            btnModUser.Location = new Point(356, 379);
            btnModUser.Name = "btnModUser";
            btnModUser.Size = new Size(113, 40);
            btnModUser.TabIndex = 7;
            btnModUser.Text = "Modify User";
            btnModUser.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(282, 51);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(261, 314);
            dataGridView2.TabIndex = 8;
            // 
            // lbDisablesUser
            // 
            lbDisablesUser.AutoSize = true;
            lbDisablesUser.Font = new Font("Segoe UI Semibold", 13.75F, FontStyle.Bold);
            lbDisablesUser.Location = new Point(285, 22);
            lbDisablesUser.Name = "lbDisablesUser";
            lbDisablesUser.Size = new Size(142, 25);
            lbDisablesUser.TabIndex = 10;
            lbDisablesUser.Text = "Disabled Users:";
            // 
            // lbActiveUsers
            // 
            lbActiveUsers.AutoSize = true;
            lbActiveUsers.Font = new Font("Segoe UI Semibold", 13.75F, FontStyle.Bold);
            lbActiveUsers.Location = new Point(18, 23);
            lbActiveUsers.Name = "lbActiveUsers";
            lbActiveUsers.Size = new Size(122, 25);
            lbActiveUsers.TabIndex = 11;
            lbActiveUsers.Text = "Active Users:";
            // 
            // ctrlUsers
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(lbActiveUsers);
            Controls.Add(lbDisablesUser);
            Controls.Add(dataGridView2);
            Controls.Add(btnModUser);
            Controls.Add(dataGridView1);
            Controls.Add(btnAddNewUser);
            Controls.Add(btnClose);
            Name = "ctrlUsers";
            Size = new Size(558, 436);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnClose;
        private Button btnAddNewUser;
        private DataGridView dataGridView1;
        private Button btnModUser;
        private DataGridView dataGridView2;
        private Label lbDisablesUser;
        private Label lbActiveUsers;
    }
}
