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
    public partial class addItemToProductForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        ItemsCategoryService itemsCategoryService = ItemsCategoryService.Instance();
        List<ItemsCategory> categories;
        List<Item> items;
        public event EventHandler<List<Item>> OnItemsAdded;
        /// <summary>
        /// Initializes the form with the candidate items and loads the category filter and item list.
        /// </summary>
        /// <param name="items">The collection of items available to be added to the product.</param>
        public addItemToProductForm(List<Item> items)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.items = items;
            LoadCategories();
            LoadItems();
        }

        /// <summary>
        /// Loads all categories into the filter combo, adding an "All" option selected by default.
        /// </summary>
        private void LoadCategories()
        {
            categories = itemsCategoryService.GetAll().ToList();
            cmbFilterCategories.Items.Add(lang.Translate("All"));
            foreach (var category in categories)
            {
                cmbFilterCategories.Items.Add(category.Name);
            }
            cmbFilterCategories.SelectedIndex = 0;
        }

        /// <summary>
        /// Populates the checked list of items by rendering the current filters.
        /// </summary>
        private void LoadItems()
        {
            RenderItems();
        }

        /// <summary>
        /// Rebuilds the checked list applying the category (index 0 = "All") and search filters.
        /// The combo holds category names (strings), so it is matched by name — never cast to a domain type.
        /// </summary>
        private void RenderItems()
        {
            cklstItems.Items.Clear();

            IEnumerable<Item> result = items;

            if (cmbFilterCategories.SelectedIndex > 0)
            {
                string categoryName = cmbFilterCategories.SelectedItem.ToString();
                result = result.Where(i => i.Category != null && i.Category.Name == categoryName);
            }

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string term = txtSearch.Text.ToLower();
                result = result.Where(i => i.Name.ToLower().Contains(term));
            }

            foreach (var item in result)
            {
                cklstItems.Items.Add(item.Name);
            }
        }

        /// <summary>
        /// Re-renders the item list when the selected category filter changes.
        /// </summary>
        private void cmbFilterCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderItems();
        }

        /// <summary>
        /// Collects the checked items, raises the OnItemsAdded event with them and closes the form,
        /// requiring at least one item to be selected.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cklstItems.CheckedItems.Count == 0)
                {
                    MessageBox.Show(
                        lang.Translate("Please select at least one item to add."),
                        lang.Translate("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                List<Item> itemsToAdd = new List<Item>();
                foreach (var checkedItem in cklstItems.CheckedItems)
                {
                    var item = items.FirstOrDefault(i => i.Name == checkedItem.ToString());
                    if (item != null)
                        itemsToAdd.Add(item);
                }

                OnItemsAdded?.Invoke(this, itemsToAdd);
                this.Close();
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
                Logger.Current.Error($"Error adding items to product: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Re-renders the item list when the search text changes.
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RenderItems();
        }

        /// <summary>
        /// Applies the current language translations to the form title, controls and placeholders.
        /// </summary>
        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("Add Item");
            btnAdd.Text = lang.Translate("Add");
            txtSearch.PlaceholderText = lang.Translate("Search");
        }
    }
}
