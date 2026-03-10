namespace UI.secondaryForms
{
    partial class deleteProviderForm
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
            lstbxProviders = new ListBox();
            btnDelete = new Button();
            cmbChooseCategory = new ComboBox();
            SuspendLayout();
            // 
            // lstbxProviders
            // 
            lstbxProviders.FormattingEnabled = true;
            lstbxProviders.ItemHeight = 15;
            lstbxProviders.Location = new Point(12, 42);
            lstbxProviders.Name = "lstbxProviders";
            lstbxProviders.Size = new Size(201, 214);
            lstbxProviders.TabIndex = 0;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(75, 262);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 39);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // cmbChooseCategory
            // 
            cmbChooseCategory.FormattingEnabled = true;
            cmbChooseCategory.Location = new Point(12, 12);
            cmbChooseCategory.Name = "cmbChooseCategory";
            cmbChooseCategory.Size = new Size(201, 23);
            cmbChooseCategory.TabIndex = 2;
            cmbChooseCategory.SelectedIndexChanged += cmbChooseCategory_SelectedIndexChanged;
            // 
            // deleteProviderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(225, 310);
            Controls.Add(cmbChooseCategory);
            Controls.Add(btnDelete);
            Controls.Add(lstbxProviders);
            Name = "deleteProviderForm";
            Text = "Delete Provider";
            ResumeLayout(false);
        }

        #endregion

        private ListBox lstbxProviders;
        private Button btnDelete;
        private ComboBox cmbChooseCategory;
    }
}