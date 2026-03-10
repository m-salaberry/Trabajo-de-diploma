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
    public partial class deleteProviderForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        ProviderService providerService = ProviderService.Instance();

        List<ItemsCategory> categories;
        List<Provider> providers;
        public event EventHandler ProviderDeleted;
        public deleteProviderForm(List<ItemsCategory> categories, List<Provider> providers)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.categories = categories;
            this.providers = providers;
            LoadProviders();
            LoadCategories();
        }

        private void LoadProviders(List<Provider> source = null)
        {
            var list = source ?? providers;
            lstbxProviders.DataSource = null;
            lstbxProviders.DataSource = list;
            lstbxProviders.DisplayMember = "Name";
        }

        private void LoadCategories()
        {
            var filterCategories = new List<ItemsCategory>
            {
                new ItemsCategory { Id = 0, Name = lang.Translate("All categories") }
            };
            filterCategories.AddRange(categories);

            cmbChooseCategory.DataSource = null;
            cmbChooseCategory.DataSource = filterCategories;
            cmbChooseCategory.DisplayMember = "Name";
            cmbChooseCategory.ValueMember = "Id";
            cmbChooseCategory.SelectedIndex = 0;
        }

        private void cmbChooseCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChooseCategory.SelectedIndex == 0)
            {
                LoadProviders();
            }
            else
            {
                int selectedCategoryId = (int)cmbChooseCategory.SelectedValue;
                var filteredProviders = providers
                    .Where(p => p.Category != null && p.Category.Id == selectedCategoryId)
                    .ToList();
                LoadProviders(filteredProviders);
            }
        }

        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("Delete Provider");
            btnDelete.Text = lang.Translate("Delete");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Provider selectedProvider = lstbxProviders.SelectedItem as Provider;
                if (selectedProvider == null)
                {
                    MessageBox.Show(
                        lang.Translate("Please select a provider to delete."),
                        lang.Translate("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show(
                    lang.Translate($"Are you sure you want to delete provider '{selectedProvider.Name}'?"),
                    lang.Translate("Confirm Delete"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    providerService.Delete(selectedProvider.Id);
                    MessageBox.Show(
                        lang.Translate("Provider deleted successfully."),
                        lang.Translate("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    providers.Remove(selectedProvider);
                    cmbChooseCategory_SelectedIndexChanged(null, null);
                    ProviderDeleted?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
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
                Logger.Current.Error($"Error deleting provider: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
