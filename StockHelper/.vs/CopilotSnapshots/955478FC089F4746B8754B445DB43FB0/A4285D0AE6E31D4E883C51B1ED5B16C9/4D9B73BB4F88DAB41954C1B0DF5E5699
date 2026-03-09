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
    public partial class deleteItemForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        ItemService itemService = ItemService.Instance();
        List<ItemsCategory> categories;
        List<Item> items;
        public event EventHandler ItemDeleted;
        public deleteItemForm(List<ItemsCategory> categories, List<Item> items)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.categories = categories;
            this.items = items;
            LoadCmb();

        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstbxItems.SelectedIndex < 0)
                {
                    MessageBox.Show(
                        lang.Translate("Please select an item to delete."),
                        lang.Translate("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                string selectedName = lstbxItems.SelectedItem.ToString();
                Item itemToDelete = items.FirstOrDefault(i => i.Name == selectedName);
                if (itemToDelete == null)
                {
                    MessageBox.Show(
                        lang.Translate("Please select an item to delete."),
                        lang.Translate("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                var confirmResult = MessageBox.Show(
                    lang.Translate("Are you sure you want to delete the selected item?") + "\n" + lang.Translate("Item selected:") + " " + itemToDelete.Name,
                    lang.Translate("Confirm Delete"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult == DialogResult.Yes)
                {
                    itemService.Delete(itemToDelete.Id);
                    items.Remove(itemToDelete);
                    LoadItems();
                    ItemDeleted?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show(
                        lang.Translate("Item deleted successfully."),
                        lang.Translate("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
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
                Logger.Current.Error($"Error deleting item: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An unexpected error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadCmb()
        {
            cmbFilterCategories.Items.Clear();
            cmbFilterCategories.Items.Add(lang.Translate("All Categories"));
            foreach (var category in categories)
            {
                cmbFilterCategories.Items.Add(category.Name);
            }
            cmbFilterCategories.SelectedIndex = 0;
            LoadItems();
        }

        private void LoadItems()
        {
            lstbxItems.Items.Clear();
            IEnumerable<Item> filteredItems = items;
            if (cmbFilterCategories.SelectedIndex > 0)
            {
                string selectedCategoryName = cmbFilterCategories.SelectedItem.ToString();
                filteredItems = filteredItems.Where(i => i.Category != null && i.Category.Name == selectedCategoryName);
            }
            foreach (var item in filteredItems)
            {
                lstbxItems.Items.Add(item.Name);
            }

        }

        private void cmbFilterCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadItems();
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error filtering items: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An unexpected error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
