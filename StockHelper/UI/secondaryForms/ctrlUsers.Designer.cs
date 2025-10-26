﻿namespace UI.secondaryForms
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
            dgvActiveUsers = new DataGridView();
            btnModUser = new Button();
            dgvDisabledUsers = new DataGridView();
            lbDisablesUser = new Label();
            lbActiveUsers = new Label();
            btnDisable = new Button();
            Name = new DataGridViewTextBoxColumn();
            Password = new DataGridViewTextBoxColumn();
            Role = new DataGridViewTextBoxColumn();
            DisableUserName = new DataGridViewTextBoxColumn();
            DisableUserPassword = new DataGridViewTextBoxColumn();
            DisableUserRole = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvActiveUsers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDisabledUsers).BeginInit();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Location = new Point(683, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(37, 24);
            btnClose.TabIndex = 0;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnAddNewUser
            // 
            btnAddNewUser.Location = new Point(114, 379);
            btnAddNewUser.Name = "btnAddNewUser";
            btnAddNewUser.Size = new Size(113, 40);
            btnAddNewUser.TabIndex = 1;
            btnAddNewUser.Text = "Add User";
            btnAddNewUser.UseVisualStyleBackColor = true;
            btnAddNewUser.Click += btnAddNewUser_Click;
            // 
            // dgvActiveUsers
            // 
            dgvActiveUsers.AllowUserToAddRows = false;
            dgvActiveUsers.AllowUserToDeleteRows = false;
            dgvActiveUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvActiveUsers.Columns.AddRange(new DataGridViewColumn[] { Name, Password, Role });
            dgvActiveUsers.Location = new Point(15, 51);
            dgvActiveUsers.Name = "dgvActiveUsers";
            dgvActiveUsers.ReadOnly = true;
            dgvActiveUsers.Size = new Size(343, 314);
            dgvActiveUsers.TabIndex = 5;
            // 
            // btnModUser
            // 
            btnModUser.Location = new Point(496, 379);
            btnModUser.Name = "btnModUser";
            btnModUser.Size = new Size(113, 40);
            btnModUser.TabIndex = 7;
            btnModUser.Text = "Modify User";
            btnModUser.UseVisualStyleBackColor = true;
            // 
            // dgvDisabledUsers
            // 
            dgvDisabledUsers.AllowUserToAddRows = false;
            dgvDisabledUsers.AllowUserToDeleteRows = false;
            dgvDisabledUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDisabledUsers.Columns.AddRange(new DataGridViewColumn[] { DisableUserName, DisableUserPassword, DisableUserRole });
            dgvDisabledUsers.Location = new Point(364, 50);
            dgvDisabledUsers.Name = "dgvDisabledUsers";
            dgvDisabledUsers.ReadOnly = true;
            dgvDisabledUsers.Size = new Size(343, 314);
            dgvDisabledUsers.TabIndex = 8;
            // 
            // lbDisablesUser
            // 
            lbDisablesUser.AutoSize = true;
            lbDisablesUser.Font = new Font("Segoe UI Semibold", 13.75F, FontStyle.Bold);
            lbDisablesUser.Location = new Point(367, 22);
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
            // btnDisable
            // 
            btnDisable.Location = new Point(305, 379);
            btnDisable.Name = "btnDisable";
            btnDisable.Size = new Size(113, 40);
            btnDisable.TabIndex = 12;
            btnDisable.Text = "Disable User";
            btnDisable.UseVisualStyleBackColor = true;
            // 
            // Name
            // 
            Name.HeaderText = "Name";
            Name.Name = "Name";
            Name.ReadOnly = true;
            // 
            // Password
            // 
            Password.HeaderText = "Password";
            Password.Name = "Password";
            Password.ReadOnly = true;
            // 
            // Role
            // 
            Role.HeaderText = "Role";
            Role.Name = "Role";
            Role.ReadOnly = true;
            // 
            // DisableUserName
            // 
            DisableUserName.HeaderText = "Name";
            DisableUserName.Name = "DisableUserName";
            DisableUserName.ReadOnly = true;
            // 
            // DisableUserPassword
            // 
            DisableUserPassword.HeaderText = "Password";
            DisableUserPassword.Name = "DisableUserPassword";
            DisableUserPassword.ReadOnly = true;
            // 
            // DisableUserRole
            // 
            DisableUserRole.HeaderText = "Role";
            DisableUserRole.Name = "DisableUserRole";
            DisableUserRole.ReadOnly = true;
            // 
            // ctrlUsers
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(btnDisable);
            Controls.Add(lbActiveUsers);
            Controls.Add(lbDisablesUser);
            Controls.Add(dgvDisabledUsers);
            Controls.Add(btnModUser);
            Controls.Add(dgvActiveUsers);
            Controls.Add(btnAddNewUser);
            Controls.Add(btnClose);
            Size = new Size(723, 436);
            ((System.ComponentModel.ISupportInitialize)dgvActiveUsers).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDisabledUsers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnClose;
        private Button btnAddNewUser;
        private DataGridView dgvActiveUsers;
        private Button btnModUser;
        private DataGridView dgvDisabledUsers;
        private Label lbDisablesUser;
        private Label lbActiveUsers;
        private Button btnDisable;
        private DataGridViewTextBoxColumn Name;
        private DataGridViewTextBoxColumn Password;
        private DataGridViewTextBoxColumn Role;
        private DataGridViewTextBoxColumn DisableUserName;
        private DataGridViewTextBoxColumn DisableUserPassword;
        private DataGridViewTextBoxColumn DisableUserRole;
    }
}
