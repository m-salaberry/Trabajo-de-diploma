namespace UI.secondaryForms
{
    partial class deleteCategoryForm
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
            lstbxCategoriesToDelete = new ListBox();
            label1 = new Label();
            btnDelete = new Button();
            SuspendLayout();
            // 
            // lstbxCategoriesToDelete
            // 
            lstbxCategoriesToDelete.FormattingEnabled = true;
            lstbxCategoriesToDelete.ItemHeight = 15;
            lstbxCategoriesToDelete.Location = new Point(12, 42);
            lstbxCategoriesToDelete.Name = "lstbxCategoriesToDelete";
            lstbxCategoriesToDelete.Size = new Size(188, 169);
            lstbxCategoriesToDelete.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(41, 9);
            label1.Name = "label1";
            label1.Size = new Size(130, 21);
            label1.TabIndex = 0;
            label1.Text = "Delete Category";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(12, 217);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(188, 23);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // deleteCategoryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(212, 247);
            Controls.Add(btnDelete);
            Controls.Add(label1);
            Controls.Add(lstbxCategoriesToDelete);
            Name = "deleteCategoryForm";
            Text = "Delete Category";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lstbxCategoriesToDelete;
        private Label label1;
        private Button btnDelete;
    }
}