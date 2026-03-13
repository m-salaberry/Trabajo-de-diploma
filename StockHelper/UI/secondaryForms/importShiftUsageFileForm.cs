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
    public partial class importShiftUsageFileForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        ItemService itemService = ItemService.Instance();
        List<Item> items;
        List<ItemsCategory> categories;
        string filePath;
        public importShiftUsageFileForm(List<Item> items, List<ItemsCategory> categories)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.items = items;
            this.categories = categories;
            InitialConfig();
        }

        private void LoadCategories()
        {
            cmbCategories.Items.Clear();
            cmbCategories.Items.Add(lang.Translate("All"));
            foreach (var category in categories)
            {
                cmbCategories.Items.Add(category.Name);
            }

        }

        private void btnBrowseAndLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                filePath = ofd.FileName;
                txtDirectory.Text = filePath;
                btnProcessFile.Enabled = true;
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("ErrorLoadingFile") + "\n\n" + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("UnexpectedError") + "\n\n" + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProcessFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show(lang.Translate("NoFileSelected"), lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                closeShiftProcessorService processor = new closeShiftProcessorService();
                List<(Item item, decimal quantity)> itemsToSubstract = processor.Process(filePath);

                dgvItemsAndStock.Rows.Clear();
                bool hasNegativeStock = false;

                foreach (var itemData in itemsToSubstract)
                {
                    Item item = items.FirstOrDefault(i => i.Id == itemData.item.Id);
                    if (item == null) continue;

                    decimal previousStock = item.Stock;
                    decimal subtractedStock = itemData.quantity;
                    decimal calculatedStock = previousStock - subtractedStock;

                    int idx = dgvItemsAndStock.Rows.Add(
                        item.Name,
                        item.Category?.Name ?? "",
                        item.Unit != null && item.Unit.ContainsKey("Name") ? item.Unit["Name"] : "",
                        previousStock,
                        subtractedStock,
                        calculatedStock
                    );

                    // Store item reference for safe retrieval when saving
                    dgvItemsAndStock.Rows[idx].Tag = item;

                    // Highlight rows with negative calculated stock
                    if (calculatedStock < 0)
                    {
                        dgvItemsAndStock.Rows[idx].DefaultCellStyle.BackColor = Color.MistyRose;
                        dgvItemsAndStock.Rows[idx].DefaultCellStyle.ForeColor = Color.DarkRed;
                        hasNegativeStock = true;
                    }
                }

                dgvItemsAndStock.Refresh();
                ItemName.ReadOnly = true;
                ItemCategory.ReadOnly = true;
                ItemUnit.ReadOnly = true;
                ItemSubtract.ReadOnly = true;
                ItemStock.ReadOnly = true;
                ItemNewStock.ReadOnly = false;
                LoadCategories();
                btnBrowseAndLoadFile.Enabled = false;
                btnProcessFile.Enabled = false;
                btnSaveNewStock.Enabled = true;

                if (hasNegativeStock)
                {
                    MessageBox.Show(
                        lang.Translate("NegativeStockWarning") ?? "Some items have insufficient stock. Rows highlighted in red will result in negative stock. Please review before saving.",
                        lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Logger.Current.Info($"[IMPORT] Shift usage file processed: '{filePath}', {dgvItemsAndStock.Rows.Count} items affected.");
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("ErrorProcessingFile") + "\n\n" + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("UnexpectedError") + "\n\n" + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitialConfig()
        {
            btnProcessFile.Enabled = false;
            btnSaveNewStock.Enabled = false;
            btnCancel.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("ErrorClosingForm") + "\n\n" + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("UnexpectedError") + "\n\n" + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveNewStock_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate all rows before saving
                List<(Item item, decimal newStock)> updatedStocks = new List<(Item item, decimal newStock)>();
                foreach (DataGridViewRow row in dgvItemsAndStock.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Use Tag to get the item reference safely (instead of searching by name)
                    if (row.Tag is not Item item) continue;

                    if (!decimal.TryParse(row.Cells["ItemNewStock"].Value?.ToString(), out decimal newStock))
                    {
                        MessageBox.Show(
                            $"Invalid stock value for '{item.Name}'. Must be a valid number.",
                            lang.Translate("ValidationError") ?? "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (newStock < 0)
                    {
                        MessageBox.Show(
                            $"New stock for '{item.Name}' cannot be negative ({newStock}).",
                            lang.Translate("ValidationError") ?? "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Validate integer unit type
                    if (item.IsUnitInteger() && newStock != Math.Floor(newStock))
                    {
                        MessageBox.Show(
                            $"'{item.Name}' uses integer units. Decimal values are not allowed.",
                            lang.Translate("ValidationError") ?? "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    updatedStocks.Add((item, newStock));
                }

                if (updatedStocks.Count == 0)
                {
                    MessageBox.Show(lang.Translate("NoItemsToUpdate") ?? "No items to update.",
                        lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirmation before bulk save
                var confirmResult = MessageBox.Show(
                    string.Format(lang.Translate("ConfirmImportStockChanges") ?? "Are you sure you want to update the stock for {0} item(s)?", updatedStocks.Count),
                    lang.Translate("Confirmation") ?? "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes) return;

                foreach (var stockUpdate in updatedStocks)
                {
                    itemService.ReduceStock(stockUpdate.item.Id, stockUpdate.newStock);
                }

                Logger.Current.Info($"[IMPORT] Stock updated from shift file '{filePath}': {updatedStocks.Count} items modified.");

                MessageBox.Show(lang.Translate("StockUpdatedSuccessfully"), lang.Translate("Success"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("ErrorUpdatingStock") + "\n\n" + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("UnexpectedError") + "\n\n" + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategories.SelectedIndex != -1)
            {
                string selectedCategory = cmbCategories.SelectedItem.ToString();
                if (selectedCategory == lang.Translate("All"))
                {
                    foreach (DataGridViewRow row in dgvItemsAndStock.Rows)
                    {
                        row.Visible = true;
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in dgvItemsAndStock.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string category = row.Cells[1].Value?.ToString() ?? "";
                        row.Visible = category == selectedCategory;
                    }
                }
            }
        }
    }
}
