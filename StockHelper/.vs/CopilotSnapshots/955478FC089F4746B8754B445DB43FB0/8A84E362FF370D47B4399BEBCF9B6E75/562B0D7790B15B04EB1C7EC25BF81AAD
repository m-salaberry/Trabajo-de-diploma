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

namespace UI.secondaryForms
{
    public partial class newItemForm : Form
    {
        LanguageService lang = LanguageService.GetInstance;
        List<ItemsCategory> categories = null;
        ItemService itemService = ItemService.Instance();
        public newItemForm(List<ItemsCategory> categories)
        {
            InitializeComponent();
            this.categories = categories;
        }

        private void btnSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                Item newItem = new Item
                {
                    Name = txtItemName.Text,
                    Category = (ItemsCategory)cmbCategories.SelectedValue!,
                    Unit = new Dictionary<string, object>
                    {
                        { "Name", txtUnit.Text },
                        { "IsInteger", ckIntegerUnit.Checked }
                    },
                };
                itemService.Insert(newItem);
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
    }
}
