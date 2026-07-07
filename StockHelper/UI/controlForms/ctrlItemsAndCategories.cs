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

        /// <summary>
        /// Initializes the items and categories control, loading the category and item lists.
        /// </summary>
        public ctrlItemsAndCategories()
        {
            InitializeComponent();
            LoadCategories();
            LoadItems();
        }

        /// <summary>
        /// Handles the split container splitter moved event. No action is taken.
        /// </summary>
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

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
        /// Opens the new category dialog and refreshes the category list when a category is added.
        /// </summary>
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

        /// <summary>
        /// Reloads the category list.
        /// </summary>
        private void RefreshCategoryList()
        {
            LoadCategories();
        }

        /// <summary>
        /// Loads all categories from the service into the categories list box and selects the first one.
        /// </summary>
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

        /// <summary>
        /// Loads all items from the service and populates the items grid.
        /// </summary>
        private void LoadItems()
        {
            items = itemService.GetAll().ToList();
            dgvItems.Rows.Clear();
            foreach (var item in items)
            {
                dgvItems.Rows.Add(item.Name, item.Category?.Name ?? "", item.Unit["Name"], item.Unit["IsInteger"]);
            }

        }

        /// <summary>
        /// Applies the current language translations to the buttons, labels and item grid columns.
        /// </summary>
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

        /// <summary>
        /// Opens the delete category dialog and refreshes the category list when a category is deleted.
        /// </summary>
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

        /// <summary>
        /// Filters the items grid to show only items belonging to the categories checked in the list box.
        /// </summary>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                List<ItemsCategory> categoriesToFilter = new List<ItemsCategory>();
                if (lstbxCategories.CheckedItems.Count <= 0)
                {
                    throw new Exception(lang.Translate("Please select at least one category to filter by."));
                }
                var selectedCategories = lstbxCategories.CheckedItems.Cast<string>().ToList();
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

        /// <summary>
        /// Clears all checked categories and reloads the full item list.
        /// </summary>
        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            foreach (int i in lstbxCategories.CheckedIndices)
            {
                lstbxCategories.SetItemCheckState(i, CheckState.Unchecked);
            }
            LoadItems();
        }

        /// <summary>
        /// Opens the modify item dialog and reloads the items grid when an item is modified.
        /// </summary>
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

        /// <summary>
        /// Opens the new item dialog and reloads the items grid when an item is added.
        /// </summary>
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

        /// <summary>
        /// Opens the delete item dialog and reloads the items grid when an item is deleted.
        /// </summary>
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
