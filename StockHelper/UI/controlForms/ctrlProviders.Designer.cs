namespace UI.controlForms
{
    partial class ctrlProviders
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
            btnClearFilter = new Button();
            btnFilter = new Button();
            lstbxCategories = new CheckedListBox();
            lbCategories = new Label();
            btnClose = new Button();
            dgvProviders = new DataGridView();
            txtProviderName = new TextBox();
            btnSearch = new Button();
            ProviderName = new DataGridViewTextBoxColumn();
            ProviderCompanyName = new DataGridViewTextBoxColumn();
            ProviderCategory = new DataGridViewTextBoxColumn();
            ProviderContactTel = new DataGridViewTextBoxColumn();
            ProviderEmail = new DataGridViewTextBoxColumn();
            btnCreateProvider = new Button();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProviders).BeginInit();
            SuspendLayout();
            // 
            // btnClearFilter
            // 
            btnClearFilter.Location = new Point(93, 349);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(90, 47);
            btnClearFilter.TabIndex = 10;
            btnClearFilter.Text = "Clear Filter";
            btnClearFilter.UseVisualStyleBackColor = true;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(3, 349);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(90, 47);
            btnFilter.TabIndex = 9;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // lstbxCategories
            // 
            lstbxCategories.FormattingEnabled = true;
            lstbxCategories.Location = new Point(3, 69);
            lstbxCategories.Name = "lstbxCategories";
            lstbxCategories.Size = new Size(180, 274);
            lstbxCategories.TabIndex = 8;
            lstbxCategories.TabStop = false;
            // 
            // lbCategories
            // 
            lbCategories.AutoSize = true;
            lbCategories.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbCategories.Location = new Point(3, 0);
            lbCategories.Name = "lbCategories";
            lbCategories.Size = new Size(115, 30);
            lbCategories.TabIndex = 7;
            lbCategories.Text = "Categories";
            // 
            // btnClose
            // 
            btnClose.Location = new Point(901, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(37, 24);
            btnClose.TabIndex = 11;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // dgvProviders
            // 
            dgvProviders.AllowUserToAddRows = false;
            dgvProviders.AllowUserToDeleteRows = false;
            dgvProviders.AllowUserToResizeColumns = false;
            dgvProviders.AllowUserToResizeRows = false;
            dgvProviders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProviders.Columns.AddRange(new DataGridViewColumn[] { ProviderName, ProviderCompanyName, ProviderCategory, ProviderContactTel, ProviderEmail });
            dgvProviders.Location = new Point(189, 33);
            dgvProviders.Name = "dgvProviders";
            dgvProviders.ReadOnly = true;
            dgvProviders.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvProviders.Size = new Size(743, 310);
            dgvProviders.TabIndex = 12;
            dgvProviders.TabStop = false;
            // 
            // txtProviderName
            // 
            txtProviderName.Location = new Point(3, 40);
            txtProviderName.Name = "txtProviderName";
            txtProviderName.PlaceholderText = "Provider Name";
            txtProviderName.Size = new Size(100, 23);
            txtProviderName.TabIndex = 13;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(109, 39);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 14;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // ProviderName
            // 
            ProviderName.HeaderText = "Name";
            ProviderName.Name = "ProviderName";
            ProviderName.ReadOnly = true;
            // 
            // ProviderCompanyName
            // 
            ProviderCompanyName.HeaderText = "Company Name";
            ProviderCompanyName.Name = "ProviderCompanyName";
            ProviderCompanyName.ReadOnly = true;
            ProviderCompanyName.Width = 200;
            // 
            // ProviderCategory
            // 
            ProviderCategory.HeaderText = "Category";
            ProviderCategory.Name = "ProviderCategory";
            ProviderCategory.ReadOnly = true;
            // 
            // ProviderContactTel
            // 
            ProviderContactTel.HeaderText = "Telephone Number";
            ProviderContactTel.Name = "ProviderContactTel";
            ProviderContactTel.ReadOnly = true;
            ProviderContactTel.Width = 150;
            // 
            // ProviderEmail
            // 
            ProviderEmail.HeaderText = "Email";
            ProviderEmail.Name = "ProviderEmail";
            ProviderEmail.ReadOnly = true;
            ProviderEmail.Width = 150;
            // 
            // btnCreateProvider
            // 
            btnCreateProvider.Location = new Point(314, 349);
            btnCreateProvider.Name = "btnCreateProvider";
            btnCreateProvider.Size = new Size(124, 47);
            btnCreateProvider.TabIndex = 15;
            btnCreateProvider.Text = "Create new Provider";
            btnCreateProvider.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(498, 349);
            button1.Name = "button1";
            button1.Size = new Size(124, 47);
            button1.TabIndex = 16;
            button1.Text = "Modify Provider";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(682, 349);
            button2.Name = "button2";
            button2.Size = new Size(124, 47);
            button2.TabIndex = 17;
            button2.Text = "Delete Provider";
            button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(189, 0);
            label1.Name = "label1";
            label1.Size = new Size(103, 30);
            label1.TabIndex = 18;
            label1.Text = "Providers";
            // 
            // ctrlProviders
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(btnCreateProvider);
            Controls.Add(btnSearch);
            Controls.Add(txtProviderName);
            Controls.Add(dgvProviders);
            Controls.Add(btnClose);
            Controls.Add(btnClearFilter);
            Controls.Add(btnFilter);
            Controls.Add(lstbxCategories);
            Controls.Add(lbCategories);
            Name = "ctrlProviders";
            Size = new Size(941, 402);
            ((System.ComponentModel.ISupportInitialize)dgvProviders).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnClearFilter;
        private Button btnFilter;
        private CheckedListBox lstbxCategories;
        private Label lbCategories;
        private Button btnClose;
        private DataGridView dgvProviders;
        private TextBox txtProviderName;
        private Button btnSearch;
        private DataGridViewTextBoxColumn ProviderName;
        private DataGridViewTextBoxColumn ProviderCompanyName;
        private DataGridViewTextBoxColumn ProviderCategory;
        private DataGridViewTextBoxColumn ProviderContactTel;
        private DataGridViewTextBoxColumn ProviderEmail;
        private Button btnCreateProvider;
        private Button button1;
        private Button button2;
        private Label label1;
    }
}
