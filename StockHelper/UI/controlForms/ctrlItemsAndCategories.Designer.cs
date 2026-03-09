namespace UI.controlForms
{
    partial class ctrlItemsAndCategories
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
            splitContainer1 = new SplitContainer();
            btnClearFilter = new Button();
            btnFilter = new Button();
            lstbxCategories = new CheckedListBox();
            deleteCategory = new Button();
            lbCategories = new Label();
            btnAddCategory = new Button();
            btnDeteleItem = new Button();
            btnModItem = new Button();
            btnCreateItem = new Button();
            btnClose = new Button();
            lbItems = new Label();
            dgvItems = new DataGridView();
            NameColumn = new DataGridViewTextBoxColumn();
            CategoryColumn = new DataGridViewTextBoxColumn();
            UnitColumn = new DataGridViewTextBoxColumn();
            IntegerUnitColumn = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btnClearFilter);
            splitContainer1.Panel1.Controls.Add(btnFilter);
            splitContainer1.Panel1.Controls.Add(lstbxCategories);
            splitContainer1.Panel1.Controls.Add(deleteCategory);
            splitContainer1.Panel1.Controls.Add(lbCategories);
            splitContainer1.Panel1.Controls.Add(btnAddCategory);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btnDeteleItem);
            splitContainer1.Panel2.Controls.Add(btnModItem);
            splitContainer1.Panel2.Controls.Add(btnCreateItem);
            splitContainer1.Panel2.Controls.Add(btnClose);
            splitContainer1.Panel2.Controls.Add(lbItems);
            splitContainer1.Panel2.Controls.Add(dgvItems);
            splitContainer1.Size = new Size(805, 455);
            splitContainer1.SplitterDistance = 260;
            splitContainer1.TabIndex = 0;
            // 
            // btnClearFilter
            // 
            btnClearFilter.Location = new Point(133, 352);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(124, 26);
            btnClearFilter.TabIndex = 6;
            btnClearFilter.Text = "Clear Filter";
            btnClearFilter.UseVisualStyleBackColor = true;
            btnClearFilter.Click += btnClearFilter_Click;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(3, 352);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(124, 26);
            btnFilter.TabIndex = 5;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click;
            // 
            // lstbxCategories
            // 
            lstbxCategories.FormattingEnabled = true;
            lstbxCategories.Location = new Point(3, 36);
            lstbxCategories.Name = "lstbxCategories";
            lstbxCategories.Size = new Size(254, 310);
            lstbxCategories.TabIndex = 4;
            lstbxCategories.TabStop = false;
            // 
            // deleteCategory
            // 
            deleteCategory.Location = new Point(3, 423);
            deleteCategory.Name = "deleteCategory";
            deleteCategory.Size = new Size(254, 26);
            deleteCategory.TabIndex = 3;
            deleteCategory.Text = "Delete Category";
            deleteCategory.UseVisualStyleBackColor = true;
            deleteCategory.Click += deleteCategory_Click;
            // 
            // lbCategories
            // 
            lbCategories.AutoSize = true;
            lbCategories.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbCategories.Location = new Point(3, 3);
            lbCategories.Name = "lbCategories";
            lbCategories.Size = new Size(115, 30);
            lbCategories.TabIndex = 2;
            lbCategories.Text = "Categories";
            // 
            // btnAddCategory
            // 
            btnAddCategory.Location = new Point(3, 391);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(254, 26);
            btnAddCategory.TabIndex = 0;
            btnAddCategory.Text = "New Category";
            btnAddCategory.UseVisualStyleBackColor = true;
            btnAddCategory.Click += btnAddCategory_Click;
            // 
            // btnDeteleItem
            // 
            btnDeteleItem.Location = new Point(387, 399);
            btnDeteleItem.Name = "btnDeteleItem";
            btnDeteleItem.Size = new Size(112, 42);
            btnDeteleItem.TabIndex = 9;
            btnDeteleItem.Text = "Delete Item";
            btnDeteleItem.UseVisualStyleBackColor = true;
            btnDeteleItem.Click += btnDeteleItem_Click;
            // 
            // btnModItem
            // 
            btnModItem.Location = new Point(209, 399);
            btnModItem.Name = "btnModItem";
            btnModItem.Size = new Size(112, 42);
            btnModItem.TabIndex = 8;
            btnModItem.Text = "Modify Item";
            btnModItem.UseVisualStyleBackColor = true;
            btnModItem.Click += btnModItem_Click;
            // 
            // btnCreateItem
            // 
            btnCreateItem.Location = new Point(30, 399);
            btnCreateItem.Name = "btnCreateItem";
            btnCreateItem.Size = new Size(112, 42);
            btnCreateItem.TabIndex = 7;
            btnCreateItem.Text = "Create new Item";
            btnCreateItem.UseVisualStyleBackColor = true;
            btnCreateItem.Click += btnCreateItem_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(499, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(37, 24);
            btnClose.TabIndex = 6;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // lbItems
            // 
            lbItems.AutoSize = true;
            lbItems.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbItems.Location = new Point(3, 3);
            lbItems.Name = "lbItems";
            lbItems.Size = new Size(66, 30);
            lbItems.TabIndex = 3;
            lbItems.Text = "Items";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AllowUserToResizeColumns = false;
            dgvItems.AllowUserToResizeRows = false;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { NameColumn, CategoryColumn, UnitColumn, IntegerUnitColumn });
            dgvItems.Location = new Point(3, 36);
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.ScrollBars = ScrollBars.Vertical;
            dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvItems.Size = new Size(533, 349);
            dgvItems.TabIndex = 0;
            dgvItems.TabStop = false;
            // 
            // NameColumn
            // 
            NameColumn.HeaderText = "Name";
            NameColumn.Name = "NameColumn";
            NameColumn.ReadOnly = true;
            NameColumn.Width = 200;
            // 
            // CategoryColumn
            // 
            CategoryColumn.HeaderText = "Category";
            CategoryColumn.Name = "CategoryColumn";
            CategoryColumn.ReadOnly = true;
            CategoryColumn.Width = 120;
            // 
            // UnitColumn
            // 
            UnitColumn.HeaderText = "Unit";
            UnitColumn.Name = "UnitColumn";
            UnitColumn.ReadOnly = true;
            UnitColumn.Width = 70;
            // 
            // IntegerUnitColumn
            // 
            IntegerUnitColumn.HeaderText = "Integer Unit";
            IntegerUnitColumn.Name = "IntegerUnitColumn";
            IntegerUnitColumn.ReadOnly = true;
            // 
            // ctrlItemsAndCategories
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(splitContainer1);
            Name = "ctrlItemsAndCategories";
            Size = new Size(805, 455);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Button btnAddCategory;
        private Button deleteCategory;
        private Label lbCategories;
        private Label lbItems;
        private DataGridView dgvItems;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn CategoryColumn;
        private DataGridViewTextBoxColumn UnitColumn;
        private DataGridViewTextBoxColumn IntegerUnitColumn;
        private Button btnClose;
        private Button btnDeteleItem;
        private Button btnModItem;
        private Button btnCreateItem;
        private CheckedListBox lstbxCategories;
        private Button btnClearFilter;
        private Button btnFilter;
    }
}
