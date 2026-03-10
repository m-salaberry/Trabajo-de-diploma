namespace UI.secondaryForms
{
    partial class newProviderForm
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
            txtName = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtCompanyName = new TextBox();
            label4 = new Label();
            txtCellNumber = new TextBox();
            label5 = new Label();
            txtEmail = new TextBox();
            btnSaveProvider = new Button();
            cmbCategories = new ComboBox();
            label6 = new Label();
            txtCUIT = new MaskedTextBox();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new Point(12, 32);
            txtName.Name = "txtName";
            txtName.Size = new Size(201, 23);
            txtName.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 2;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label2.Location = new Point(12, 123);
            label2.Name = "label2";
            label2.Size = new Size(43, 20);
            label2.TabIndex = 3;
            label2.Text = "CUIT";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label3.Location = new Point(12, 180);
            label3.Name = "label3";
            label3.Size = new Size(118, 20);
            label3.TabIndex = 5;
            label3.Text = "Company name";
            // 
            // txtCompanyName
            // 
            txtCompanyName.Location = new Point(12, 203);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new Size(201, 23);
            txtCompanyName.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label4.Location = new Point(12, 237);
            label4.Name = "label4";
            label4.Size = new Size(144, 20);
            label4.TabIndex = 7;
            label4.Text = "Cell Phone Number";
            // 
            // txtCellNumber
            // 
            txtCellNumber.Location = new Point(12, 260);
            txtCellNumber.Name = "txtCellNumber";
            txtCellNumber.Size = new Size(201, 23);
            txtCellNumber.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label5.Location = new Point(12, 294);
            label5.Name = "label5";
            label5.Size = new Size(47, 20);
            label5.TabIndex = 9;
            label5.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(12, 317);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(201, 23);
            txtEmail.TabIndex = 8;
            // 
            // btnSaveProvider
            // 
            btnSaveProvider.Location = new Point(71, 358);
            btnSaveProvider.Name = "btnSaveProvider";
            btnSaveProvider.Size = new Size(82, 36);
            btnSaveProvider.TabIndex = 10;
            btnSaveProvider.Text = "Save";
            btnSaveProvider.UseVisualStyleBackColor = true;
            btnSaveProvider.Click += btnSaveProvider_Click;
            // 
            // cmbCategories
            // 
            cmbCategories.FormattingEnabled = true;
            cmbCategories.Location = new Point(12, 89);
            cmbCategories.Name = "cmbCategories";
            cmbCategories.Size = new Size(201, 23);
            cmbCategories.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label6.Location = new Point(12, 66);
            label6.Name = "label6";
            label6.Size = new Size(73, 20);
            label6.TabIndex = 12;
            label6.Text = "Category";
            // 
            // txtCUIT
            // 
            txtCUIT.Location = new Point(12, 146);
            txtCUIT.Mask = "00-00000000-0";
            txtCUIT.Name = "txtCUIT";
            txtCUIT.PromptChar = 'X';
            txtCUIT.Size = new Size(201, 23);
            txtCUIT.TabIndex = 13;
            // 
            // newProviderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(225, 402);
            Controls.Add(txtCUIT);
            Controls.Add(label6);
            Controls.Add(cmbCategories);
            Controls.Add(btnSaveProvider);
            Controls.Add(label5);
            Controls.Add(txtEmail);
            Controls.Add(label4);
            Controls.Add(txtCellNumber);
            Controls.Add(label3);
            Controls.Add(txtCompanyName);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtName);
            Name = "newProviderForm";
            Text = "Create Provider";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtName;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtCompanyName;
        private Label label4;
        private TextBox txtCellNumber;
        private Label label5;
        private TextBox txtEmail;
        private Button btnSaveProvider;
        private ComboBox cmbCategories;
        private Label label6;
        private MaskedTextBox txtCUIT;
    }
}