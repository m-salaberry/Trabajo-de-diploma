namespace UI.controlForms
{
    partial class ctrlStock
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
            dgvItemsAndStock = new DataGridView();
            label1 = new Label();
            ItemName = new DataGridViewTextBoxColumn();
            ItemCategory = new DataGridViewTextBoxColumn();
            ItemUnit = new DataGridViewTextBoxColumn();
            ItemStock = new DataGridViewTextBoxColumn();
            ItemUpdatedDate = new DataGridViewTextBoxColumn();
            txtSearchItem = new TextBox();
            cmbCategories = new ComboBox();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvItemsAndStock).BeginInit();
            SuspendLayout();
            // 
            // dgvItemsAndStock
            // 
            dgvItemsAndStock.AllowUserToAddRows = false;
            dgvItemsAndStock.AllowUserToDeleteRows = false;
            dgvItemsAndStock.AllowUserToResizeColumns = false;
            dgvItemsAndStock.AllowUserToResizeRows = false;
            dgvItemsAndStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItemsAndStock.Columns.AddRange(new DataGridViewColumn[] { ItemName, ItemCategory, ItemUnit, ItemStock, ItemUpdatedDate });
            dgvItemsAndStock.Location = new Point(3, 62);
            dgvItemsAndStock.Name = "dgvItemsAndStock";
            dgvItemsAndStock.ReadOnly = true;
            dgvItemsAndStock.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvItemsAndStock.Size = new Size(693, 358);
            dgvItemsAndStock.TabIndex = 0;
            dgvItemsAndStock.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(162, 30);
            label1.TabIndex = 8;
            label1.Text = "Items Stock List";
            // 
            // ItemName
            // 
            ItemName.HeaderText = "Name";
            ItemName.Name = "ItemName";
            ItemName.ReadOnly = true;
            ItemName.Width = 150;
            // 
            // ItemCategory
            // 
            ItemCategory.HeaderText = "Category";
            ItemCategory.Name = "ItemCategory";
            ItemCategory.ReadOnly = true;
            ItemCategory.Width = 150;
            // 
            // ItemUnit
            // 
            ItemUnit.HeaderText = "Unit";
            ItemUnit.Name = "ItemUnit";
            ItemUnit.ReadOnly = true;
            ItemUnit.Width = 120;
            // 
            // ItemStock
            // 
            ItemStock.HeaderText = "Current Stock";
            ItemStock.Name = "ItemStock";
            ItemStock.ReadOnly = true;
            ItemStock.Width = 120;
            // 
            // ItemUpdatedDate
            // 
            ItemUpdatedDate.HeaderText = "Last Updated";
            ItemUpdatedDate.Name = "ItemUpdatedDate";
            ItemUpdatedDate.ReadOnly = true;
            ItemUpdatedDate.Width = 110;
            // 
            // txtSearchItem
            // 
            txtSearchItem.Location = new Point(3, 33);
            txtSearchItem.Name = "txtSearchItem";
            txtSearchItem.PlaceholderText = "Search Item...";
            txtSearchItem.Size = new Size(180, 23);
            txtSearchItem.TabIndex = 9;
            // 
            // cmbCategories
            // 
            cmbCategories.FormattingEnabled = true;
            cmbCategories.Location = new Point(189, 33);
            cmbCategories.Name = "cmbCategories";
            cmbCategories.Size = new Size(180, 23);
            cmbCategories.TabIndex = 10;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(660, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(37, 24);
            btnClose.TabIndex = 11;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // ctrlStock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnClose);
            Controls.Add(cmbCategories);
            Controls.Add(txtSearchItem);
            Controls.Add(label1);
            Controls.Add(dgvItemsAndStock);
            Name = "ctrlStock";
            Size = new Size(700, 569);
            ((System.ComponentModel.ISupportInitialize)dgvItemsAndStock).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvItemsAndStock;
        private Label label1;
        private DataGridViewTextBoxColumn ItemName;
        private DataGridViewTextBoxColumn ItemCategory;
        private DataGridViewTextBoxColumn ItemUnit;
        private DataGridViewTextBoxColumn ItemStock;
        private DataGridViewTextBoxColumn ItemUpdatedDate;
        private TextBox txtSearchItem;
        private ComboBox cmbCategories;
        private Button btnClose;
    }
}
