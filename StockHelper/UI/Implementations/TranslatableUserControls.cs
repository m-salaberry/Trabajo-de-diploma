using Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Contracts;

namespace UI.Implementations
{
    public class TranslatableUserControls: UserControl, ITranslatable
    {
        public TranslatableUserControls()
        {
            LanguageService.GetInstance.LanguageChanged += OnLanguageChanged;

            this.Load += (s, e) => ApplyTranslations();
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            ApplyTranslations();
        }

        public virtual void ApplyTranslations()
        {
            // Override in derived classes to apply translations to controls
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                LanguageService.GetInstance.LanguageChanged -= OnLanguageChanged;
            }
            base.Dispose(disposing);
        }
    }
}
