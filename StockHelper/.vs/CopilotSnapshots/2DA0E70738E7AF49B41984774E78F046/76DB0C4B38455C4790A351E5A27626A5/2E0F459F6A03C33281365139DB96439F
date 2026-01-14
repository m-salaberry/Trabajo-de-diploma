using Services.Contracts.CustomException;
using Services.Contracts.Logs;
using Services.DAL.Implementations.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public sealed class LanguageService
    {
        private readonly static LanguageService _instace = new LanguageService();

        public static LanguageService GetInstance
        {
            get
            {
                return _instace;
            }
        }

        public event EventHandler LanguageChanged;

        private LanguageService()
        {
            LoadCultureFromConfig();
        }

        /// <summary>
        /// Method to translate a word using the LanguageRepository.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string Translate(string word)
        {
            try
            {
                return LanguageRepository.Translate(word);
            }
            catch (WordNotFoundException ex)
            {
                LanguageRepository.GetInstance.AddDatakey(word);
                Logger.Current.Warning($"The word '{word}' was not found in the language file. Added to the file for future translation.");
                return word; // Return the original word if not found
            }
        }

        private void LoadCultureFromConfig()
        {
            try
            {
                string cultureName = ConfigurationManager.AppSettings["Culture"] ?? "en-US";
                SetCultureInternal(cultureName);
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error loading culture from config: {ex.Message}");
            }
        }

        public void ChangeCulture(string cultureName)
        {
            try
            {
                SetCultureInternal(cultureName);
                SaveCultureToConfig(cultureName);

                // Notify subscribers about the language change
                LanguageChanged?.Invoke(this, EventArgs.Empty);

                Logger.Current.Info($"Culture changed to: {cultureName}");
            }
            catch (CultureNotFoundException ex)
            {
                Logger.Current.Error($"Invalid culture: {cultureName}. {ex.Message}");
            }
        }

        private void SetCultureInternal(string cultureName)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private void SaveCultureToConfig(string cultureName)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (config.AppSettings.Settings["Culture"] == null)
                {
                    config.AppSettings.Settings.Add("Culture", cultureName);
                }
                else
                {
                    config.AppSettings.Settings["Culture"].Value = cultureName;
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                Logger.Current.Error($"Error saving culture to config: {ex.Message}");
            }
        }

        public string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }
    }
}
