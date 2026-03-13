using BLL.Implementations;
using Domain;
using Services.Contracts.CustomsException;
using Services.Contracts.Logs;
using Services.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Implementations;

namespace UI.secondaryForms
{
    public partial class uploadBillToPurchaseOrderForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        PurchaseOrderService purchaseOrderService = PurchaseOrderService.Instance();

        private PurchaseOrder _purchaseOrder;

        public event EventHandler InvoiceUploaded;

        /// <summary>
        /// Initializes the invoice upload form for the specified purchase order.
        /// </summary>
        public uploadBillToPurchaseOrderForm(PurchaseOrder purchaseOrder)
        {
            InitializeComponent();
            this.CenterToScreen();
            _purchaseOrder = purchaseOrder;
            txtOrderNumber.Text = purchaseOrder.ReplacementOrder.ReplacementOrderNumber;
            txtOrderNumber.ReadOnly = true;

            btnUploadBillFile.Click += btnUploadBillFile_Click;
            btnConfirm.Click += btnConfirm_Click;
            btnCancel.Click += btnCancel_Click;
            txtTotalAmount.KeyPress += txtTotalAmount_KeyPress;
        }

        /// <summary>
        /// Applies translations to form controls.
        /// </summary>
        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("Invoice Reception");
            label1.Text = lang.Translate("Purchase Order #:");
            label2.Text = lang.Translate("Enter Total Invoice Amount:");
            btnUploadBillFile.Text = lang.Translate("Browse and Upload Invoice File");
            btnConfirm.Text = lang.Translate("Confirm and Close Order");
            btnCancel.Text = lang.Translate("Cancel");
        }

        /// <summary>
        /// Restricts input to numeric characters and a single decimal separator.
        /// </summary>
        private void txtTotalAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)
                && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.' || e.KeyChar == ',') && txtTotalAmount.Text.IndexOfAny(new[] { '.', ',' }) >= 0)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Opens a file dialog for selecting an invoice file.
        /// </summary>
        private void btnUploadBillFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Invoice files (*.pdf;*.png;*.jpg;*.jpeg)|*.pdf;*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
                openFileDialog.Title = lang.Translate("Select Invoice File");

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// Validates input and processes the invoice upload to close the purchase order.
        /// </summary>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFilePath.Text))
                {
                    MessageBox.Show(
                        lang.Translate("Please select an invoice file before confirming."),
                        lang.Translate("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtTotalAmount.Text.Replace(',', '.'),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal totalAmount) || totalAmount <= 0)
                {
                    MessageBox.Show(
                        lang.Translate("Please enter a valid total amount greater than zero."),
                        lang.Translate("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    lang.Translate("Are you sure you want to confirm this invoice and close the order?"),
                    lang.Translate("Confirm"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    purchaseOrderService.ReceiveOrder(_purchaseOrder.Id, txtFilePath.Text, totalAmount);
                    MessageBox.Show(
                        lang.Translate("Invoice uploaded and order closed successfully."),
                        lang.Translate("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    InvoiceUploaded?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error uploading invoice: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Closes the form without saving.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
