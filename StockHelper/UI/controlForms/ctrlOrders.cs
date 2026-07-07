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

        /// <summary>
        /// Initializes the orders control, wires up the list formatting and provider filter events,
        /// loads the existing orders and providers, and applies the initial read-only configuration.
        /// </summary>
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

        /// <summary>
        /// Applies the current language translations to all labels, buttons, group boxes and grid
        /// column headers of the control, then refreshes the orders grid.
        /// </summary>
        public override void ApplyTranslations()
        {
            label1.Text = lang.Translate("Replacement Orders:");
            btnCreateNewReplacementOrder.Text = lang.Translate("Create new Replacement Order");
            btnModifyReplacementOrder.Text = lang.Translate("Modify Replacement Order");
            btnDeleteOrder.Text = lang.Translate("Delete Replacement Order");
            gpbxOrder.Text = lang.Translate("Order Details:");
            btnCancel.Text = lang.Translate("Cancel");
            btnSave.Text = lang.Translate("Save Order");
            btnSendOrder.Text = lang.Translate("Send Order to Provider");

            if (dgvOrders.Columns["ItemName"] != null)
                dgvOrders.Columns["ItemName"].HeaderText = lang.Translate("Item Name");
            if (dgvOrders.Columns["ItemUnit"] != null)
                dgvOrders.Columns["ItemUnit"].HeaderText = lang.Translate("Unit");
            if (dgvOrders.Columns["ItemQuantity"] != null)
                dgvOrders.Columns["ItemQuantity"].HeaderText = lang.Translate("Quantity");

            dgvOrders.Refresh();
        }

        /// <summary>
        /// Handles the orders list box formatting: displays each replacement order as its order
        /// number padded into a column alongside the provider name (or "N/A" when none).
        /// </summary>
        private void LstbxOrders_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is ReplacementOrder order)
            {
                string providerName = order.Provider != null ? order.Provider.Name : "N/A";
                e.Value = $"{order.ReplacementOrderNumber,-20} | {providerName}";
            }
        }

        /// <summary>
        /// Loads all replacement orders that have not yet been turned into a purchase order and
        /// applies the current filters to refresh the displayed list.
        /// </summary>
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
        /// <summary>
        /// Loads all providers from the provider service into the local providers list.
        /// </summary>
        private void LoadProviders()
        {
            providers = providerService.GetAll().ToList();
        }

        /// <summary>
        /// Filters the orders by the provider filter text (matching provider name or order number)
        /// and renders the resulting list.
        /// </summary>
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

        /// <summary>
        /// Clears the orders list box and repopulates it with the given orders.
        /// </summary>
        /// <param name="source">The orders to display in the list box.</param>
        private void RenderOrders(List<ReplacementOrder> source)
        {
            lstbxOrders.Items.Clear();
            foreach (var order in source)
            {
                lstbxOrders.Items.Add(order);
            }
        }

        /// <summary>
        /// Handles the provider filter text change: re-applies the filters to the orders list.
        /// </summary>
        private void txtFilterByProvider_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Handles selection changes in the orders list: shows the selected order's number and
        /// renders its details, or clears the details when nothing is selected.
        /// </summary>
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

        /// <summary>
        /// Populates the details grid with the given order's rows, showing each item's name, unit
        /// and quantity, and tags each grid row with its source order row.
        /// </summary>
        /// <param name="order">The order whose rows are displayed in the grid.</param>
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

        /// <summary>
        /// Applies the initial control state: makes the grid read-only and disables the save button.
        /// </summary>
        private void InitialConfig()
        {
            dgvOrders.ReadOnly = true;
            btnSave.Enabled = false;
        }

        /// <summary>
        /// Switches the control into edit mode: makes quantities editable, enables saving and locks
        /// the order list and create button so switching orders cannot discard the current one.
        /// </summary>
        private void EnterEditMode()
        {
            dgvOrders.ReadOnly = false;
            ItemName.ReadOnly = true;
            ItemQuantity.ReadOnly = false;
            btnModifyReplacementOrder.Enabled = false;
            btnSave.Enabled = true;
            lstbxOrders.Enabled = false;
            btnCreateNewReplacementOrder.Enabled = false;
        }

        /// <summary>
        /// Leaves edit mode: makes the grid read-only again and re-enables the modify button, order
        /// list and create button.
        /// </summary>
        private void ExitEditMode()
        {
            dgvOrders.ReadOnly = true;
            btnModifyReplacementOrder.Enabled = true;
            btnSave.Enabled = false;
            lstbxOrders.Enabled = true;
            btnCreateNewReplacementOrder.Enabled = true;
        }

        /// <summary>
        /// Handles the Modify button click: sets the selected order as the current one and enters
        /// edit mode, or warns the user when no order is selected.
        /// </summary>
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

        /// <summary>
        /// Handles the Save button click: validates the grid quantities and either inserts the new
        /// order or updates the current one, then reloads the orders and exits edit mode.
        /// </summary>
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (newReplacementOrder != null)
                {
                    var rows = new List<OrderRow>();
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
                            rows.Add(new OrderRow { Item = item, Quantity = quantity });
                        }
                    }
                    newReplacementOrder.OrderRows.Clear();
                    newReplacementOrder.OrderRows.AddRange(rows);
                    replacementOrderService.Insert(newReplacementOrder);
                    newReplacementOrder = null;
                }
                else if (currentReplacementOrder != null)
                {
                    var pending = new List<(OrderRow row, decimal quantity)>();
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
                        pending.Add((orderRow, newQuantity));
                    }
                    currentReplacementOrder.OrderRows.Clear();
                    foreach (var (orderRow, quantity) in pending)
                    {
                        orderRow.Quantity = quantity;
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

        /// <summary>
        /// Handles the Create button click: opens the provider chooser dialog and, once a provider
        /// is selected, starts a new replacement order, loads the provider's items and enters edit mode.
        /// </summary>
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

        /// <summary>
        /// Fills the details grid with all items belonging to the provider's category, each starting
        /// at quantity zero and tagged with its source item.
        /// </summary>
        /// <param name="provider">The provider whose category determines which items are shown.</param>
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

        /// <summary>
        /// Handles the Cancel button click: discards any new or in-progress order, exits edit mode
        /// and restores the details of the previously selected order (or clears them).
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            newReplacementOrder = null;
            currentReplacementOrder = null;
            ExitEditMode();

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

        /// <summary>
        /// Handles the Delete button click: after user confirmation, deletes the selected order and
        /// reloads the list, or warns the user when no order is selected.
        /// </summary>
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

        /// <summary>
        /// Handles the Send button click: after user confirmation, creates a purchase order from the
        /// selected replacement order, sends a WhatsApp message to the provider and reloads the list.
        /// </summary>
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
