using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.secondaryForms;
using Services.Contracts.Logs;
using Services.Contracts.CustomsException;
using Services.Implementations;
using UI.Implementations;
using Domain;
using BLL.Implementations;

namespace UI.controlForms
{
    public partial class ctrlItemsAndCategories : TranslatableUserControls
    {
        LanguageService lang = LanguageService.GetInstance;
        List<ItemsCategory> categories = null;
        List<Item> items = null;
        ItemsCategoryService categoryService = ItemsCategoryService.Instance();
        ItemService itemService = ItemService.Instance();

        public ctrlItemsAndCategories()
        {
            InitializeComponent();
            LoadCategories();
            LoadItems();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            frmMain.GetInstance().ResetMainPanelSize();
            this.Dispose();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            try
            {
                newCategoryForm newCategoryForm = new newCategoryForm();
                // Subscribe to the CategoryAdded event
                newCategoryForm.CategoryAdded += (s, ev) => RefreshCategoryList();
                newCategoryForm.ShowDialog();
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
                Logger.Current.Error($"Error opening new category form: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RefreshCategoryList()
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            categories = categoryService.GetAll().ToList();
            lstbxCategories.Items.Clear();
            lstbxCategories.Items.AddRange(categories.Select(c => c.Name).ToArray());


            if (categories.Count > 0)
            {
                lstbxCategories.SelectedIndex = 0;
            }
        }

        private void LoadItems()
        {
            items = itemService.GetAll().ToList();
            dgvItems.Rows.Clear();
            foreach (var item in items)
            {
                dgvItems.Rows.Add(item.Name, item.Category?.Name ?? "", item.Unit["Name"], item.Unit["IsInteger"]);
            }

        }

        public override void ApplyTranslations()
        {
            // Translate buttons
            btnAddCategory.Text = lang.Translate("New Category");
            deleteCategory.Text = lang.Translate("Delete Category");
            // btnClose keeps its "X" text — must never be translated

            // Translate labels
            lbCategories.Text = lang.Translate("Categories");
            lbItems.Text = lang.Translate("Items");

            // Translate DataGridView columns
            if (dgvItems.Columns["NameColumn"] != null)
                dgvItems.Columns["NameColumn"].HeaderText = lang.Translate("Name");
            if (dgvItems.Columns["CategoryColumn"] != null)
                dgvItems.Columns["CategoryColumn"].HeaderText = lang.Translate("Category");
            if (dgvItems.Columns["UnitColumn"] != null)
                dgvItems.Columns["UnitColumn"].HeaderText = lang.Translate("Unit");
            if (dgvItems.Columns["IntegerUnitColumn"] != null)
                dgvItems.Columns["IntegerUnitColumn"].HeaderText = lang.Translate("Integer Unit");

            // Refresh controls
            dgvItems.Refresh();
        }

        private void deleteCategory_Click(object sender, EventArgs e)
        {
            try
            {
                deleteCategoryForm deleteForm = new deleteCategoryForm(categories);
                // Subscribe to the CategoryDeleted event
                deleteForm.CategoryDeleted += (s, ev) => RefreshCategoryList();
                deleteForm.ShowDialog();
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
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                List<ItemsCategory> categoriesToFilter = new List<ItemsCategory>();
                if (lstbxCategories.CheckedItems.Count <= 0)
                {
                    throw new Exception(lang.Translate("Please select at least one category to filter by."));
                }
                var selectedCategories = lstbxCategories.SelectedItems.Cast<string>().ToList();
                categoriesToFilter = categories.Where(c => selectedCategories.Contains(c.Name)).ToList();
                var filteredItems = items.Where(i => i.Category != null && categoriesToFilter.Any(c => c.Id == i.Category.Id)).ToList();
                dgvItems.Rows.Clear();
                foreach (var item in filteredItems)
                {
                    dgvItems.Rows.Add(item.Name, item.Category?.Name ?? "", item.Unit["Name"], item.Unit["IsInteger"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            foreach (int i in lstbxCategories.CheckedIndices)
            {
                lstbxCategories.SetItemCheckState(i, CheckState.Unchecked);
            }
            LoadItems();
        }

        private void btnModItem_Click(object sender, EventArgs e)
        {
            try
            {
                modItemForm modItemForm = new modItemForm(categories, items);
                // Subscribe to the ItemModified event
                modItemForm.ItemModified += (s, ev) => LoadItems();
                modItemForm.ShowDialog();
                modItemForm.Dispose();
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
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCreateItem_Click(object sender, EventArgs e)
        {
            try
            {
                newItemForm newItemForm = new newItemForm(categories);
                // Subscribe to the ItemAdded event
                newItemForm.ItemAdded += (s, ev) => LoadItems();
                newItemForm.ShowDialog();
                newItemForm.Dispose();
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
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDeteleItem_Click(object sender, EventArgs e)
        {
            try
            {
                deleteItemForm deleteItemForm = new deleteItemForm(categories, items);
                // Subscribe to the ItemDeleted event
                deleteItemForm.ItemDeleted += (s, ev) => LoadItems();
                deleteItemForm.ShowDialog();
                deleteItemForm.Dispose();
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
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
