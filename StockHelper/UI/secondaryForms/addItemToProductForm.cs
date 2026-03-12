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
        public addItemToProductForm(List<Item> items)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.items = items;
            LoadCategories();
            LoadItems();
        }

        private void LoadCategories()
        {
            categories = itemsCategoryService.GetAll().ToList();
            cmbFilterCategories.Items.Add(lang.Translate("All"));
            foreach (var category in categories)
            {
                cmbFilterCategories.Items.Add(category.Name);
            }
        }

        private void LoadItems()
        {
            if (cmbFilterCategories.SelectedIndex == -1)
            {
                foreach (var item in items)
                {
                    cklstItems.Items.Add(item.Name);
                }
            }
            if (cmbFilterCategories.SelectedIndex != -1)
            {
                var selectedCategory = cmbFilterCategories.SelectedItem as ItemsCategory;
                var filteredItems = items.Where(i => i.Category.Id == selectedCategory.Id).ToList();
                foreach (var item in filteredItems)
                {
                    cklstItems.Items.Add(item.Name);
                }
            }
        }

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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                cklstItems.Items.Clear();
                LoadItems();
            }
            if (cmbFilterCategories.SelectedIndex == -1)
            {
                var filteredItems = items.Where(i => i.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                cklstItems.Items.Clear();
                foreach (var item in filteredItems)
                {
                    cklstItems.Items.Add(item.Name);
                }
            }
            if (cmbFilterCategories.SelectedIndex != -1)
            {
                var selectedCategory = cmbFilterCategories.SelectedItem as ItemsCategory;
                var filteredItems = items.Where(i => i.Category.Id == selectedCategory.Id && i.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                cklstItems.Items.Clear();
                foreach (var item in filteredItems)
                {
                    cklstItems.Items.Add(item.Name);
                }
            }
        }

        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("Add Item");
            btnAdd.Text = lang.Translate("Add");
            txtSearch.PlaceholderText = lang.Translate("Search");
        }
    }
}
