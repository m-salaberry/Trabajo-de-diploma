namespace UI.controlForms
{
    partial class ctrlConfiguration
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
            label1 = new Label();
            cmbLangauge = new ComboBox();
            btnSaveConfig = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(29, 17);
            label1.Name = "label1";
            label1.Size = new Size(136, 20);
            label1.TabIndex = 0;
            label1.Text = "System Language:";
            // 
            // cmbLangauge
            // 
            cmbLangauge.FormattingEnabled = true;
            cmbLangauge.Location = new Point(3, 40);
            cmbLangauge.Name = "cmbLangauge";
            cmbLangauge.Size = new Size(188, 23);
            cmbLangauge.TabIndex = 1;
            // 
            // btnSaveConfig
            // 
            btnSaveConfig.Location = new Point(60, 84);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.Size = new Size(75, 23);
            btnSaveConfig.TabIndex = 2;
            btnSaveConfig.Text = "Save";
            btnSaveConfig.UseVisualStyleBackColor = true;
            btnSaveConfig.Click += btnSaveConfig_Click;
            // 
            // ctrlConfiguration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnSaveConfig);
            Controls.Add(cmbLangauge);
            Controls.Add(label1);
            Name = "ctrlConfiguration";
            Size = new Size(194, 117);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbLangauge;
        private Button btnSaveConfig;
    }
}
