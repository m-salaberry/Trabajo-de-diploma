namespace UI.secondaryForms
{
    partial class deleteItemForm
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
            btnDeleteItem = new Button();
            cmbFilterCategories = new ComboBox();
            lstbxItems = new ListBox();
            SuspendLayout();
            // 
            // btnDeleteItem
            // 
            btnDeleteItem.Location = new Point(68, 269);
            btnDeleteItem.Name = "btnDeleteItem";
            btnDeleteItem.Size = new Size(74, 35);
            btnDeleteItem.TabIndex = 1;
            btnDeleteItem.Text = "Delete";
            btnDeleteItem.UseVisualStyleBackColor = true;
            btnDeleteItem.Click += btnDeleteItem_Click;
            // 
            // cmbFilterCategories
            // 
            cmbFilterCategories.FormattingEnabled = true;
            cmbFilterCategories.Location = new Point(12, 12);
            cmbFilterCategories.Name = "cmbFilterCategories";
            cmbFilterCategories.Size = new Size(186, 23);
            cmbFilterCategories.TabIndex = 2;
            cmbFilterCategories.SelectedIndexChanged += cmbFilterCategories_SelectedIndexChanged;
            // 
            // lstbxItems
            // 
            lstbxItems.FormattingEnabled = true;
            lstbxItems.ItemHeight = 15;
            lstbxItems.Location = new Point(13, 41);
            lstbxItems.Name = "lstbxItems";
            lstbxItems.Size = new Size(185, 214);
            lstbxItems.TabIndex = 3;
            // 
            // deleteItemForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(209, 316);
            Controls.Add(lstbxItems);
            Controls.Add(cmbFilterCategories);
            Controls.Add(btnDeleteItem);
            Name = "deleteItemForm";
            Text = "Delete Item";
            ResumeLayout(false);
        }

        #endregion

        private Button btnDeleteItem;
        private ComboBox cmbFilterCategories;
        private ListBox lstbxItems;
    }
}