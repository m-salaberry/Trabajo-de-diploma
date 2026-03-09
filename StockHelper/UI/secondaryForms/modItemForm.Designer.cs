namespace UI.secondaryForms
{
    partial class modItemForm
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
            ckIntegerUnit = new CheckBox();
            label3 = new Label();
            txtUnit = new TextBox();
            label2 = new Label();
            cmbCategories = new ComboBox();
            label1 = new Label();
            txtItemName = new TextBox();
            btnSaveChangesItem = new Button();
            cmbFilterCategory = new ComboBox();
            cmbItems = new ComboBox();
            SuspendLayout();
            // 
            // ckIntegerUnit
            // 
            ckIntegerUnit.AutoSize = true;
            ckIntegerUnit.CheckAlign = ContentAlignment.MiddleRight;
            ckIntegerUnit.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            ckIntegerUnit.Location = new Point(12, 293);
            ckIntegerUnit.Name = "ckIntegerUnit";
            ckIntegerUnit.Size = new Size(113, 24);
            ckIntegerUnit.TabIndex = 16;
            ckIntegerUnit.Text = "Integer Unit";
            ckIntegerUnit.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label3.Location = new Point(12, 226);
            label3.Name = "label3";
            label3.Size = new Size(159, 20);
            label3.TabIndex = 15;
            label3.Text = "Unit of measurement";
            // 
            // txtUnit
            // 
            txtUnit.Location = new Point(12, 249);
            txtUnit.Name = "txtUnit";
            txtUnit.Size = new Size(246, 23);
            txtUnit.TabIndex = 14;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label2.Location = new Point(12, 157);
            label2.Name = "label2";
            label2.Size = new Size(73, 20);
            label2.TabIndex = 13;
            label2.Text = "Category";
            // 
            // cmbCategories
            // 
            cmbCategories.FormattingEnabled = true;
            cmbCategories.Location = new Point(12, 180);
            cmbCategories.Name = "cmbCategories";
            cmbCategories.Size = new Size(246, 23);
            cmbCategories.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label1.Location = new Point(12, 88);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 11;
            label1.Text = "Name";
            // 
            // txtItemName
            // 
            txtItemName.Location = new Point(12, 111);
            txtItemName.Name = "txtItemName";
            txtItemName.Size = new Size(246, 23);
            txtItemName.TabIndex = 10;
            // 
            // btnSaveChangesItem
            // 
            btnSaveChangesItem.Location = new Point(76, 348);
            btnSaveChangesItem.Name = "btnSaveChangesItem";
            btnSaveChangesItem.Size = new Size(118, 35);
            btnSaveChangesItem.TabIndex = 9;
            btnSaveChangesItem.Text = "Save changes";
            btnSaveChangesItem.UseVisualStyleBackColor = true;
            btnSaveChangesItem.Click += btnSaveChangesItem_Click;
            // 
            // cmbFilterCategory
            // 
            cmbFilterCategory.FormattingEnabled = true;
            cmbFilterCategory.Location = new Point(12, 31);
            cmbFilterCategory.Name = "cmbFilterCategory";
            cmbFilterCategory.Size = new Size(121, 23);
            cmbFilterCategory.TabIndex = 17;
            cmbFilterCategory.SelectedIndexChanged += cmbFilterCategory_SelectedIndexChanged;
            // 
            // cmbItems
            // 
            cmbItems.FormattingEnabled = true;
            cmbItems.Location = new Point(139, 31);
            cmbItems.Name = "cmbItems";
            cmbItems.Size = new Size(121, 23);
            cmbItems.TabIndex = 18;
            cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
            // 
            // modItemForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(270, 395);
            Controls.Add(cmbItems);
            Controls.Add(cmbFilterCategory);
            Controls.Add(ckIntegerUnit);
            Controls.Add(label3);
            Controls.Add(txtUnit);
            Controls.Add(label2);
            Controls.Add(cmbCategories);
            Controls.Add(label1);
            Controls.Add(txtItemName);
            Controls.Add(btnSaveChangesItem);
            Name = "modItemForm";
            Text = "Modify Item";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox ckIntegerUnit;
        private Label label3;
        private TextBox txtUnit;
        private Label label2;
        private ComboBox cmbCategories;
        private Label label1;
        private TextBox txtItemName;
        private Button btnSaveChangesItem;
        private ComboBox cmbFilterCategory;
        private ComboBox cmbItems;
    }
}