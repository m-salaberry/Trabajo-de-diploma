using BLL.Implementations;
using BLL.Templates;
using Domain;
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
    public partial class ctrlOrders : TranslatableUserControls
    {
        LanguageService lang = LanguageService.GetInstance;
        ReplacementOrderService replacementOrderService = ReplacementOrderService.Instance();
        PurchaseOrderService purchaseOrderService = PurchaseOrderService.Instance();
        ProviderService providerService = ProviderService.Instance();
        ItemService itemService = ItemService.Instance();

        List<ReplacementOrder> orders;
        List<ReplacementOrder> filteredOrders;
        List<Provider> providers;

        ReplacementOrder newReplacementOrder;
        ReplacementOrder currentReplacementOrder;

        public ctrlOrders()
        {
            InitializeComponent();

            lstbxOrders.FormattingEnabled = true;
            lstbxOrders.Format += LstbxOrders_Format;

            LoadOrders();
            LoadProviders();
            txtFilterByProvider.TextChanged += txtFilterByProvider_TextChanged;
            InitialConfig();
        }

        private void LstbxOrders_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is ReplacementOrder order)
            {
                string providerName = order.Provider != null ? order.Provider.Name : "N/A";
                // Alinea el número de orden a la izquierda usando padding simulando columnas
                e.Value = $"{order.ReplacementOrderNumber,-20} | {providerName}";
            }
        }

        private void LoadOrders()
        {
            var allPurchaseOrderReplacementIds = purchaseOrderService.GetAll()
                .Select(po => po.ReplacementOrder.Id)
                .ToHashSet();

            orders = replacementOrderService.GetAll()
                .Where(ro => !allPurchaseOrderReplacementIds.Contains(ro.Id))
                .ToList();

            filteredOrders = orders;
            ApplyFilters();
        }
        private void LoadProviders()
        {
            providers = providerService.GetAll().ToList();
        }

        private void ApplyFilters()
        {
            var result = orders.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtFilterByProvider.Text))
            {
                string searchTerm = txtFilterByProvider.Text.ToLower();
                result = result.Where(o =>
                    o.Provider.Name.ToLower().Contains(searchTerm) ||
                    o.ReplacementOrderNumber.ToLower().Contains(searchTerm));
            }

            filteredOrders = result.ToList();
            RenderOrders(filteredOrders);
        }

        private void RenderOrders(List<ReplacementOrder> source)
        {
            lstbxOrders.Items.Clear();
            foreach (var order in source)
            {
                lstbxOrders.Items.Add(order);
            }
        }

        private void txtFilterByProvider_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void lstbxOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstbxOrders.SelectedItem is ReplacementOrder selectedOrder)
            {
                lbOrder.Text = $"Order: {selectedOrder.ReplacementOrderNumber}";
                RenderOrderDetails(selectedOrder);
            }
            else
            {
                lbOrder.Text = "Order: -";
                dgvOrders.Rows.Clear();
            }
        }

        private void RenderOrderDetails(ReplacementOrder order)
        {
            dgvOrders.Rows.Clear();
            foreach (var row in order.OrderRows)
            {
                string unitName = row.Item.Unit != null && row.Item.Unit.ContainsKey("Name")
                    ? row.Item.Unit["Name"]?.ToString() : "";
                int idx = dgvOrders.Rows.Add(row.Item.Name, unitName, row.Quantity);
                dgvOrders.Rows[idx].Tag = row;
            }
        }

        private void InitialConfig()
        {
            dgvOrders.ReadOnly = true;
            btnSave.Enabled = false;
        }

        private void EnterEditMode()
        {
            dgvOrders.ReadOnly = false;
            ItemName.ReadOnly = true;
            ItemQuantity.ReadOnly = false;
            btnModifyReplacementOrder.Enabled = false;
            btnSave.Enabled = true;
        }

        private void ExitEditMode()
        {
            dgvOrders.ReadOnly = true;
            btnModifyReplacementOrder.Enabled = true;
            btnSave.Enabled = false;
        }

        private void btnModifyReplacementOrder_Click(object sender, EventArgs e)
        {
            if (lstbxOrders.SelectedItem is ReplacementOrder selectedOrder)
            {
                currentReplacementOrder = selectedOrder;
                EnterEditMode();
            }
            else
            {
                MessageBox.Show(lang.Translate("Please select an order to modify."),
                    lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (newReplacementOrder != null)
                {
                    foreach (DataGridViewRow row in dgvOrders.Rows)
                    {
                        if (row.Tag is not Item item) continue;
                        if (!decimal.TryParse(row.Cells["ItemQuantity"].Value?.ToString(), out decimal quantity))
                        {
                            MessageBox.Show(
                                $"{lang.Translate("Invalid quantity for")} '{item.Name}'.",
                                lang.Translate("Validation Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (quantity > 0)
                        {
                            newReplacementOrder.OrderRows.Add(new OrderRow { Item = item, Quantity = quantity });
                        }
                    }
                    replacementOrderService.Insert(newReplacementOrder);
                    newReplacementOrder = null;
                }
                else if (currentReplacementOrder != null)
                {
                    currentReplacementOrder.OrderRows.Clear();
                    foreach (DataGridViewRow row in dgvOrders.Rows)
                    {
                        if (row.Tag is not OrderRow orderRow) continue;
                        if (!decimal.TryParse(row.Cells["ItemQuantity"].Value?.ToString(), out decimal newQuantity) || newQuantity <= 0)
                        {
                            MessageBox.Show(
                                $"{lang.Translate("Invalid quantity for")} '{orderRow.Item.Name}'.",
                                lang.Translate("Validation Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        orderRow.Quantity = newQuantity;
                        currentReplacementOrder.OrderRows.Add(orderRow);
                    }
                    replacementOrderService.Update(currentReplacementOrder);
                    currentReplacementOrder = null;
                }

                LoadOrders();
                ExitEditMode();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateNewReplacementOrder_Click(object sender, EventArgs e)
        {
            try
            {
                chooseProviderForm providerForm = new chooseProviderForm(providers);
                providerForm.OnProviderSelected += (s, selectedProvider) =>
                {
                    newReplacementOrder = new ReplacementOrder(selectedProvider);
                    lbOrder.Text = $"Order: {lang.Translate("New")} - {selectedProvider.Name}";
                    LoadItemsByProviderCategory(selectedProvider);
                    EnterEditMode();
                };
                providerForm.ShowDialog();
                providerForm.Dispose();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadItemsByProviderCategory(Provider provider)
        {
            var items = itemService.GetAll()
                .Where(i => i.Category != null && i.Category.Id == provider.Category.Id)
                .ToList();

            dgvOrders.Rows.Clear();
            foreach (var item in items)
            {
                string unitName = item.Unit != null && item.Unit.ContainsKey("Name")
                    ? item.Unit["Name"]?.ToString() : "";
                int idx = dgvOrders.Rows.Add(item.Name, unitName, "0");
                dgvOrders.Rows[idx].Tag = item;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            newReplacementOrder = null;
            currentReplacementOrder = null;
            ExitEditMode();

            // Restore previous selection or clear
            if (lstbxOrders.SelectedItem is ReplacementOrder selectedOrder)
            {
                RenderOrderDetails(selectedOrder);
            }
            else
            {
                lbOrder.Text = "Order: -";
                dgvOrders.Rows.Clear();
            }
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                currentReplacementOrder = lstbxOrders.SelectedItem as ReplacementOrder;
                if (currentReplacementOrder != null)
                {
                    var confirmResult = MessageBox.Show(
                        lang.Translate("Are you sure you want to delete this order?"),
                        lang.Translate("Confirm Delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmResult == DialogResult.Yes)
                    {
                        replacementOrderService.Delete(currentReplacementOrder.Id);
                        LoadOrders();
                        lbOrder.Text = "Order: -";
                        dgvOrders.Rows.Clear();
                    }
                }
                else
                {
                    MessageBox.Show(lang.Translate("Please select an order to delete."),
                        lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSendOrder_Click(object sender, EventArgs e)
        {
            try
            {
                currentReplacementOrder = lstbxOrders.SelectedItem as ReplacementOrder;
                if (currentReplacementOrder != null)
                {
                    var confirmResult = MessageBox.Show(
                        lang.Translate("Are you sure you want to send this order?"),
                        lang.Translate("Confirm Send"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmResult == DialogResult.Yes)
                    {
                        PurchaseOrder newPurchaseOrder = new PurchaseOrder();
                        newPurchaseOrder.ReplacementOrder = currentReplacementOrder;
                        newPurchaseOrder.TotalAmount = 0;
                        purchaseOrderService.Insert(newPurchaseOrder);

                        // Send WhatsApp message to provider
                        string message = WhatsAppMessageTemplates.BuildOrderMessage(currentReplacementOrder, lang);
                        string phone = currentReplacementOrder.Provider.ContactTel;
                        var whatsApp = new WhatsAppMessengerService(phone, message);
                        whatsApp.SendMessage();

                        MessageBox.Show(lang.Translate("Order sent successfully!"),
                            lang.Translate("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadOrders();
                        lbOrder.Text = "Order: -";
                        dgvOrders.Rows.Clear();
                    }
                }
                else
                {
                    MessageBox.Show(lang.Translate("Please select an order to send."),
                        lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
