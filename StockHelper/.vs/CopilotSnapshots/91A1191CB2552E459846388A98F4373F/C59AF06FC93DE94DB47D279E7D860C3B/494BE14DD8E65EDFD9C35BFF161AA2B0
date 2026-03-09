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
using BLL.Implementations;
using Domain;

namespace UI.secondaryForms
{
    public partial class newCategoryForm : TranslatableForm
    {
        public event EventHandler CategoryAdded;
        private ItemsCategoryService _categoryService = ItemsCategoryService.Instance();

        public newCategoryForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                {
                    MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (_categoryService.GetAll().Any(c => c.Name.Equals(txtCategoryName.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("A category with this name already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtCategoryName.Text.Trim().Length > 100)
                {
                    MessageBox.Show("Category name cannot exceed 100 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ItemsCategory newCategory = new ItemsCategory
                {
                    Name = txtCategoryName.Text.Trim()
                };

                _categoryService.Insert(newCategory);

                CategoryAdded?.Invoke(this, EventArgs.Empty);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }
    }
}
