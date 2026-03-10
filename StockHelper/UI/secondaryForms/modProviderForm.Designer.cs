namespace UI.secondaryForms
{
    partial class modProviderForm
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
            txtCUIT = new MaskedTextBox();
            label6 = new Label();
            cmbCategories = new ComboBox();
            btnSaveProvider = new Button();
            label5 = new Label();
            txtEmail = new TextBox();
            label4 = new Label();
            txtCellNumber = new TextBox();
            label3 = new Label();
            txtCompanyName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            txtName = new TextBox();
            cmbChooseCategory = new ComboBox();
            cmbChooseProvider = new ComboBox();
            SuspendLayout();
            // 
            // txtCUIT
            // 
            txtCUIT.Location = new Point(10, 190);
            txtCUIT.Mask = "00-00000000-0";
            txtCUIT.Name = "txtCUIT";
            txtCUIT.PromptChar = 'X';
            txtCUIT.Size = new Size(201, 23);
            txtCUIT.TabIndex = 26;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label6.Location = new Point(10, 110);
            label6.Name = "label6";
            label6.Size = new Size(73, 20);
            label6.TabIndex = 25;
            label6.Text = "Category";
            // 
            // cmbCategories
            // 
            cmbCategories.FormattingEnabled = true;
            cmbCategories.Location = new Point(10, 133);
            cmbCategories.Name = "cmbCategories";
            cmbCategories.Size = new Size(201, 23);
            cmbCategories.TabIndex = 24;
            // 
            // btnSaveProvider
            // 
            btnSaveProvider.Location = new Point(69, 402);
            btnSaveProvider.Name = "btnSaveProvider";
            btnSaveProvider.Size = new Size(82, 36);
            btnSaveProvider.TabIndex = 23;
            btnSaveProvider.Text = "Save";
            btnSaveProvider.UseVisualStyleBackColor = true;
            btnSaveProvider.Click += btnSaveProvider_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label5.Location = new Point(10, 338);
            label5.Name = "label5";
            label5.Size = new Size(47, 20);
            label5.TabIndex = 22;
            label5.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(10, 361);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(201, 23);
            txtEmail.TabIndex = 21;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label4.Location = new Point(10, 281);
            label4.Name = "label4";
            label4.Size = new Size(144, 20);
            label4.TabIndex = 20;
            label4.Text = "Cell Phone Number";
            // 
            // txtCellNumber
            // 
            txtCellNumber.Location = new Point(10, 304);
            txtCellNumber.Name = "txtCellNumber";
            txtCellNumber.Size = new Size(201, 23);
            txtCellNumber.TabIndex = 19;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label3.Location = new Point(10, 224);
            label3.Name = "label3";
            label3.Size = new Size(118, 20);
            label3.TabIndex = 18;
            label3.Text = "Company name";
            // 
            // txtCompanyName
            // 
            txtCompanyName.Location = new Point(10, 247);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new Size(201, 23);
            txtCompanyName.TabIndex = 17;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label2.Location = new Point(10, 167);
            label2.Name = "label2";
            label2.Size = new Size(43, 20);
            label2.TabIndex = 16;
            label2.Text = "CUIT";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label1.Location = new Point(10, 53);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 15;
            label1.Text = "Name";
            // 
            // txtName
            // 
            txtName.Location = new Point(10, 76);
            txtName.Name = "txtName";
            txtName.Size = new Size(201, 23);
            txtName.TabIndex = 14;
            // 
            // cmbChooseCategory
            // 
            cmbChooseCategory.FormattingEnabled = true;
            cmbChooseCategory.Location = new Point(7, 12);
            cmbChooseCategory.Name = "cmbChooseCategory";
            cmbChooseCategory.Size = new Size(100, 23);
            cmbChooseCategory.TabIndex = 27;
            // 
            // cmbChooseProvider
            // 
            cmbChooseProvider.FormattingEnabled = true;
            cmbChooseProvider.Location = new Point(113, 12);
            cmbChooseProvider.Name = "cmbChooseProvider";
            cmbChooseProvider.Size = new Size(100, 23);
            cmbChooseProvider.TabIndex = 28;
            // 
            // modProviderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(225, 450);
            Controls.Add(cmbChooseProvider);
            Controls.Add(cmbChooseCategory);
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
            Name = "modProviderForm";
            Text = "Modify Provider";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MaskedTextBox txtCUIT;
        private Label label6;
        private ComboBox cmbCategories;
        private Button btnSaveProvider;
        private Label label5;
        private TextBox txtEmail;
        private Label label4;
        private TextBox txtCellNumber;
        private Label label3;
        private TextBox txtCompanyName;
        private Label label2;
        private Label label1;
        private TextBox txtName;
        private ComboBox cmbChooseCategory;
        private ComboBox cmbChooseProvider;
    }
}