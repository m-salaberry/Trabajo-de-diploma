namespace UI.controlForms
{
    partial class ctrlAnalytics
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
            dtpFrom = new DateTimePicker();
            dataGridView1 = new DataGridView();
            CategoryOfPurchase = new DataGridViewTextBoxColumn();
            TotalOrders = new DataGridViewTextBoxColumn();
            TotalSpent = new DataGridViewTextBoxColumn();
            Percentage = new DataGridViewTextBoxColumn();
            dataGridView2 = new DataGridView();
            ProviderOfPurchase = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dtpTo = new DateTimePicker();
            label1 = new Label();
            lbAnalytics = new Label();
            label2 = new Label();
            btnGenerate = new Button();
            btnSendToEmail = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // dtpFrom
            // 
            dtpFrom.Location = new Point(3, 76);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(220, 23);
            dtpFrom.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { CategoryOfPurchase, TotalOrders, TotalSpent, Percentage });
            dataGridView1.Location = new Point(3, 131);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size(543, 245);
            dataGridView1.TabIndex = 2;
            // 
            // CategoryOfPurchase
            // 
            CategoryOfPurchase.HeaderText = "Category of Purchase";
            CategoryOfPurchase.Name = "CategoryOfPurchase";
            CategoryOfPurchase.ReadOnly = true;
            CategoryOfPurchase.Width = 150;
            // 
            // TotalOrders
            // 
            TotalOrders.HeaderText = "Total Orders";
            TotalOrders.Name = "TotalOrders";
            TotalOrders.ReadOnly = true;
            // 
            // TotalSpent
            // 
            TotalSpent.HeaderText = "Total Spent";
            TotalSpent.Name = "TotalSpent";
            TotalSpent.ReadOnly = true;
            // 
            // Percentage
            // 
            Percentage.HeaderText = "% of Total Spending";
            Percentage.Name = "Percentage";
            Percentage.ReadOnly = true;
            Percentage.Width = 150;
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { ProviderOfPurchase, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            dataGridView2.Location = new Point(3, 391);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.Size = new Size(543, 245);
            dataGridView2.TabIndex = 3;
            // 
            // ProviderOfPurchase
            // 
            ProviderOfPurchase.HeaderText = "Provider";
            ProviderOfPurchase.Name = "ProviderOfPurchase";
            ProviderOfPurchase.ReadOnly = true;
            ProviderOfPurchase.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Total Orders";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Total Spent";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "% of Total Spending";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dtpTo
            // 
            dtpTo.Location = new Point(229, 76);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(220, 23);
            dtpTo.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 58);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 5;
            label1.Text = "From:";
            // 
            // lbAnalytics
            // 
            lbAnalytics.AutoSize = true;
            lbAnalytics.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbAnalytics.Location = new Point(0, 0);
            lbAnalytics.Name = "lbAnalytics";
            lbAnalytics.Size = new Size(187, 30);
            lbAnalytics.TabIndex = 6;
            lbAnalytics.Text = "Purchase Statistics";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(229, 58);
            label2.Name = "label2";
            label2.Size = new Size(23, 15);
            label2.TabIndex = 7;
            label2.Text = "To:";
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(455, 78);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(91, 23);
            btnGenerate.TabIndex = 8;
            btnGenerate.Text = "Generate";
            btnGenerate.UseVisualStyleBackColor = true;
            // 
            // btnSendToEmail
            // 
            btnSendToEmail.Location = new Point(424, 642);
            btnSendToEmail.Name = "btnSendToEmail";
            btnSendToEmail.Size = new Size(122, 23);
            btnSendToEmail.TabIndex = 9;
            btnSendToEmail.Text = "Send to Email";
            btnSendToEmail.UseVisualStyleBackColor = true;
            btnSendToEmail.Click += btnSendToEmail_Click;
            // 
            // ctrlAnalytics
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(btnSendToEmail);
            Controls.Add(btnGenerate);
            Controls.Add(label2);
            Controls.Add(lbAnalytics);
            Controls.Add(label1);
            Controls.Add(dtpTo);
            Controls.Add(dataGridView2);
            Controls.Add(dataGridView1);
            Controls.Add(dtpFrom);
            Name = "ctrlAnalytics";
            Size = new Size(554, 669);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dtpFrom;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn CategoryOfPurchase;
        private DataGridViewTextBoxColumn TotalOrders;
        private DataGridViewTextBoxColumn TotalSpent;
        private DataGridViewTextBoxColumn Percentage;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn ProviderOfPurchase;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DateTimePicker dtpTo;
        private Label label1;
        private Label lbAnalytics;
        private Label label2;
        private Button btnGenerate;
        private Button btnSendToEmail;
    }
}
