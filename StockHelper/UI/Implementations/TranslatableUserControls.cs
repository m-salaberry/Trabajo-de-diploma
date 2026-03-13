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
        /// <summary>
        /// Initializes the control and subscribes to language change events.
        /// </summary>
        public TranslatableUserControls()
        {
            LanguageService.GetInstance.LanguageChanged += OnLanguageChanged;

            this.Load += (s, e) => ApplyTranslations();
        }

        /// <summary>
        /// Handles the language changed event by reapplying translations.
        /// </summary>
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            ApplyTranslations();
        }

        /// <summary>
        /// Applies translations to all controls. Override in derived classes.
        /// </summary>
        public virtual void ApplyTranslations()
        {
            // Override in derived classes to apply translations to controls
        }

        /// <summary>
        /// Unsubscribes from language change events and disposes resources.
        /// </summary>
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
