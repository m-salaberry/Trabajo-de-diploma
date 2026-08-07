using Services.Contracts.Logs;
using Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Contracts;

namespace UI.Implementations
{
    public class TranslatableForm: Form, ITranslatable
    {
        /// <summary>
        /// Application icon shared by every form, loaded once from the embedded resource.
        /// Null if the resource is missing, in which case the default icon is kept.
        /// </summary>
        private static readonly Icon? AppIcon = LoadAppIcon();

        /// <summary>
        /// Initializes the form, applies the application icon and subscribes to language change events.
        /// </summary>
        public TranslatableForm()
        {
            if (AppIcon != null)
            {
                this.Icon = AppIcon;
            }

            LanguageService.GetInstance.LanguageChanged += OnLanguageChanged;
            this.Load += (s,e) => ApplyTranslations();
        }

        /// <summary>
        /// Loads the icon embedded in the assembly. The same file is set as the executable icon
        /// through ApplicationIcon, so the window title bars match the shortcut and the taskbar.
        /// </summary>
        /// <returns>The application icon, or null when it cannot be loaded.</returns>
        private static Icon? LoadAppIcon()
        {
            try
            {
                using Stream? stream = typeof(TranslatableForm).Assembly
                    .GetManifestResourceStream("UI.Resources.stockhelper.ico");

                if (stream == null)
                {
                    Logger.Current.Warning("Embedded application icon not found; using the default one.");
                    return null;
                }

                return new Icon(stream);
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Warning, "Error loading the application icon", ex);
                return null;
            }
        }

        /// <summary>
        /// Handles the language changed event by reapplying translations.
        /// </summary>
        private void OnLanguageChanged(object? sender, EventArgs e)
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
