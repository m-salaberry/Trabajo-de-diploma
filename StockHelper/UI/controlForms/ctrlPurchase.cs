using BLL.Implementations;
using Domain;
using Domain.Enums;
using Services.Contracts.CustomsException;
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
using UI.secondaryForms;

namespace UI.controlForms
{
    public partial class ctrlPurchase : TranslatableUserControls
    {
        LanguageService lang = LanguageService.GetInstance;
        PurchaseOrderService purchaseOrderService = PurchaseOrderService.Instance();

        List<PurchaseOrder> purchaseOrders;
        List<PurchaseOrder> filteredOrders;

        public ctrlPurchase()
        {
            InitializeComponent();
            InitFilters();
            LoadPurchaseOrders();
            btnCancelOrder.Click += btnCancelOrder_Click;
            btnUploadInvoice.Click += btnUploadInvoice_Click;
            txtFilterByProvider.TextChanged += txtFilterByProvider_TextChanged;
            cmbFilterStatus.SelectedIndexChanged += cmbFilterStatus_SelectedIndexChanged;
        }

        private void InitFilters()
        {
            cmbFilterStatus.Items.Clear();
            cmbFilterStatus.Items.Add("All");
            cmbFilterStatus.Items.Add(PurchaseOrderStatus.SentToProvider);
            cmbFilterStatus.Items.Add(PurchaseOrderStatus.Cancelled);
            cmbFilterStatus.Items.Add(PurchaseOrderStatus.BillReceived);
            cmbFilterStatus.SelectedIndex = 0;
            cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public override void ApplyTranslations()
        {
            label1.Text = lang.Translate("Purchase Orders:");
            btnCancelOrder.Text = lang.Translate("Cancel Order");
            btnUploadInvoice.Text = lang.Translate("Upload Invoice");
            txtFilterByProvider.PlaceholderText = lang.Translate("Search by Provider...");

            OrderName.HeaderText = lang.Translate("Order Name");
            OrderProvider.HeaderText = lang.Translate("Provider");
            OrderCategory.HeaderText = lang.Translate("Category");
            OrderStatus.HeaderText = lang.Translate("Status");
            OrderAmount.HeaderText = lang.Translate("Total Amount");
        }

        private void LoadPurchaseOrders()
        {
            purchaseOrders = purchaseOrderService.GetAll().ToList();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var result = purchaseOrders.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtFilterByProvider.Text))
            {
                string searchTerm = txtFilterByProvider.Text.ToLower();
                result = result.Where(po =>
                    po.ReplacementOrder.Provider != null &&
                    po.ReplacementOrder.Provider.Name.ToLower().Contains(searchTerm));
            }

            if (cmbFilterStatus.SelectedIndex > 0)
            {
                string selectedStatus = cmbFilterStatus.SelectedItem.ToString();
                result = result.Where(po => po.Status == selectedStatus);
            }

            filteredOrders = result.ToList();
            RenderPurchaseOrders();
        }

        private void RenderPurchaseOrders()
        {
            dgvPurchaseOrders.Rows.Clear();
            foreach (var po in filteredOrders)
            {
                string orderNumber = po.ReplacementOrder?.ReplacementOrderNumber ?? "N/A";
                string providerName = po.ReplacementOrder?.Provider?.Name ?? "N/A";
                string categoryName = po.ReplacementOrder?.Provider?.Category?.Name ?? "N/A";
                string status = po.Status ?? "N/A";
                string amount = po.TotalAmount.ToString("N2");

                int idx = dgvPurchaseOrders.Rows.Add(orderNumber, providerName, categoryName, status, amount);
                dgvPurchaseOrders.Rows[idx].Tag = po;
            }
        }

        private void txtFilterByProvider_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    lang.Translate("Please select a purchase order to cancel."),
                    lang.Translate("Warning"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            PurchaseOrder selectedPO = dgvPurchaseOrders.SelectedRows[0].Tag as PurchaseOrder;
            if (selectedPO == null) return;

            cancelPurchaseOrderForm cancelForm = new cancelPurchaseOrderForm(selectedPO);
            cancelForm.OrderCancelled += (s, args) => LoadPurchaseOrders();
            cancelForm.ShowDialog();
            cancelForm.Dispose();
        }

        private void btnUploadInvoice_Click(object sender, EventArgs e)
        {
            if (dgvPurchaseOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    lang.Translate("Please select a purchase order to upload an invoice."),
                    lang.Translate("Warning"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            PurchaseOrder selectedPO = dgvPurchaseOrders.SelectedRows[0].Tag as PurchaseOrder;
            if (selectedPO == null) return;

            uploadBillToPurchaseOrderForm uploadForm = new uploadBillToPurchaseOrderForm(selectedPO);
            uploadForm.InvoiceUploaded += (s, args) => LoadPurchaseOrders();
            uploadForm.ShowDialog();
            uploadForm.Dispose();
        }
    }
}
