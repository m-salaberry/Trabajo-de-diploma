namespace UI.secondaryForms
{
    partial class chooseProviderForm
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
            btnSelectProvider = new Button();
            SuspendLayout();
            // 
            // lstbxProviders
            // 
            lstbxProviders.FormattingEnabled = true;
            lstbxProviders.ItemHeight = 15;
            lstbxProviders.Location = new Point(12, 12);
            lstbxProviders.Name = "lstbxProviders";
            lstbxProviders.Size = new Size(198, 214);
            lstbxProviders.TabIndex = 0;
            // 
            // btnSelectProvider
            // 
            btnSelectProvider.Location = new Point(12, 232);
            btnSelectProvider.Name = "btnSelectProvider";
            btnSelectProvider.Size = new Size(198, 49);
            btnSelectProvider.TabIndex = 1;
            btnSelectProvider.Text = "Select";
            btnSelectProvider.UseVisualStyleBackColor = true;
            btnSelectProvider.Click += btnSelectProvider_Click;
            // 
            // chooseProviderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(223, 293);
            Controls.Add(btnSelectProvider);
            Controls.Add(lstbxProviders);
            Name = "chooseProviderForm";
            Text = "Choose a Provider";
            ResumeLayout(false);
        }

        #endregion

        private ListBox lstbxProviders;
        private Button btnSelectProvider;
    }
}