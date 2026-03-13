namespace UI.secondaryForms
{
    partial class uploadBillToPurchaseOrderForm
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
            label1 = new Label();
            txtOrderNumber = new TextBox();
            btnUploadBillFile = new Button();
            txtFilePath = new TextBox();
            label2 = new Label();
            txtTotalAmount = new TextBox();
            btnConfirm = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(101, 15);
            label1.TabIndex = 3;
            label1.Text = "Purchase Order #:";
            // 
            // txtOrderNumber
            // 
            txtOrderNumber.Location = new Point(12, 30);
            txtOrderNumber.Name = "txtOrderNumber";
            txtOrderNumber.Size = new Size(239, 23);
            txtOrderNumber.TabIndex = 2;
            // 
            // btnUploadBillFile
            // 
            btnUploadBillFile.Location = new Point(12, 76);
            btnUploadBillFile.Name = "btnUploadBillFile";
            btnUploadBillFile.Size = new Size(239, 32);
            btnUploadBillFile.TabIndex = 4;
            btnUploadBillFile.Text = "Browse and Upload Invoice File";
            btnUploadBillFile.UseVisualStyleBackColor = true;
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(12, 114);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.ReadOnly = true;
            txtFilePath.Size = new Size(238, 23);
            txtFilePath.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 168);
            label2.Name = "label2";
            label2.Size = new Size(154, 15);
            label2.TabIndex = 6;
            label2.Text = "Enter Total Invoice Amount:";
            // 
            // txtTotalAmount
            // 
            txtTotalAmount.Location = new Point(12, 186);
            txtTotalAmount.Name = "txtTotalAmount";
            txtTotalAmount.Size = new Size(239, 23);
            txtTotalAmount.TabIndex = 7;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(39, 215);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(185, 23);
            btnConfirm.TabIndex = 8;
            btnConfirm.Text = "Confirm and Close Order";
            btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(75, 244);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(113, 23);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // uploadBillToPurchaseOrderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(263, 274);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(txtTotalAmount);
            Controls.Add(label2);
            Controls.Add(txtFilePath);
            Controls.Add(btnUploadBillFile);
            Controls.Add(label1);
            Controls.Add(txtOrderNumber);
            Name = "uploadBillToPurchaseOrderForm";
            Text = "Invoice Recepcion";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtOrderNumber;
        private Button btnUploadBillFile;
        private TextBox txtFilePath;
        private Label label2;
        private TextBox txtTotalAmount;
        private Button btnConfirm;
        private Button btnCancel;
    }
}