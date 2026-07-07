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
using UI.secondaryForms;

namespace UI.controlForms
{
    public partial class ctrlProductBuilder : TranslatableUserControls
    {
        LanguageService lang = LanguageService.GetInstance;
        ProductService productService = ProductService.Instance();
        ItemService itemService = ItemService.Instance();
        List<Product> products;
        List<Item> items;

        List<Item> selectedItems;
        Product currentProduct;
        /// <summary>
        /// Initializes the product builder control, applies the initial disabled configuration and
        /// loads the available items and existing products.
        /// </summary>
        public ctrlProductBuilder()
        {
            InitializeComponent();
            InitialConfig();
            GetItems();
            LoadProducts();
        }

        /// <summary>
        /// Reloads all products from the product service and lists their names in the products list box.
        /// </summary>
        private void LoadProducts()
        {
            lstbxProducts.Items.Clear();
            products = productService.GetAll().ToList();
            foreach (var product in products)
            {
                lstbxProducts.Items.Add(product.Name);
            }
        }

        /// <summary>
        /// Loads all items from the item service into the local items list.
        /// </summary>
        private void GetItems()
        {
            items = itemService.GetAll().ToList();
        }
        /// <summary>
        /// Applies the initial inactive state: disables the inputs, buttons and detail grid and greys
        /// out their styling to signal that no product is being edited.
        /// </summary>
        private void InitialConfig()
        {
            txtProductName.Enabled = false;
            nmCode.Enabled = false;

            btnAddItem.Enabled = false;
            btnAddItem.BackColor = SystemColors.ControlLight;
            btnAddItem.ForeColor = SystemColors.GrayText;

            btnSaveRecipe.Enabled = false;
            btnSaveRecipe.BackColor = SystemColors.ControlLight;
            btnSaveRecipe.ForeColor = SystemColors.GrayText;

            btnCancel.Enabled = false;
            btnCancel.BackColor = SystemColors.ControlLight;
            btnCancel.ForeColor = SystemColors.GrayText;

            dgvDetailProduct.Enabled = false;
            dgvDetailProduct.BackgroundColor = SystemColors.Control;
            dgvDetailProduct.DefaultCellStyle.BackColor = SystemColors.Control;
            dgvDetailProduct.DefaultCellStyle.ForeColor = SystemColors.GrayText;
            dgvDetailProduct.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.GrayText;
            QuantityToConsume.ReadOnly = true;

            ItemAction.Text = lang.Translate("Delete");
            ItemAction.UseColumnTextForButtonValue = true;

            label2.ForeColor = SystemColors.GrayText;
            label3.ForeColor = SystemColors.GrayText;
            lbRecipeDetail.ForeColor = SystemColors.GrayText;
            lbRecipeDetail.Text = lang.Translate("Recipe Details: -");
        }

        /// <summary>
        /// Switches the control into editing state: enables the inputs, buttons and detail grid and
        /// restores their normal (active) styling.
        /// </summary>
        private void EditConfig()
        {
            txtProductName.Enabled = true;
            nmCode.Enabled = true;

            btnAddItem.Enabled = true;
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.ForeColor = SystemColors.ControlText;

            btnSaveRecipe.Enabled = true;
            btnSaveRecipe.UseVisualStyleBackColor = true;
            btnSaveRecipe.ForeColor = SystemColors.ControlText;

            btnCancel.Enabled = true;
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.ForeColor = SystemColors.ControlText;

            dgvDetailProduct.Enabled = true;
            dgvDetailProduct.BackgroundColor = SystemColors.Window;
            dgvDetailProduct.DefaultCellStyle.BackColor = SystemColors.Window;
            dgvDetailProduct.DefaultCellStyle.ForeColor = SystemColors.ControlText;
            dgvDetailProduct.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            QuantityToConsume.ReadOnly = false;

            label2.ForeColor = SystemColors.ControlText;
            label3.ForeColor = SystemColors.ControlText;
            lbRecipeDetail.ForeColor = SystemColors.ControlText;
        }

        /// <summary>
        /// Handles the Close button click: removes this control from its parent, resets the main
        /// panel size and disposes the control.
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            frmMain.GetInstance().ResetMainPanelSize();
            this.Dispose();
        }

