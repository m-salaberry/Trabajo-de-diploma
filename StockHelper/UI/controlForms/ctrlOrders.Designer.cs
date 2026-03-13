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
            dataGridView1 = new DataGridView();
            ItemName = new DataGridViewTextBoxColumn();
            ItemQuantity = new DataGridViewTextBoxColumn();
            ItemAction = new DataGridViewButtonColumn();
            listBox1 = new ListBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ItemName, ItemQuantity, ItemAction });
            dataGridView1.Location = new Point(302, 74);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size(393, 379);
            dataGridView1.TabIndex = 0;
            // 
            // ItemName
            // 
            ItemName.HeaderText = "Item Name";
            ItemName.Name = "ItemName";
            ItemName.ReadOnly = true;
            ItemName.Width = 150;
            // 
            // ItemQuantity
            // 
            ItemQuantity.HeaderText = "Quantity";
            ItemQuantity.Name = "ItemQuantity";
            ItemQuantity.ReadOnly = true;
            // 
            // ItemAction
            // 
            ItemAction.HeaderText = "Actions";
            ItemAction.Name = "ItemAction";
            ItemAction.ReadOnly = true;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(214, 25);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(120, 94);
            listBox1.TabIndex = 2;
            // 
            // ctrlOrders
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(listBox1);
            Controls.Add(dataGridView1);
            Name = "ctrlOrders";
            Size = new Size(698, 456);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn ItemName;
        private DataGridViewTextBoxColumn ItemQuantity;
        private DataGridViewButtonColumn ItemAction;
        private ListBox listBox1;
    }
}
