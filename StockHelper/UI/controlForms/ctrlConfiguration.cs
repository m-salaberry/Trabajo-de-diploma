using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Services.Implementations;
using UI.Implementations;

namespace UI.controlForms
{
    public partial class ctrlConfiguration : TranslatableUserControls
    {
        LanguageService lang = LanguageService.GetInstance;

        public ctrlConfiguration()
        {
            InitializeComponent();
            cmbLangauge.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadAvailableLanguages();
        }

        /// <summary>
        /// Applies the current language translations to all translatable controls in the configuration panel.
        /// </summary>
        public override void ApplyTranslations()
        {
            label1.Text = lang.Translate("System Language:");
            btnSaveConfig.Text = lang.Translate("Save");
        }

        /// <summary>
        /// Populates the language combo box with all available cultures from the I18n folder
        /// and preselects the currently active culture.
        /// </summary>
        private void LoadAvailableLanguages()
        {
            cmbLangauge.Items.Clear();
            var cultures = lang.GetAvailableCultures();
            foreach (var culture in cultures)
            {
                cmbLangauge.Items.Add(culture);
            }
            string currentCulture = lang.GetCurrentCulture();
            int index = cmbLangauge.Items.IndexOf(currentCulture);
            if (index >= 0)
                cmbLangauge.SelectedIndex = index;
        }

        /// <summary>
        /// Handles the Save button click. Validates a language is selected, applies the new culture
        /// via LanguageService, and displays a confirmation message.
        /// </summary>
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            if (cmbLangauge.SelectedItem == null)
            {
                MessageBox.Show(
                    lang.Translate("Please select a language."),
                    lang.Translate("Warning"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string selectedCulture = cmbLangauge.SelectedItem.ToString();
            lang.ChangeCulture(selectedCulture);

            MessageBox.Show(
                lang.Translate("Language changed successfully."),
                lang.Translate("Success"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
