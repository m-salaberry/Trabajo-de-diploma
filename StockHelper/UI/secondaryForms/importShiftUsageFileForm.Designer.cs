namespace UI.secondaryForms
{
    partial class importShiftUsageFileForm
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
            btnBrowseAndLoadFile = new Button();
            txtDirectory = new TextBox();
            btnProcessFile = new Button();
            dgvItemsAndStock = new DataGridView();
            ItemName = new DataGridViewTextBoxColumn();
            ItemCategory = new DataGridViewTextBoxColumn();
            ItemUnit = new DataGridViewTextBoxColumn();
            ItemStock = new DataGridViewTextBoxColumn();
            ItemSubtract = new DataGridViewTextBoxColumn();
            ItemNewStock = new DataGridViewTextBoxColumn();
            btnCancel = new Button();
            btnSaveNewStock = new Button();
            cmbCategories = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvItemsAndStock).BeginInit();
            SuspendLayout();
            // 
            // btnBrowseAndLoadFile
            // 
            btnBrowseAndLoadFile.Location = new Point(12, 12);
            btnBrowseAndLoadFile.Name = "btnBrowseAndLoadFile";
            btnBrowseAndLoadFile.Size = new Size(143, 44);
            btnBrowseAndLoadFile.TabIndex = 0;
            btnBrowseAndLoadFile.Text = "Load Close Shift File";
            btnBrowseAndLoadFile.UseVisualStyleBackColor = true;
            btnBrowseAndLoadFile.Click += btnBrowseAndLoadFile_Click;
            // 
            // txtDirectory
            // 
            txtDirectory.Location = new Point(161, 24);
            txtDirectory.Name = "txtDirectory";
            txtDirectory.ReadOnly = true;
            txtDirectory.Size = new Size(411, 23);
            txtDirectory.TabIndex = 1;
            // 
            // btnProcessFile
            // 
            btnProcessFile.Location = new Point(578, 18);
            btnProcessFile.Name = "btnProcessFile";
            btnProcessFile.Size = new Size(167, 33);
            btnProcessFile.TabIndex = 2;
            btnProcessFile.Text = "Process Fle";
            btnProcessFile.UseVisualStyleBackColor = true;
            btnProcessFile.Click += btnProcessFile_Click;
            // 
            // dgvItemsAndStock
            // 
            dgvItemsAndStock.AllowUserToAddRows = false;
            dgvItemsAndStock.AllowUserToDeleteRows = false;
            dgvItemsAndStock.AllowUserToResizeColumns = false;
            dgvItemsAndStock.AllowUserToResizeRows = false;
            dgvItemsAndStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItemsAndStock.Columns.AddRange(new DataGridViewColumn[] { ItemName, ItemCategory, ItemUnit, ItemStock, ItemSubtract, ItemNewStock });
            dgvItemsAndStock.Location = new Point(12, 101);
            dgvItemsAndStock.Name = "dgvItemsAndStock";
            dgvItemsAndStock.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvItemsAndStock.Size = new Size(733, 372);
            dgvItemsAndStock.TabIndex = 3;
            dgvItemsAndStock.TabStop = false;
            // 
            // ItemName
            // 
            ItemName.HeaderText = "Name";
            ItemName.Name = "ItemName";
            ItemName.Width = 150;
            // 
            // ItemCategory
            // 
            ItemCategory.HeaderText = "Category";
            ItemCategory.Name = "ItemCategory";
            ItemCategory.Width = 150;
            // 
            // ItemUnit
            // 
            ItemUnit.HeaderText = "Unit";
            ItemUnit.Name = "ItemUnit";
            ItemUnit.Width = 80;
            // 
            // ItemStock
            // 
            ItemStock.HeaderText = "Current Stock";
            ItemStock.Name = "ItemStock";
            ItemStock.Width = 110;
            // 
            // ItemSubtract
            // 
            ItemSubtract.HeaderText = "Quantity to Subtract";
            ItemSubtract.Name = "ItemSubtract";
            // 
            // ItemNewStock
            // 
            ItemNewStock.HeaderText = "Calculated New Stock";
            ItemNewStock.Name = "ItemNewStock";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(651, 479);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 16;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSaveNewStock
            // 
            btnSaveNewStock.Location = new Point(531, 479);
            btnSaveNewStock.Name = "btnSaveNewStock";
            btnSaveNewStock.Size = new Size(114, 29);
            btnSaveNewStock.TabIndex = 17;
            btnSaveNewStock.Text = "Save Stock";
            btnSaveNewStock.UseVisualStyleBackColor = true;
            btnSaveNewStock.Click += btnSaveNewStock_Click;
            // 
            // cmbCategories
            // 
            cmbCategories.FormattingEnabled = true;
            cmbCategories.Location = new Point(12, 67);
            cmbCategories.Name = "cmbCategories";
            cmbCategories.Size = new Size(193, 23);
            cmbCategories.TabIndex = 18;
            cmbCategories.SelectedIndexChanged += cmbCategories_SelectedIndexChanged;
            // 
            // importShiftUsageFileForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(757, 520);
            Controls.Add(cmbCategories);
            Controls.Add(btnSaveNewStock);
            Controls.Add(btnCancel);
            Controls.Add(dgvItemsAndStock);
            Controls.Add(btnProcessFile);
            Controls.Add(txtDirectory);
            Controls.Add(btnBrowseAndLoadFile);
            Name = "importShiftUsageFileForm";
            Text = "Import Shift Usage";
            ((System.ComponentModel.ISupportInitialize)dgvItemsAndStock).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBrowseAndLoadFile;
        private TextBox txtDirectory;
        private Button btnProcessFile;
        private DataGridView dgvItemsAndStock;
        private Button btnCancel;
        private Button btnSaveNewStock;
        private DataGridViewTextBoxColumn ItemName;
        private DataGridViewTextBoxColumn ItemCategory;
        private DataGridViewTextBoxColumn ItemUnit;
        private DataGridViewTextBoxColumn ItemStock;
        private DataGridViewTextBoxColumn ItemSubtract;
        private DataGridViewTextBoxColumn ItemNewStock;
        private ComboBox cmbCategories;
    }
}