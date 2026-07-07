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
    public partial class newProviderForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;
        ProviderService providerService = ProviderService.Instance();

        List<ItemsCategory> categories;
        public event EventHandler ProviderCreated;

        /// <summary>
        /// Initializes the form with the selectable categories and binds them to the category combo.
        /// </summary>
        /// <param name="categories">The categories a provider can be assigned to.</param>
        public newProviderForm(List<ItemsCategory> categories)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.categories = categories;
            LoadCategories();
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

        /// <summary>
        /// Applies the current language translations to the form title, labels and buttons.
        /// </summary>
        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("Create Provider");
            label1.Text = lang.Translate("Name");
            label2.Text = lang.Translate("CUIT");
            label3.Text = lang.Translate("Company name");
            label4.Text = lang.Translate("Cell Phone Number");
            label5.Text = lang.Translate("Email");
            label6.Text = lang.Translate("Category");
            btnSaveProvider.Text = lang.Translate("Save");
        }

        /// <summary>
        /// Builds a new provider from the form fields, persists it, raises the ProviderCreated event and closes the form.
        /// </summary>
        private void btnSaveProvider_Click(object sender, EventArgs e)
        {
            try
            {
                Provider newProvider = new Provider
                {
                    Name = txtName.Text.Trim(),
                    CUIT = txtCUIT.Text.Trim().Replace("-", ""),
                    CompanyName = txtCompanyName.Text.Trim(),
                    ContactTel = txtCellNumber.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Category = categories.FirstOrDefault(c => c.Id == (int)cmbCategories.SelectedValue)
                };

                providerService.Insert(newProvider);
                MessageBox.Show(
                    lang.Translate("Provider added successfully!"),
                    lang.Translate("Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ProviderCreated?.Invoke(this, EventArgs.Empty);
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
    }
}
