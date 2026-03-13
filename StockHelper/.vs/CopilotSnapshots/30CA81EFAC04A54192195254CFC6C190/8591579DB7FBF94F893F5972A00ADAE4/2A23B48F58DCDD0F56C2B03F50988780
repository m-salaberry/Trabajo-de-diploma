using BLL.Implementations;
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
using Domain;

namespace UI.controlForms
{
    public partial class ctrlStock : TranslatableUserControls   
    {
        LanguageService lang = LanguageService.GetInstance;
        ItemService itemService = ItemService.Instance();
        ItemsCategoryService itemsCategoryService = ItemsCategoryService.Instance();

        List<Item> items;
        List<ItemsCategory> categories;
        public ctrlStock()
        {
            InitializeComponent();
            LoadData();
            LoadCategories();
            LoadItems();
        }

        private void LoadData()
        {
            items = itemService.GetAll().ToList();
            categories = itemsCategoryService.GetAll().ToList();
        }

        private void LoadCategories()
        {
            cmbCategories.Items.Clear();
            foreach (var category in categories)
            {
                cmbCategories.Items.Add(category.Name);
            }
        }

        private void LoadItems()
        {
            dgvItemsAndStock.Rows.Clear();
            foreach (var item in items)
            {
                dgvItemsAndStock.Rows.Add(item.Name, item.Category.Name, item.Unit["name"], item.Stock, item.LastUpdate);
            }
            dgvItemsAndStock.Refresh();
        }
    }
}
