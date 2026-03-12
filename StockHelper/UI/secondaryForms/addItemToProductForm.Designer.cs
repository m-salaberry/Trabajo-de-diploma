namespace UI.secondaryForms
{
    partial class addItemToProductForm
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
            cmbFilterCategories = new ComboBox();
            btnAdd = new Button();
            txtSearch = new TextBox();
            cklstItems = new CheckedListBox();
            SuspendLayout();
            // 
            // cmbFilterCategories
            // 
            cmbFilterCategories.FormattingEnabled = true;
            cmbFilterCategories.Location = new Point(11, 12);
            cmbFilterCategories.Name = "cmbFilterCategories";
            cmbFilterCategories.Size = new Size(186, 23);
            cmbFilterCategories.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(67, 350);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(74, 35);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 41);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search";
            txtSearch.Size = new Size(185, 23);
            txtSearch.TabIndex = 7;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // cklstItems
            // 
            cklstItems.CheckOnClick = true;
            cklstItems.FormattingEnabled = true;
            cklstItems.Location = new Point(11, 70);
            cklstItems.Name = "cklstItems";
            cklstItems.Size = new Size(186, 274);
            cklstItems.TabIndex = 8;
            // 
            // addItemToProductForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(209, 392);
            Controls.Add(cklstItems);
            Controls.Add(txtSearch);
            Controls.Add(cmbFilterCategories);
            Controls.Add(btnAdd);
            Name = "addItemToProductForm";
            Text = "Add Item";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ComboBox cmbFilterCategories;
        private Button btnAdd;
        private TextBox txtSearch;
        private CheckedListBox cklstItems;
    }
}