        /// <summary>
        /// Handles the product search text change: reloads all products when the box is empty,
        /// otherwise lists only the products whose name contains the search text.
        /// </summary>
        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchProduct.Text == "") 
            {
                LoadProducts();
            }
            else
            {
                lstbxProducts.Items.Clear();
                var filtered = products.Where(p => p.Name.IndexOf(txtSearchProduct.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                foreach (var product in filtered)
                {
                    lstbxProducts.Items.Add(product.Name);
                }
            }
        }

        /// <summary>
        /// Handles the product name text change: updates the recipe detail label to include the
        /// typed name, or shows a placeholder when the name is empty.
        /// </summary>
        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            if (txtProductName.Text != "")
            {
                lbRecipeDetail.Text = lang.Translate("Recipe Details: ") + txtProductName.Text;
            }
            else
            {
                lbRecipeDetail.Text = lang.Translate("Recipe Details: -");
            }
        }

        /// <summary>
        /// Handles the New Product button click: starts a fresh product with empty details, clears
        /// the inputs and detail grid and switches the control into editing state.
        /// </summary>
        private void btnNewProduct_Click(object sender, EventArgs e)
        {
            try
            {
                currentProduct = new Product { DetailProducts = new List<DetailProduct>() };
                selectedItems = new List<Item>();
                txtProductName.Text = string.Empty;
                nmCode.Value = 0;
                ClearDetailProductGrid();
                EditConfig();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error starting new product: {ex.Message}");
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the Add Item button click: opens the item picker dialog and, if the user confirms,
        /// adds the newly chosen items to the recipe (skipping duplicates) and shows their grid rows.
        /// </summary>
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                addItemToProductForm addItemToProductForm = new addItemToProductForm(items);

                List<Item> newlyAdded = null;
                addItemToProductForm.OnItemsAdded += (s, itemsToAdd) =>
                {
                    newlyAdded = itemsToAdd;
                };

                addItemToProductForm.ShowDialog();

                if (newlyAdded == null)
                    return;

                selectedItems ??= new List<Item>();
                foreach (var item in newlyAdded)
                {
                    if (!selectedItems.Any(i => i.Name == item.Name))
                        selectedItems.Add(item);
                }

                AddMissingItemRows();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error adding items to recipe: {ex.Message}");
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the Save Recipe button click: validates that a product is being edited with a
        /// name and at least one item, builds its detail rows from the grid quantities, then inserts
        /// or updates the product and resets the control to its initial state.
        /// </summary>
        private void btnSaveRecipe_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentProduct == null)
                {
                    MessageBox.Show(lang.Translate("No product is being edited."), lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtProductName.Text))
                {
                    MessageBox.Show(lang.Translate("Product name cannot be empty."), lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (selectedItems == null || !selectedItems.Any())
                {
                    MessageBox.Show(lang.Translate("Please add at least one item to the recipe."), lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                currentProduct.Code = (int)nmCode.Value;
                currentProduct.Name = txtProductName.Text.Trim();

                currentProduct.DetailProducts ??= new List<DetailProduct>();
                currentProduct.DetailProducts.Clear();

                foreach (var item in selectedItems)
                {
                    currentProduct.DetailProducts.Add(new DetailProduct
                    {
                        Item = item,
                        QuantityToConsume = dgvDetailProduct.Rows.Cast<DataGridViewRow>()
                            .Where(r => r.Cells[nameof(ItemName)].Value?.ToString() == item.Name)
                            .Select(r => decimal.TryParse(r.Cells[nameof(QuantityToConsume)].Value?.ToString(), out var qty) ? qty : 0m)
                            .FirstOrDefault()
                    });
                }

                if (currentProduct.Id == 0)
                {
                    productService.Insert(currentProduct);
                    MessageBox.Show(lang.Translate("Product created successfully!"), lang.Translate("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    productService.Update(currentProduct);
                    MessageBox.Show(lang.Translate("Product updated successfully!"), lang.Translate("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                currentProduct = null;
                selectedItems = null;
                LoadProducts();
                ClearDetailProductGrid();
                InitialConfig();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error saving product recipe: {ex.Message}");
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the Cancel button click: after user confirmation, discards the in-progress product
        /// and unsaved changes, clears the inputs and grid and returns the control to its initial state.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    lang.Translate("Are you sure you want to cancel? All unsaved changes will be lost."),
                    lang.Translate("Confirm Cancel"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    currentProduct = null;
                    selectedItems = null;
                    txtProductName.Text = string.Empty;
                    nmCode.Value = 0;
                    ClearDetailProductGrid();
                    InitialConfig();
                }
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error canceling product edit: {ex.Message}");
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles clicks on the detail grid: when the Delete action cell is clicked, removes the
        /// corresponding item from the selected items and deletes its grid row.
        /// </summary>
        private void dgvDetailProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvDetailProduct.Columns[e.ColumnIndex].Name != nameof(ItemAction))
                return;

            string itemName = dgvDetailProduct.Rows[e.RowIndex].Cells[nameof(ItemName)].Value?.ToString();

            if (selectedItems != null)
            {
                var toRemove = selectedItems.FirstOrDefault(i => i.Name == itemName);
                if (toRemove != null)
                    selectedItems.Remove(toRemove);
            }

            dgvDetailProduct.Rows.RemoveAt(e.RowIndex);
        }

        /// <summary>
        /// Rebuilds the detail grid from a list of items, adding one row per item with a zero quantity
        /// to consume and its unit name.
        /// </summary>
        /// <param name="itemsOfProduct">The items to display, each starting at quantity zero.</param>
        private void FillDetailProductGrid(List<Item> itemsOfProduct)
        {
            dgvDetailProduct.Rows.Clear();
            if (itemsOfProduct != null)
            {
                foreach (var item in itemsOfProduct)
                {
                    string unitName = item.Unit.TryGetValue("Name", out var u) ? u?.ToString() : string.Empty;
                    dgvDetailProduct.Rows.Add(item.Name, 0, unitName);
                }
            }
            dgvDetailProduct.Refresh();
        }

        /// <summary>
        /// Rebuilds the detail grid from a list of product details, adding one row per detail with its
        /// item name, quantity to consume and unit name.
        /// </summary>
        /// <param name="details">The product details to display in the grid.</param>
        private void FillDetailProductGrid(List<DetailProduct> details)
        {
            dgvDetailProduct.Rows.Clear();
            if (details != null)
            {
                foreach (var detail in details)
                {
                    string unitName = detail.Item.Unit.TryGetValue("Name", out var u) ? u?.ToString() : string.Empty;
                    dgvDetailProduct.Rows.Add(detail.Item.Name, detail.QuantityToConsume, unitName);
                }
            }
            dgvDetailProduct.Refresh();
        }

        /// <summary>
        /// Clears all rows from the detail product grid and refreshes it.
        /// </summary>
        private void ClearDetailProductGrid()
        {
            dgvDetailProduct.Rows.Clear();
            dgvDetailProduct.Refresh();
        }

        /// <summary>
        /// Adds grid rows for selected items not yet shown, preserving existing rows and their
        /// already-typed quantities (unlike FillDetailProductGrid which rebuilds everything at qty 0).
        /// </summary>
        private void AddMissingItemRows()
        {
            var existingNames = dgvDetailProduct.Rows.Cast<DataGridViewRow>()
                .Select(r => r.Cells[nameof(ItemName)].Value?.ToString())
                .Where(n => !string.IsNullOrEmpty(n))
                .ToHashSet();

            foreach (var item in selectedItems)
            {
                if (existingNames.Contains(item.Name))
                    continue;

                string unitName = item.Unit.TryGetValue("Name", out var u) ? u?.ToString() : string.Empty;
                dgvDetailProduct.Rows.Add(item.Name, 0, unitName);
            }
            dgvDetailProduct.Refresh();
        }

        /// <summary>
        /// Applies the current language translations to all labels, buttons, search placeholder and
        /// detail grid column headers of the control.
        /// </summary>
        public override void ApplyTranslations()
        {
            lbCategories.Text = lang.Translate("Products");
            txtSearchProduct.PlaceholderText = lang.Translate("Search");
            btnNewProduct.Text = lang.Translate("New Product");
            btnModProduct.Text = lang.Translate("Modify");
            btnDelete.Text = lang.Translate("Delete");
            btnAddItem.Text = lang.Translate("Add Item to Recipe");
            btnSaveRecipe.Text = lang.Translate("Save Recipe");
            btnCancel.Text = lang.Translate("Cancel");
            label2.Text = lang.Translate("Product Code:");
            label3.Text = lang.Translate("Product Name:");
            lbRecipeDetail.Text = lang.Translate("Recipe Details: -");
            ItemName.HeaderText = lang.Translate("Item Name");
            QuantityToConsume.HeaderText = lang.Translate("Quantity to Consume");
            ItemUnit.HeaderText = lang.Translate("Unit");
            ItemAction.HeaderText = lang.Translate("Action");
            ItemAction.Text = lang.Translate("Delete");
        }

        /// <summary>
        /// Handles the Modify button click: loads the selected product into the editor (name, code
        /// and detail items) and switches the control into editing state, or warns when none is selected.
        /// </summary>
        private void btnModProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstbxProducts.SelectedItem == null)
                {
                    MessageBox.Show(lang.Translate("Please select a product to modify."), lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                currentProduct = products.FirstOrDefault(p => p.Name == lstbxProducts.SelectedItem.ToString());
                if (currentProduct != null)
                {
                    txtProductName.Text = currentProduct.Name;
                    nmCode.Value = currentProduct.Code;
                    selectedItems = currentProduct.DetailProducts?.Select(dp => dp.Item).ToList() ?? new List<Item>();
                    FillDetailProductGrid(currentProduct.DetailProducts ?? new List<DetailProduct>());
                    EditConfig();
                }
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error loading product for modification: {ex.Message}");
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the Delete button click: after user confirmation, deletes the selected product,
        /// reloads the product list and resets the control, or warns when none is selected.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstbxProducts.SelectedItem == null)
                {
                    MessageBox.Show(lang.Translate("Please select a product to delete."), lang.Translate("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                currentProduct = products.FirstOrDefault(p => p.Name == lstbxProducts.SelectedItem.ToString());
                if (currentProduct != null)
                {
                    DialogResult result = MessageBox.Show(
                        lang.Translate($"Are you sure you want to delete the product '{currentProduct.Name}'?"),
                        lang.Translate("Confirm Delete"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        productService.Delete(currentProduct.Id);
                        MessageBox.Show(lang.Translate("Product deleted successfully!"), lang.Translate("Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        currentProduct = null;
                        LoadProducts();
                        ClearDetailProductGrid();
                        InitialConfig();
                    }
                }
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error deleting product: {ex.Message}");
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message, lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
