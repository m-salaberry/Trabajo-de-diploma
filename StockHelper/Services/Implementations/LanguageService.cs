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
    /// <summary>
    /// Service for managing application language and translations.
    /// Provides culture management and translation services.
    /// </summary>
    public sealed class LanguageService
    {
        #region Singleton
        private readonly static LanguageService _instance = new LanguageService();

        public static LanguageService GetInstance => _instance;

        /// <summary>
        /// Initializes the singleton instance by loading the configured culture.
        /// </summary>
        private LanguageService()
        {
            LoadCultureFromConfig();
        }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when the application language/culture changes.
        /// </summary>
        public event EventHandler LanguageChanged;
        #endregion

        #region Public Methods
        /// <summary>
        /// Translates a word/key using the LanguageRepository.
        /// Automatically adds missing keys to the translation file.
        /// </summary>
        /// <param name="word">The word/key to translate</param>
        /// <returns>Translated string or original word if not found</returns>
        public string Translate(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return word;
            }

            string trimmedWord = word.Trim();
            string leading = word.Substring(0, word.Length - word.TrimStart().Length);
            string trailing = word.Substring(word.TrimEnd().Length);

            try
            {
                return leading + LanguageRepository.GetInstance.Translate(trimmedWord) + trailing;
            }
            catch (WordNotFoundException)
            {
                LanguageRepository.GetInstance.AddDatakey(trimmedWord);
                
                Logger.Current.Warning(
                    $"Translation key '{word}' not found in culture '{GetCurrentCulture()}'. " +
                    $"Added to translation file for future translation.");
                
                return word;
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Error, 
                    $"Unexpected error translating '{word}'", ex);
                return word;
            }
        }

        /// <summary>
        /// Changes the application culture/language.
        /// </summary>
        /// <param name="cultureName">Culture code (e.g., "es-ES", "en-US")</param>
        public void ChangeCulture(string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                Logger.Current.Warning("Attempted to change to null or empty culture");
                return;
            }

            try
            {
                CultureInfo culture = new CultureInfo(cultureName);

                if (!LanguageRepository.GetInstance.TranslationFileExists(cultureName))
                {
                    Logger.Current.Warning(
                        $"Translation file not found for culture '{cultureName}'. " +
                        $"Application will use keys as fallback.");
                }

                SetCultureInternal(cultureName);

                SaveCultureToConfig(cultureName);

                OnLanguageChanged();

                Logger.Current.Info($"Culture changed successfully to: {cultureName}");
            }
            catch (CultureNotFoundException ex)
            {
                Logger.Current.LogException(LogLevels.Error, 
                    $"Invalid culture: {cultureName}", ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Error, 
                    $"Error changing culture to {cultureName}", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the current application culture code.
        /// </summary>
        /// <returns>Culture code (e.g., "es-ES")</returns>
        public string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        /// <summary>
        /// Gets the current culture info object.
        /// </summary>
        /// <returns>CultureInfo object</returns>
        public CultureInfo GetCurrentCultureInfo()
        {
            return Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        /// Gets a list of available translation files/cultures.
        /// </summary>
        /// <returns>List of available culture codes</returns>
        public List<string> GetAvailableCultures()
        {
            List<string> cultures = new List<string>();

            try
            {
                string folderPath = ConfigurationManager.AppSettings["LanguageFolderPath"];
                string fileName = ConfigurationManager.AppSettings["LanguageFileName"];

                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I18n");
                }

                if (!Path.IsPathRooted(folderPath))
                {
                    folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderPath);
                }

                if (!Directory.Exists(folderPath))
                {
                    Logger.Current.Warning($"Translation folder not found: {folderPath}");
                    return cultures;
                }

                string searchPattern = $"{fileName}.*";
                string[] files = Directory.GetFiles(folderPath, searchPattern);

                foreach (string file in files)
                {
                    string fileNameOnly = Path.GetFileName(file);
                    int lastDotIndex = fileNameOnly.LastIndexOf('.');
                    
                    if (lastDotIndex > 0 && lastDotIndex < fileNameOnly.Length - 1)
                    {
                        string cultureName = fileNameOnly.Substring(lastDotIndex + 1);
                        
                        try
                        {
                            CultureInfo culture = new CultureInfo(cultureName);
                            cultures.Add(cultureName);
                        }
                        catch (CultureNotFoundException)
                        {
                            Logger.Current.Debug($"Skipping invalid culture file: {fileNameOnly}");
                        }
                    }
                }

                Logger.Current.Debug($"Found {cultures.Count} available translation cultures");
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Error, 
                    "Error getting available cultures", ex);
            }

            return cultures;
        }

        /// <summary>
        /// Clears the translation cache, forcing reload from files.
        /// </summary>
        public void RefreshTranslations()
        {
            LanguageRepository.GetInstance.ClearCache();
            Logger.Current.Info("Translation cache refreshed");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the culture from the application configuration, falling back to "en-US" when missing or on error.
        /// </summary>
        private void LoadCultureFromConfig()
        {
            try
            {
                string cultureName = ConfigurationManager.AppSettings["Culture"];

                if (string.IsNullOrWhiteSpace(cultureName))
                {
                    cultureName = "en-US";
                    Logger.Current.Info($"No culture configured, using default: {cultureName}");
                }

                SetCultureInternal(cultureName);
                Logger.Current.Info($"Culture loaded from config: {cultureName}");
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Error, 
                    "Error loading culture from config, using default (en-US)", ex);
                SetCultureInternal("en-US");
            }
        }

        /// <summary>
        /// Applies the specified culture to the current thread and as the default for new threads.
        /// </summary>
        /// <param name="cultureName">Culture code to apply (e.g., "es-ES", "en-US").</param>
        private void SetCultureInternal(string cultureName)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        /// <summary>
        /// Persists the specified culture to the application configuration file.
        /// </summary>
        /// <param name="cultureName">Culture code to save (e.g., "es-ES", "en-US").</param>
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

                Logger.Current.Debug($"Culture '{cultureName}' saved to config file");
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Warning, 
                    $"Error saving culture '{cultureName}' to config file", ex);
            }
        }

        /// <summary>
        /// Raises the LanguageChanged event, swallowing and logging any exception thrown by subscribers.
        /// </summary>
        private void OnLanguageChanged()
        {
            try
            {
                LanguageChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Error, 
                    "Error in LanguageChanged event handler", ex);
            }
        }
        #endregion
    }
}
