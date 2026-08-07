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

        #region Constants
        /// <summary>
        /// Name of the key that holds the culture inside the per-user settings file.
        /// </summary>
        private const string CultureSettingKey = "Culture";
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

                SaveCultureToUserSettings(cultureName);

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
        /// Loads the culture to apply on start-up. The culture saved by the user takes precedence over the
        /// one configured in the application configuration file, which acts as the factory default.
        /// Falls back to "en-US" when both are missing or on error.
        /// </summary>
        private void LoadCultureFromConfig()
        {
            try
            {
                string? cultureName = ReadCultureFromUserSettings();
                string origin = "user settings";

                if (string.IsNullOrWhiteSpace(cultureName))
                {
                    cultureName = ConfigurationManager.AppSettings["Culture"];
                    origin = "config";
                }

                if (string.IsNullOrWhiteSpace(cultureName))
                {
                    cultureName = "en-US";
                    origin = "default";
                    Logger.Current.Info($"No culture configured, using default: {cultureName}");
                }

                SetCultureInternal(cultureName);
                Logger.Current.Info($"Culture loaded from {origin}: {cultureName}");
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
        /// Gets the per-user settings folder (%APPDATA%\StockHelper). It is used instead of the application
        /// folder because the latter is read-only once the application is installed under Program Files.
        /// </summary>
        /// <returns>Absolute path of the settings folder.</returns>
        private static string GetUserSettingsFolderPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "StockHelper");
        }

        /// <summary>
        /// Gets the full path of the per-user settings file (%APPDATA%\StockHelper\settings.ini).
        /// </summary>
        /// <returns>Absolute path of the settings file.</returns>
        private static string GetUserSettingsFilePath()
        {
            return Path.Combine(GetUserSettingsFolderPath(), "settings.ini");
        }

        /// <summary>
        /// Reads the culture previously saved by the user.
        /// </summary>
        /// <returns>The saved culture code, or null when there is no saved preference.</returns>
        private string? ReadCultureFromUserSettings()
        {
            try
            {
                string filePath = GetUserSettingsFilePath();

                if (!File.Exists(filePath))
                {
                    return null;
                }

                foreach (string line in File.ReadAllLines(filePath))
                {
                    string trimmedLine = line.Trim();

                    if (trimmedLine.Length == 0 || trimmedLine.StartsWith("#"))
                    {
                        continue;
                    }

                    int separatorIndex = trimmedLine.IndexOf('=');

                    if (separatorIndex <= 0)
                    {
                        continue;
                    }

                    string key = trimmedLine.Substring(0, separatorIndex).Trim();

                    if (!key.Equals(CultureSettingKey, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    string value = trimmedLine.Substring(separatorIndex + 1).Trim();

                    return string.IsNullOrWhiteSpace(value) ? null : value;
                }
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Warning,
                    "Error reading the user settings file, falling back to the configured culture", ex);
            }

            return null;
        }

        /// <summary>
        /// Persists the specified culture to the per-user settings file. The application configuration file
        /// is not used because, once installed, it lives in a read-only folder (Program Files) and saving
        /// there fails for non-elevated users.
        /// </summary>
        /// <param name="cultureName">Culture code to save (e.g., "es-ES", "en-US").</param>
        private void SaveCultureToUserSettings(string cultureName)
        {
            try
            {
                string filePath = GetUserSettingsFilePath();
                string folderPath = GetUserSettingsFolderPath();

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                List<string> lines = File.Exists(filePath)
                    ? File.ReadAllLines(filePath).ToList()
                    : new List<string>();

                string settingLine = $"{CultureSettingKey}={cultureName}";
                bool replaced = false;

                for (int i = 0; i < lines.Count; i++)
                {
                    string trimmedLine = lines[i].Trim();
                    int separatorIndex = trimmedLine.IndexOf('=');

                    if (trimmedLine.StartsWith("#") || separatorIndex <= 0)
                    {
                        continue;
                    }

                    string key = trimmedLine.Substring(0, separatorIndex).Trim();

                    if (key.Equals(CultureSettingKey, StringComparison.OrdinalIgnoreCase))
                    {
                        lines[i] = settingLine;
                        replaced = true;
                        break;
                    }
                }

                if (!replaced)
                {
                    lines.Add(settingLine);
                }

                File.WriteAllLines(filePath, lines);

                Logger.Current.Debug($"Culture '{cultureName}' saved to user settings file: {filePath}");
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Warning,
                    $"Error saving culture '{cultureName}' to the user settings file", ex);
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
