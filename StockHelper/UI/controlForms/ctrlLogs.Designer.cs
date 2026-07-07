namespace UI.controlForms
{
    partial class ctrlLogs
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
            dateTimePicker1 = new DateTimePicker();
            dateTimePicker2 = new DateTimePicker();
            label1 = new Label();
            label2 = new Label();
            cklbLogsLevels = new CheckedListBox();
            btnFilter = new Button();
            dataGridView1 = new DataGridView();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker1.Location = new Point(3, 62);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(129, 23);
            dateTimePicker1.TabIndex = 0;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Location = new Point(3, 111);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(129, 23);
            dateTimePicker2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 44);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 2;
            label1.Text = "From:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 93);
            label2.Name = "label2";
            label2.Size = new Size(23, 15);
            label2.TabIndex = 3;
            label2.Text = "To:";
            // 
            // cklbLogsLevels
            // 
            cklbLogsLevels.FormattingEnabled = true;
            cklbLogsLevels.Location = new Point(3, 140);
            cklbLogsLevels.Name = "cklbLogsLevels";
            cklbLogsLevels.Size = new Size(129, 76);
            cklbLogsLevels.TabIndex = 4;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(3, 222);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(129, 32);
            btnFilter.TabIndex = 5;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(138, 44);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(814, 431);
            dataGridView1.TabIndex = 6;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(915, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(37, 24);
            btnClose.TabIndex = 7;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // ctrlLogs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(btnClose);
            Controls.Add(dataGridView1);
            Controls.Add(btnFilter);
            Controls.Add(cklbLogsLevels);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dateTimePicker2);
            Controls.Add(dateTimePicker1);
            Name = "ctrlLogs";
            Size = new Size(955, 478);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private Label label1;
        private Label label2;
        private CheckedListBox cklbLogsLevels;
        private Button btnFilter;
        private DataGridView dataGridView1;
        private Button btnClose;
    }
}
