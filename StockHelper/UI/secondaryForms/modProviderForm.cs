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
    public partial class modProviderForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        ProviderService providerService = ProviderService.Instance();

        List<ItemsCategory> categories;
        List<Provider> providers;
        private int _lastValidProviderIndex = -1;
        public event EventHandler ProviderModded;
        /// <summary>
        /// Initialize the modify-provider form, center it, store the supplied category and provider
        /// lists, populate the dropdowns and apply the initial disabled-controls configuration.
        /// </summary>
        /// <param name="categories">Available item categories used to populate the category selectors.</param>
        /// <param name="providers">Existing providers available for modification.</param>
        public modProviderForm(List<ItemsCategory> categories, List<Provider> providers)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.providers = providers;
            this.categories = categories;
            LoadCategories();
            LoadProviders();
            InitialConfig();
        }
        /// <summary>
        /// Bind the read-only category selector to all categories and the filter selector to the
        /// categories preceded by an "All categories" option.
        /// </summary>
        private void LoadCategories()
        {
            cmbCategories.DataSource = null;
            cmbCategories.DataSource = categories;
            cmbCategories.DisplayMember = "Name";
            cmbCategories.ValueMember = "Id";
            cmbCategories.Enabled = false;

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

        /// <summary>
        /// Bind the provider selector to the full provider list preceded by a placeholder entry,
        /// temporarily detaching the selection-changed handler to avoid spurious events.
        /// </summary>
        private void LoadProviders()
        {
            cmbChooseProvider.SelectedIndexChanged -= cmbChooseProvider_SelectedIndexChanged;

            var sourceProviders = new List<Provider>
            {
                new Provider { Name = lang.Translate("Select a Provider") }
            };
            sourceProviders.AddRange(providers);

            cmbChooseProvider.DataSource = null;
            cmbChooseProvider.DataSource = sourceProviders;
            cmbChooseProvider.DisplayMember = "Name";
            cmbChooseProvider.ValueMember = "Id";
            cmbChooseProvider.SelectedIndex = 0;

            cmbChooseProvider.SelectedIndexChanged += cmbChooseProvider_SelectedIndexChanged;
        }

        /// <summary>
        /// Disable all editable provider fields until a provider is selected.
        /// </summary>
        private void InitialConfig()
        {
            txtName.Enabled = false;
            txtCompanyName.Enabled = false;
            txtCellNumber.Enabled = false;
            txtEmail.Enabled = false;
            txtCUIT.Enabled = false;
            cmbCategories.Enabled = false;
        }

        /// <summary>
        /// Filter the provider selector by the chosen category, or reload all providers when the
        /// "All categories" option is selected.
        /// </summary>
        private void cmbChooseCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChooseCategory.SelectedIndex == 0)
            {
                LoadProviders();
            }
             else
            {
                int selectedCategoryId = (int)cmbChooseCategory.SelectedValue;
                var filteredProviders = providers.Where(p => p.Category != null && p.Category.Id == selectedCategoryId).ToList();
                cmbChooseProvider.SelectedIndexChanged -= cmbChooseProvider_SelectedIndexChanged;
                var sourceProviders = new List<Provider>
                {
                    new Provider { Name = lang.Translate("Select a Provider") }
                };
                sourceProviders.AddRange(filteredProviders);
                cmbChooseProvider.DataSource = null;
                cmbChooseProvider.DataSource = sourceProviders;
                cmbChooseProvider.DisplayMember = "Name";
                cmbChooseProvider.ValueMember = "Id";
                cmbChooseProvider.SelectedIndex = 0;
                cmbChooseProvider.SelectedIndexChanged += cmbChooseProvider_SelectedIndexChanged;
            }
        }

        /// <summary>
        /// Load the selected provider's details into the editable fields, reverting to the last
        /// valid selection when the placeholder entry is chosen.
        /// </summary>
        private void cmbChooseProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChooseProvider.SelectedIndex == 0 && _lastValidProviderIndex >= 1)
            {
                cmbChooseProvider.SelectedIndex = _lastValidProviderIndex;
                return;
            }

            if (cmbChooseProvider.SelectedIndex > 0)
                _lastValidProviderIndex = cmbChooseProvider.SelectedIndex;

            Provider selected = cmbChooseProvider.SelectedItem as Provider;
            if (selected != null)
            {
                txtName.Enabled = true;
                txtName.Text = selected.Name;
                txtCompanyName.Enabled = true;
                txtCompanyName.Text = selected.CompanyName;
                txtCellNumber.Enabled = true;
                txtCellNumber.Text = selected.ContactTel;
                txtEmail.Enabled = true;
                txtEmail.Text = selected.Email;
                txtCUIT.Enabled = true;
                txtCUIT.Text = FormatCUIT(selected.CUIT);
                cmbCategories.Enabled = true;
                cmbCategories.SelectedValue = selected.Category.Id;
            }
        }

        /// <summary>
        /// Validate that a provider is selected, apply the edited field values, persist the changes
        /// through the provider service, raise <see cref="ProviderModded"/> and close the form.
        /// </summary>
        private void btnSaveProvider_Click(object sender, EventArgs e)
        {
            Provider providerToMod = cmbChooseProvider.SelectedItem as Provider;
            if (providerToMod == null || providerToMod.Id == Guid.Empty)
            {
                MessageBox.Show(
                    lang.Translate("Please select a provider to modify."),
                    lang.Translate("Warning"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                providerToMod.Name = txtName.Text.Trim();
                providerToMod.CUIT = txtCUIT.Text.Trim().Replace("-", "");
                providerToMod.CompanyName = txtCompanyName.Text.Trim();
                providerToMod.ContactTel = txtCellNumber.Text.Trim();
                providerToMod.Email = txtEmail.Text.Trim();
                providerToMod.Category = categories.FirstOrDefault(c => c.Id == (int)cmbCategories.SelectedValue);

                providerService.Update(providerToMod);
                MessageBox.Show(
                    lang.Translate("Provider modded successfully!"),
                    lang.Translate("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ProviderModded?.Invoke(this, EventArgs.Empty);
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
                Logger.Current.Error($"Error saving new provider: {ex.Message}");
                MessageBox.Show(
                    lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Apply the current language translations to the form title, labels and buttons.
        /// </summary>
        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("Edit Provider");
            label1.Text = lang.Translate("Name");
            label2.Text = lang.Translate("CUIT");
            label3.Text = lang.Translate("Company name");
            label4.Text = lang.Translate("Cell Phone Number");
            label5.Text = lang.Translate("Email");
            label6.Text = lang.Translate("Category");
            btnSaveProvider.Text = lang.Translate("Save");
        }

        /// <summary>
        /// Format an 11-digit CUIT string into the dashed "XX-XXXXXXXX-X" presentation form.
        /// </summary>
        /// <param name="cuit">The raw CUIT value to format.</param>
        /// <returns>The dashed CUIT when it has 11 digits; otherwise the original value or an empty string.</returns>
        private string FormatCUIT(string cuit)
        {
            if (cuit?.Length == 11)
                return $"{cuit[..2]}-{cuit[2..10]}-{cuit[10]}";
            return cuit ?? string.Empty;
        }
    }
}
