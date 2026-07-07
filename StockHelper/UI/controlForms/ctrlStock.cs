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
        /// <summary>
        /// Initializes the stock control, loading items, categories and the initial edit-mode configuration.
        /// </summary>
        public ctrlStock()
        {
            InitializeComponent();
            LoadData();
            LoadCategories();
            LoadItems();
            InitialConfig();
        }

        /// <summary>
        /// Applies the current language translations to the labels, buttons, grid columns and category combo box.
        /// </summary>
        public override void ApplyTranslations()
        {
            label1.Text = lang.Translate("Items Stock List");
            btnEditMode.Text = lang.Translate("Enable Edition");
            btnSaveEdit.Text = lang.Translate("Save");
            btnImportShiftUsage.Text = lang.Translate("Import Shift Usage");
            btnCancelEdit.Text = lang.Translate("Cancel Edition");

            if (dgvItemsAndStock.Columns["ItemName"] != null)
                dgvItemsAndStock.Columns["ItemName"].HeaderText = lang.Translate("Name");
            if (dgvItemsAndStock.Columns["ItemCategory"] != null)
                dgvItemsAndStock.Columns["ItemCategory"].HeaderText = lang.Translate("Category");
            if (dgvItemsAndStock.Columns["ItemUnit"] != null)
                dgvItemsAndStock.Columns["ItemUnit"].HeaderText = lang.Translate("Unit");
            if (dgvItemsAndStock.Columns["ItemStock"] != null)
                dgvItemsAndStock.Columns["ItemStock"].HeaderText = lang.Translate("Current Stock");
            if (dgvItemsAndStock.Columns["ItemUpdatedDate"] != null)
                dgvItemsAndStock.Columns["ItemUpdatedDate"].HeaderText = lang.Translate("Last Updated");

            dgvItemsAndStock.Refresh();

            // Refresh category combobox if needed
            if (cmbCategories.Items.Count > 0)
            {
                int selIndex = cmbCategories.SelectedIndex;
                LoadCategories();
                cmbCategories.SelectedIndex = selIndex >= 0 ? selIndex : 0;
            }
        }

        /// <summary>
        /// Loads all items and categories from the services and resets the filtered items list.
        /// </summary>
        private void LoadData()
        {
            items = itemService.GetAll().ToList();
            categories = itemsCategoryService.GetAll().ToList();
            filteredItems = items;
        }

        /// <summary>
        /// Fills the category combo box with an "All" entry followed by every available category name.
        /// </summary>
        private void LoadCategories()
        {
            cmbCategories.Items.Clear();
            cmbCategories.Items.Add(lang.Translate("All"));
            foreach (var category in categories)
            {
                cmbCategories.Items.Add(category.Name);
            }
        }

        /// <summary>
        /// Resets the filtered list to all items and reapplies the current filters.
        /// </summary>
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

        /// <summary>
        /// Renders the given items into the stock grid, tagging each row with its source item.
        /// </summary>
        /// <param name="source">The items to display in the grid.</param>
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

        /// <summary>
        /// Closes the control, removing it from its parent, resetting the main panel size and disposing it.
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            frmMain.GetInstance().ResetMainPanelSize();
            this.Dispose();
        }

        /// <summary>
        /// Reapplies the filters when a category is selected in the combo box.
        /// </summary>
        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategories.SelectedIndex != -1)
            {
                ApplyFilters();
            }
        }

        /// <summary>
        /// Reapplies the filters whenever the search text changes.
        /// </summary>
        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Sets the initial read-only state, disabling the cancel and save buttons.
        /// </summary>
        private void InitialConfig()
        {
            btnCancelEdit.Enabled = false;
            btnSaveEdit.Enabled = false;
            dgvItemsAndStock.ReadOnly = true;
        }

        /// <summary>
        /// Enables stock editing, making only the stock column editable and toggling the relevant buttons.
        /// </summary>
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

        /// <summary>
        /// Leaves stock editing, restoring the read-only state and toggling the relevant buttons.
        /// </summary>
        private void ExitEditMode()
        {
            ItemStock.ReadOnly = true;
            dgvItemsAndStock.ReadOnly = true;
            btnEditMode.Enabled = true;
            btnSaveEdit.Enabled = false;
            btnCancelEdit.Enabled = false;
            btnImportShiftUsage.Enabled = true;
        }

        /// <summary>
        /// Enters stock edit mode when the edit button is clicked.
        /// </summary>
        private void btnEditMode_Click(object sender, EventArgs e)
        {
            EnterEditMode();
        }

        /// <summary>
        /// Discards edits by reloading the data and exiting edit mode.
        /// </summary>
        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            LoadData();
            LoadItems();
            ExitEditMode();
        }

        /// <summary>
        /// Validates and saves the edited stock values after confirmation, updating only changed items,
        /// then reloads the data and exits edit mode.
        /// </summary>
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


        /// <summary>
        /// Opens the shift usage import dialog and refreshes the data once it closes.
        /// </summary>
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
