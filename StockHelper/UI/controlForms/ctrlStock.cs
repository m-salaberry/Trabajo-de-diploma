using BLL.Implementations;
using Services.Implementations;
using Services.Contracts.Logs;
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
using Domain;
using Services.Contracts.CustomsException;
using UI.secondaryForms;

namespace UI.controlForms
{
    public partial class ctrlStock : TranslatableUserControls
    {
        LanguageService lang = LanguageService.GetInstance;
        ItemService itemService = ItemService.Instance();
        ItemsCategoryService itemsCategoryService = ItemsCategoryService.Instance();

        List<Item> items;
        List<ItemsCategory> categories;

        List<Item> filteredItems;
        public ctrlStock()
        {
            InitializeComponent();
            LoadData();
            LoadCategories();
            LoadItems();
            InitialConfig();
        }

        private void LoadData()
        {
            items = itemService.GetAll().ToList();
            categories = itemsCategoryService.GetAll().ToList();
            filteredItems = items;
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

        private void LoadItems()
        {
            filteredItems = items;
            ApplyFilters();
        }

        /// <summary>
        /// Applies category and search filters together to keep them coordinated.
        /// </summary>
        private void ApplyFilters()
        {
            var result = items.AsEnumerable();

            // Apply category filter
            if (cmbCategories.SelectedIndex > 0)
            {
                string selectedCategory = cmbCategories.SelectedItem.ToString();
                result = result.Where(i => i.Category.Name == selectedCategory);
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(txtSearchItem.Text))
            {
                string searchTerm = txtSearchItem.Text.ToLower();
                result = result.Where(i => i.Name.ToLower().Contains(searchTerm));
            }

            filteredItems = result.ToList();
            RenderItems(filteredItems);
        }

        private void RenderItems(List<Item> source)
        {
            dgvItemsAndStock.Rows.Clear();
            foreach (var item in source)
            {
                int idx = dgvItemsAndStock.Rows.Add(item.Name, item.Category.Name, item.Unit["Name"], item.Stock, item.LastUpdate);
                dgvItemsAndStock.Rows[idx].Tag = item;
            }
            dgvItemsAndStock.Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            frmMain.GetInstance().ResetMainPanelSize();
            this.Dispose();
        }

        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategories.SelectedIndex != -1)
            {
                ApplyFilters();
            }
        }

        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void InitialConfig()
        {
            btnCancelEdit.Enabled = false;
            btnSaveEdit.Enabled = false;
            dgvItemsAndStock.ReadOnly = true;
        }

        private void EnterEditMode()
        {
            dgvItemsAndStock.ReadOnly = false;
            ItemName.ReadOnly = true;
            ItemCategory.ReadOnly = true;
            ItemUnit.ReadOnly = true;
            ItemStock.ReadOnly = false;
            ItemUpdatedDate.ReadOnly = true;
            btnEditMode.Enabled = false;
            btnSaveEdit.Enabled = true;
            btnCancelEdit.Enabled = true;
            btnImportShiftUsage.Enabled = false;
        }

        private void ExitEditMode()
        {
            ItemStock.ReadOnly = true;
            dgvItemsAndStock.ReadOnly = true;
            btnEditMode.Enabled = true;
            btnSaveEdit.Enabled = false;
            btnCancelEdit.Enabled = false;
            btnImportShiftUsage.Enabled = true;
        }

        private void btnEditMode_Click(object sender, EventArgs e)
        {
            EnterEditMode();
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            LoadData();
            LoadItems();
            ExitEditMode();
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate all rows before saving
                foreach (DataGridViewRow row in dgvItemsAndStock.Rows)
                {
                    if (row.Tag is not Item item) continue;

                    if (!decimal.TryParse(row.Cells["ItemStock"].Value?.ToString(), out decimal newStock) || newStock < 0)
                    {
                        MessageBox.Show(
                            $"Invalid stock value for '{item.Name}'. Must be a non-negative number.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Validate integer unit type
                    if (item.IsUnitInteger() && newStock != Math.Floor(newStock))
                    {
                        MessageBox.Show(
                            $"'{item.Name}' uses integer units. Decimal values are not allowed.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Confirmation before bulk save
                var confirmResult = MessageBox.Show(
                    lang.Translate("ConfirmSaveStockChanges") ?? "Are you sure you want to save the stock changes?",
                    lang.Translate("Confirmation") ?? "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes) return;

                foreach (DataGridViewRow row in dgvItemsAndStock.Rows)
                {
                    if (row.Tag is not Item item) continue;

                    decimal newStock = decimal.Parse(row.Cells["ItemStock"].Value.ToString());

                    if (newStock != item.Stock)
                    {
                        item.Stock = newStock;
                        itemService.Update(item);
                    }
                }

                LoadData();
                LoadItems();
                ExitEditMode();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("ErrorSavingChanges") + "\n\n" + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("UnexpectedError") + "\n\n" + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private void btnImportShiftUsage_Click(object sender, EventArgs e)
        {
            try
            {
                importShiftUsageFileForm importForm = new importShiftUsageFileForm(items, categories);
                importForm.ShowDialog();

                // Refresh data after import
                LoadData();
                LoadItems();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("ErrorOpeningImportForm") + "\n\n" + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("UnexpectedError") + "\n\n" + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
