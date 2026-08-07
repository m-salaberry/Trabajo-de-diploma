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
    public partial class modItemForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        ItemService itemService = ItemService.Instance();
        List<ItemsCategory> categories;
        List<Item> items;
        ItemsCategory selectedCategory = null;
        Item selectedItem = null;
        public event EventHandler ItemModified;

        /// <summary>
        /// Initialize the modify-item form, center it, store the supplied category and item lists and
        /// populate the combo boxes.
        /// </summary>
        /// <param name="categories">Available item categories used to populate the category selectors.</param>
        /// <param name="items">Existing items available for modification.</param>
        public modItemForm(List<ItemsCategory> categories, List<Item> items)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.categories = categories;
            this.items = items;
            LoadCmb();
        }

        /// <summary>
        /// Applies the current language translations to the window title and the form controls.
        /// </summary>
        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("Modify Item");
            label1.Text = lang.Translate("Name");
            label2.Text = lang.Translate("Category");
            label3.Text = lang.Translate("Unit of measurement");
            ckIntegerUnit.Text = lang.Translate("Integer Unit");
            btnSaveChangesItem.Text = lang.Translate("Save changes");
        }

        /// <summary>
        /// Validate that an item is selected, apply the edited name, category and unit values to it,
        /// persist the update through the item service and raise <see cref="ItemModified"/>.
        /// </summary>
        private void btnSaveChangesItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedItem == null)
                {
                    MessageBox.Show(
                        lang.Translate("No item selected"),
                        lang.Translate("Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                Item itemToMod = items.FirstOrDefault(i => i.Id == selectedItem.Id);
                if (itemToMod == null)
                {
                    throw new MySystemException(lang.Translate("The item selected is null"), "UI");
                }
                itemToMod.Name = txtItemName.Text;
                itemToMod.Category = cmbCategories.SelectedIndex >= 0 ? categories[cmbCategories.SelectedIndex] : null;
                itemToMod.Unit["Name"] = txtUnit.Text;
                itemToMod.Unit["IsInteger"] = ckIntegerUnit.Checked;

                itemService.Update(itemToMod);
                ItemModified?.Invoke(this, EventArgs.Empty);
                MessageBox.Show(
                    lang.Translate("Item updated successfully"),
                    lang.Translate("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                ex.Handler();
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error updating item: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An unexpected error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// Populate the category, filter-category and item combo boxes with their initial selections
        /// and disable the editable fields until an item is chosen.
        /// </summary>
        private void LoadCmb()
        {
            cmbCategories.Items.Clear();
            cmbCategories.Items.AddRange(categories.Select(c => c.Name).ToArray());
            if (categories.Count > 0)
                cmbCategories.SelectedIndex = 0;
            cmbCategories.Enabled = false;

            cmbFilterCategory.Items.Clear();
            cmbFilterCategory.Items.Add(lang.Translate("All Categories"));
            cmbFilterCategory.Items.AddRange(categories.Select(c => c.Name).ToArray());
            cmbFilterCategory.SelectedIndex = 0;

            cmbItems.Items.Clear();
            cmbItems.Items.AddRange(items.Select(i => i.Name).ToArray());
            if (items.Count > 0)
                cmbItems.SelectedIndex = 0;

            txtItemName.Enabled = false;
            txtUnit.Enabled = false;
            ckIntegerUnit.Enabled = false;
        }

        /// <summary>
        /// Populate the items combo box with the items belonging to the given category, or with all
        /// items when no category is provided.
        /// </summary>
        /// <param name="category">Category to filter items by, or <c>null</c> to show all items.</param>
        private void LoadItemsWithCategory(ItemsCategory category)
        {
            var filteredItems = category == null ? items : items.Where(i => i.Category != null && i.Category.Id == category.Id).ToList();
            cmbItems.Items.Clear();
            cmbItems.Items.AddRange(filteredItems.Select(i => i.Name).ToArray());
            if (filteredItems.Count > 0)
            {
                cmbItems.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Update the selected filter category and reload the items combo box accordingly, treating
        /// the first entry as "all categories".
        /// </summary>
        private void cmbFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterCategory.SelectedIndex > 0)
            {
                selectedCategory = categories[cmbFilterCategory.SelectedIndex - 1];
            }
            else
            {
                selectedCategory = null;
            }
            LoadItemsWithCategory(selectedCategory);
        }

        /// <summary>
        /// Load the selected item's name, category and unit details into the editable fields and
        /// enable them.
        /// </summary>
        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbItems.SelectedIndex < 0)
                    return;

                string selectedItemName = cmbItems.SelectedItem.ToString();
                selectedItem = items.FirstOrDefault(i => i.Name == selectedItemName);
                if (selectedItem == null)
                    return;

                txtItemName.Text = selectedItem.Name;
                cmbCategories.SelectedItem = selectedItem.Category != null ? selectedItem.Category.Name : null;
                txtUnit.Text = selectedItem.Unit["Name"] as string;
                ckIntegerUnit.Checked = selectedItem.Unit["IsInteger"] is bool isInt && isInt;

                txtItemName.Enabled = true;
                cmbCategories.Enabled = true;
                txtUnit.Enabled = true;
                ckIntegerUnit.Enabled = true;
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
                Logger.Current.Error($"Error loading item details: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
