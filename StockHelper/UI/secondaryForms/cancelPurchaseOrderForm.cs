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
    public partial class cancelPurchaseOrderForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        PurchaseOrderService purchaseOrderService = PurchaseOrderService.Instance();

        private PurchaseOrder _purchaseOrder;

        public event EventHandler OrderCancelled;

        /// <summary>
        /// Initializes the cancellation form for the specified purchase order.
        /// </summary>
        public cancelPurchaseOrderForm(PurchaseOrder purchaseOrder)
        {
            InitializeComponent();
            this.CenterToScreen();
            _purchaseOrder = purchaseOrder;
            textBox1.Text = purchaseOrder.ReplacementOrder.ReplacementOrderNumber;
            textBox1.ReadOnly = true;
        }

        /// <summary>
        /// Applies translations to form controls.
        /// </summary>
        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("Cancel Purchase Order");
            label1.Text = lang.Translate("Purchase Order #:");
            btnCancel.Text = lang.Translate("Cancel Order");
        }

        /// <summary>
        /// Handles the cancel button click to cancel the purchase order after confirmation.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show(
                    lang.Translate("Are you sure you want to cancel this purchase order?"),
                    lang.Translate("Confirm Cancel"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    purchaseOrderService.CancelOrder(_purchaseOrder.Id);
                    MessageBox.Show(
                        lang.Translate("Purchase order cancelled successfully."),
                        lang.Translate("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    OrderCancelled?.Invoke(this, EventArgs.Empty);
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
                Logger.Current.Error($"Error cancelling purchase order: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
