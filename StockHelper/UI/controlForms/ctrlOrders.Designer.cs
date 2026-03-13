namespace UI.controlForms
{
    partial class ctrlOrders
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
            dgvOrders = new DataGridView();
            ItemName = new DataGridViewTextBoxColumn();
            ItemUnit = new DataGridViewTextBoxColumn();
            ItemQuantity = new DataGridViewTextBoxColumn();
            lstbxOrders = new ListBox();
            txtFilterByProvider = new TextBox();
            label1 = new Label();
            btnCreateNewReplacementOrder = new Button();
            btnModifyReplacementOrder = new Button();
            btnDeleteOrder = new Button();
            gpbxOrder = new GroupBox();
            btnCancel = new Button();
            btnSave = new Button();
            lbOrder = new Label();
            btnSendOrder = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            gpbxOrder.SuspendLayout();
            SuspendLayout();
            // 
            // dgvOrders
            // 
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.AllowUserToDeleteRows = false;
            dgvOrders.AllowUserToResizeColumns = false;
            dgvOrders.AllowUserToResizeRows = false;
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Columns.AddRange(new DataGridViewColumn[] { ItemName, ItemUnit, ItemQuantity });
            dgvOrders.Location = new Point(6, 43);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.ReadOnly = true;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvOrders.Size = new Size(393, 414);
            dgvOrders.TabIndex = 0;
            // 
            // ItemName
            // 
            ItemName.HeaderText = "Item Name";
            ItemName.Name = "ItemName";
            ItemName.ReadOnly = true;
            ItemName.Width = 150;
            // 
            // ItemUnit
            // 
            ItemUnit.HeaderText = "Unit";
            ItemUnit.Name = "ItemUnit";
            ItemUnit.ReadOnly = true;
            // 
            // ItemQuantity
            // 
            ItemQuantity.HeaderText = "Quantity";
            ItemQuantity.Name = "ItemQuantity";
            ItemQuantity.ReadOnly = true;
            // 
            // lstbxOrders
            // 
            lstbxOrders.FormattingEnabled = true;
            lstbxOrders.ItemHeight = 15;
            lstbxOrders.Location = new Point(3, 57);
            lstbxOrders.Name = "lstbxOrders";
            lstbxOrders.Size = new Size(280, 334);
            lstbxOrders.TabIndex = 1;
            lstbxOrders.SelectedIndexChanged += lstbxOrders_SelectedIndexChanged;
            // 
            // txtFilterByProvider
            // 
            txtFilterByProvider.Location = new Point(3, 28);
            txtFilterByProvider.Name = "txtFilterByProvider";
            txtFilterByProvider.PlaceholderText = "Search by Provider...";
            txtFilterByProvider.Size = new Size(280, 23);
            txtFilterByProvider.TabIndex = 2;
            txtFilterByProvider.TextChanged += txtFilterByProvider_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 13.75F, FontStyle.Bold);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(190, 25);
            label1.TabIndex = 13;
            label1.Text = "Replacement Orders:";
            // 
            // btnCreateNewReplacementOrder
            // 
            btnCreateNewReplacementOrder.Location = new Point(3, 397);
            btnCreateNewReplacementOrder.Name = "btnCreateNewReplacementOrder";
            btnCreateNewReplacementOrder.Size = new Size(280, 44);
            btnCreateNewReplacementOrder.TabIndex = 14;
            btnCreateNewReplacementOrder.Text = "Create new Replacement Order";
            btnCreateNewReplacementOrder.UseVisualStyleBackColor = true;
            btnCreateNewReplacementOrder.Click += btnCreateNewReplacementOrder_Click;
            // 
            // btnModifyReplacementOrder
            // 
            btnModifyReplacementOrder.Location = new Point(3, 447);
            btnModifyReplacementOrder.Name = "btnModifyReplacementOrder";
            btnModifyReplacementOrder.Size = new Size(140, 44);
            btnModifyReplacementOrder.TabIndex = 18;
            btnModifyReplacementOrder.Text = "Modify Replacement Order";
            btnModifyReplacementOrder.UseVisualStyleBackColor = true;
            btnModifyReplacementOrder.Click += btnModifyReplacementOrder_Click;
            // 
            // btnDeleteOrder
            // 
            btnDeleteOrder.Location = new Point(149, 447);
            btnDeleteOrder.Name = "btnDeleteOrder";
            btnDeleteOrder.Size = new Size(134, 44);
            btnDeleteOrder.TabIndex = 19;
            btnDeleteOrder.Text = "Delete Replacement Order";
            btnDeleteOrder.UseVisualStyleBackColor = true;
            btnDeleteOrder.Click += btnDeleteOrder_Click;
            // 
            // gpbxOrder
            // 
            gpbxOrder.Controls.Add(btnCancel);
            gpbxOrder.Controls.Add(btnSave);
            gpbxOrder.Controls.Add(lbOrder);
            gpbxOrder.Controls.Add(dgvOrders);
            gpbxOrder.Font = new Font("Segoe UI", 9F);
            gpbxOrder.Location = new Point(289, 28);
            gpbxOrder.Name = "gpbxOrder";
            gpbxOrder.Size = new Size(411, 496);
            gpbxOrder.TabIndex = 20;
            gpbxOrder.TabStop = false;
            gpbxOrder.Text = "Order Details:";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(6, 463);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(311, 463);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(88, 27);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save Order";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click_1;
            // 
            // lbOrder
            // 
            lbOrder.AutoSize = true;
            lbOrder.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lbOrder.Location = new Point(6, 19);
            lbOrder.Name = "lbOrder";
            lbOrder.Size = new Size(68, 21);
            lbOrder.TabIndex = 1;
            lbOrder.Text = "Order: -";
            // 
            // btnSendOrder
            // 
            btnSendOrder.Location = new Point(3, 497);
            btnSendOrder.Name = "btnSendOrder";
            btnSendOrder.Size = new Size(280, 27);
            btnSendOrder.TabIndex = 5;
            btnSendOrder.Text = "Send Order to Provider";
            btnSendOrder.UseVisualStyleBackColor = true;
            btnSendOrder.Click += btnSendOrder_Click;
            // 
            // ctrlOrders
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(btnSendOrder);
            Controls.Add(gpbxOrder);
            Controls.Add(btnDeleteOrder);
            Controls.Add(btnModifyReplacementOrder);
            Controls.Add(btnCreateNewReplacementOrder);
            Controls.Add(label1);
            Controls.Add(txtFilterByProvider);
            Controls.Add(lstbxOrders);
            Name = "ctrlOrders";
            Size = new Size(708, 532);
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
            gpbxOrder.ResumeLayout(false);
            gpbxOrder.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvOrders;
        private ListBox lstbxOrders;
        private TextBox txtFilterByProvider;
        private Label label1;
        private Button btnCreateNewReplacementOrder;
        private Button btnModifyReplacementOrder;
        private Button btnDeleteOrder;
        private GroupBox gpbxOrder;
        private Label lbOrder;
        private Button btnSave;
        private DataGridViewTextBoxColumn ItemName;
        private DataGridViewTextBoxColumn ItemUnit;
        private DataGridViewTextBoxColumn ItemQuantity;
        private Button btnCancel;
        private Button btnSendOrder;
    }
}
