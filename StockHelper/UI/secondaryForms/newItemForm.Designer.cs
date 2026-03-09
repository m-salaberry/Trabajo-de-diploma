namespace UI.secondaryForms
{
    partial class newItemForm
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
            btnSaveItem = new Button();
            txtItemName = new TextBox();
            label1 = new Label();
            cmbCategories = new ComboBox();
            label2 = new Label();
            txtUnit = new TextBox();
            label3 = new Label();
            ckIntegerUnit = new CheckBox();
            SuspendLayout();
            // 
            // btnSaveItem
            // 
            btnSaveItem.Location = new Point(67, 270);
            btnSaveItem.Name = "btnSaveItem";
            btnSaveItem.Size = new Size(74, 35);
            btnSaveItem.TabIndex = 0;
            btnSaveItem.Text = "Save";
            btnSaveItem.UseVisualStyleBackColor = true;
            btnSaveItem.Click += btnSaveItem_Click;
            // 
            // txtItemName
            // 
            txtItemName.Location = new Point(12, 33);
            txtItemName.Name = "txtItemName";
            txtItemName.Size = new Size(185, 23);
            txtItemName.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label1.Location = new Point(12, 10);
            label1.Name = "label1";
            label1.Size = new Size(51, 20);
            label1.TabIndex = 2;
            label1.Text = "Name";
            // 
            // cmbCategories
            // 
            cmbCategories.FormattingEnabled = true;
            cmbCategories.Location = new Point(12, 102);
            cmbCategories.Name = "cmbCategories";
            cmbCategories.Size = new Size(185, 23);
            cmbCategories.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label2.Location = new Point(12, 79);
            label2.Name = "label2";
            label2.Size = new Size(73, 20);
            label2.TabIndex = 4;
            label2.Text = "Category";
            // 
            // txtUnit
            // 
            txtUnit.Location = new Point(12, 171);
            txtUnit.Name = "txtUnit";
            txtUnit.Size = new Size(185, 23);
            txtUnit.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label3.Location = new Point(12, 148);
            label3.Name = "label3";
            label3.Size = new Size(159, 20);
            label3.TabIndex = 6;
            label3.Text = "Unit of measurement";
            // 
            // ckIntegerUnit
            // 
            ckIntegerUnit.AutoSize = true;
            ckIntegerUnit.CheckAlign = ContentAlignment.MiddleRight;
            ckIntegerUnit.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            ckIntegerUnit.Location = new Point(12, 215);
            ckIntegerUnit.Name = "ckIntegerUnit";
            ckIntegerUnit.Size = new Size(113, 24);
            ckIntegerUnit.TabIndex = 8;
            ckIntegerUnit.Text = "Integer Unit";
            ckIntegerUnit.UseVisualStyleBackColor = true;
            // 
            // newItemForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(209, 316);
            Controls.Add(ckIntegerUnit);
            Controls.Add(label3);
            Controls.Add(txtUnit);
            Controls.Add(label2);
            Controls.Add(cmbCategories);
            Controls.Add(label1);
            Controls.Add(txtItemName);
            Controls.Add(btnSaveItem);
            Name = "newItemForm";
            Text = "New Item";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSaveItem;
        private TextBox txtItemName;
        private Label label1;
        private ComboBox cmbCategories;
        private Label label2;
        private TextBox txtUnit;
        private Label label3;
        private CheckBox ckIntegerUnit;
    }
}