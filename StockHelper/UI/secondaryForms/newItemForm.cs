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
    public partial class newItemForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        List<ItemsCategory> categories = null;
        ItemService itemService = ItemService.Instance();
        public event EventHandler ItemAdded;
        /// <summary>
        /// Initializes the form with the selectable categories and binds them to the category combo.
        /// </summary>
        /// <param name="categories">The categories an item can be assigned to.</param>
        public newItemForm(List<ItemsCategory> categories)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.categories = categories;
            LoadCategories();
        }

        /// <summary>
        /// Applies the current language translations to the window title and the form controls.
        /// </summary>
        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("New Item");
            label1.Text = lang.Translate("Name");
            label2.Text = lang.Translate("Category");
            label3.Text = lang.Translate("Unit of measurement");
            ckIntegerUnit.Text = lang.Translate("Integer Unit");
            btnSaveItem.Text = lang.Translate("Save");
        }

        /// <summary>
        /// Builds a new item from the form fields (name, category, unit and zero stock), persists it,
        /// raises the ItemAdded event and closes the form.
        /// </summary>
        private void btnSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                Item newItem = new Item
                {
                    Name = txtItemName.Text,
                    Category = categories.FirstOrDefault(c => c.Id == (int)cmbCategories.SelectedValue),
                    Unit = new Dictionary<string, object>
                    {
                        { "Name", txtUnit.Text },
                        { "IsInteger", ckIntegerUnit.Checked }
                    },
                    Stock = 0
                };
                itemService.Insert(newItem);
                MessageBox.Show(
                    lang.Translate("Item added successfully!"),
                    lang.Translate("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ItemAdded?.Invoke(this, EventArgs.Empty);
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
                Logger.Current.Error($"Error saving new item: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Binds the categories to the combo, using the name for display and the id as the value.
        /// </summary>
        private void LoadCategories()
        {
            cmbCategories.DataSource = null;
            cmbCategories.DataSource = categories;
            cmbCategories.DisplayMember = "Name";
            cmbCategories.ValueMember = "Id";
        }
    }
}
