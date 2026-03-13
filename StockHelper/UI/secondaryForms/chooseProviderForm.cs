using Domain;
using Services.Contracts.CustomsException;
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
    public partial class chooseProviderForm : TranslatableForm
    {
        LanguageService lang = LanguageService.GetInstance;

        List<Provider> providers;
        public event EventHandler<Provider> OnProviderSelected;

        /// <summary>
        /// Initializes the provider selection form with the given provider list.
        /// </summary>
        public chooseProviderForm(List<Provider> providers)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.providers = providers;
            LoadProviders();
        }

        /// <summary>
        /// Applies translations to form controls.
        /// </summary>
        public override void ApplyTranslations()
        {
            this.Text = lang.Translate("Choose a Provider");
            btnSelectProvider.Text = lang.Translate("Select");
        }

        /// <summary>
        /// Populates the provider list box.
        /// </summary>
        private void LoadProviders()
        {
            lstbxProviders.Items.Clear();
            foreach (var provider in providers)
            {
                lstbxProviders.Items.Add(provider);
            }
            lstbxProviders.DisplayMember = "Name";
        }

        /// <summary>
        /// Handles provider selection and raises the OnProviderSelected event.
        /// </summary>
        private void btnSelectProvider_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstbxProviders.SelectedItem is not Provider selectedProvider)
                {
                    MessageBox.Show(
                        lang.Translate("Please select a provider."),
                        lang.Translate("Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                OnProviderSelected?.Invoke(this, selectedProvider);
                this.Close();
            }
            catch (MySystemException ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ex.Handler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(lang.Translate("An error occurred: ") + ex.Message,
                    lang.Translate("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
