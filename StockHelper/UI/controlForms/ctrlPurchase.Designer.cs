namespace UI.controlForms
{
    partial class ctrlPurchase
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
            dgvPurchaseOrders = new DataGridView();
            OrderName = new DataGridViewTextBoxColumn();
            OrderProvider = new DataGridViewTextBoxColumn();
            OrderCategory = new DataGridViewTextBoxColumn();
            OrderStatus = new DataGridViewTextBoxColumn();
            OrderAmount = new DataGridViewTextBoxColumn();
            btnCancelOrder = new Button();
            btnUploadInvoice = new Button();
            label1 = new Label();
            txtFilterByProvider = new TextBox();
            cmbFilterStatus = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvPurchaseOrders).BeginInit();
            SuspendLayout();
            // 
            // dgvPurchaseOrders
            // 
            dgvPurchaseOrders.AllowUserToAddRows = false;
            dgvPurchaseOrders.AllowUserToDeleteRows = false;
            dgvPurchaseOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPurchaseOrders.Columns.AddRange(new DataGridViewColumn[] { OrderName, OrderProvider, OrderCategory, OrderStatus, OrderAmount });
            dgvPurchaseOrders.Location = new Point(3, 57);
            dgvPurchaseOrders.Name = "dgvPurchaseOrders";
            dgvPurchaseOrders.ReadOnly = true;
            dgvPurchaseOrders.Size = new Size(543, 340);
            dgvPurchaseOrders.TabIndex = 0;
            // 
            // OrderName
            // 
            OrderName.HeaderText = "Order Name";
            OrderName.Name = "OrderName";
            OrderName.ReadOnly = true;
            // 
            // OrderProvider
            // 
            OrderProvider.HeaderText = "Provider";
            OrderProvider.Name = "OrderProvider";
            OrderProvider.ReadOnly = true;
            // 
            // OrderCategory
            // 
            OrderCategory.HeaderText = "Cateogry";
            OrderCategory.Name = "OrderCategory";
            OrderCategory.ReadOnly = true;
            // 
            // OrderStatus
            // 
            OrderStatus.HeaderText = "Status";
            OrderStatus.Name = "OrderStatus";
            OrderStatus.ReadOnly = true;
            // 
            // OrderAmount
            // 
            OrderAmount.HeaderText = "Total Amount";
            OrderAmount.Name = "OrderAmount";
            OrderAmount.ReadOnly = true;
            // 
            // btnCancelOrder
            // 
            btnCancelOrder.Location = new Point(280, 403);
            btnCancelOrder.Name = "btnCancelOrder";
            btnCancelOrder.Size = new Size(130, 32);
            btnCancelOrder.TabIndex = 1;
            btnCancelOrder.Text = "Cancel Order";
            btnCancelOrder.UseVisualStyleBackColor = true;
            // 
            // btnUploadInvoice
            // 
            btnUploadInvoice.Location = new Point(416, 403);
            btnUploadInvoice.Name = "btnUploadInvoice";
            btnUploadInvoice.Size = new Size(130, 32);
            btnUploadInvoice.TabIndex = 2;
            btnUploadInvoice.Text = "Upload Invoice";
            btnUploadInvoice.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 13.75F, FontStyle.Bold);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(156, 25);
            label1.TabIndex = 14;
            label1.Text = "Purchase Orders:";
            // 
            // txtFilterByProvider
            // 
            txtFilterByProvider.Location = new Point(3, 28);
            txtFilterByProvider.Name = "txtFilterByProvider";
            txtFilterByProvider.PlaceholderText = "Search by Provider...";
            txtFilterByProvider.Size = new Size(189, 23);
            txtFilterByProvider.TabIndex = 15;
            // 
            // cmbFilterStatus
            // 
            cmbFilterStatus.FormattingEnabled = true;
            cmbFilterStatus.Location = new Point(198, 28);
            cmbFilterStatus.Name = "cmbFilterStatus";
            cmbFilterStatus.Size = new Size(162, 23);
            cmbFilterStatus.TabIndex = 16;
            // 
            // ctrlPurchase
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(cmbFilterStatus);
            Controls.Add(txtFilterByProvider);
            Controls.Add(label1);
            Controls.Add(btnUploadInvoice);
            Controls.Add(btnCancelOrder);
            Controls.Add(dgvPurchaseOrders);
            Name = "ctrlPurchase";
            Size = new Size(551, 440);
            ((System.ComponentModel.ISupportInitialize)dgvPurchaseOrders).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvPurchaseOrders;
        private DataGridViewTextBoxColumn OrderName;
        private DataGridViewTextBoxColumn OrderProvider;
        private DataGridViewTextBoxColumn OrderCategory;
        private DataGridViewTextBoxColumn OrderStatus;
        private DataGridViewTextBoxColumn OrderAmount;
        private Button btnCancelOrder;
        private Button btnUploadInvoice;
        private Label label1;
        private TextBox txtFilterByProvider;
        private ComboBox cmbFilterStatus;
    }
}
