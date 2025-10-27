using Services.Contracts.CustomException;
using Services.Contracts.CustomsException;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DAL.Implementations.Repositories
{
    public sealed class LanguageRepository
    {
        #region Singleton
        private readonly static LanguageRepository _instance = new LanguageRepository();

        public static LanguageRepository GetInstance { get { return _instance; } }

        private LanguageRepository() { }
        #endregion

        private static string folderPath = ConfigurationManager.AppSettings["LanguageFolderPath"];
        private static string fileName = ConfigurationManager.AppSettings["LanguageFileName"];
        private static string path = default;

        static LanguageRepository()
        {
            path = Path.Combine(folderPath, fileName);
        }

        public static string Translate(string word)
        {
            try
            {
                string culture = Thread.CurrentThread.CurrentCulture.Name;
                string localPath = $"{path}.{culture}";

                // Verificar si el archivo existe
                if (!File.Exists(localPath))
                {
                    throw new FileNotFoundException($"Translation file not found: {localPath}");
                }

                using (StreamReader sr = new StreamReader(localPath, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                            continue;

                        if (!line.Contains('='))
                            continue;

                        string[] strings = line.Split(new[] { '=' }, 2); 
                        
                        if (strings.Length < 2)
                            continue;

                        string key = strings[0].Trim();
                        string value = strings[1].Trim();

                        if (key.Equals(word, StringComparison.OrdinalIgnoreCase))
                        {
                            return value;
                        }
                    }
                }

                throw new WordNotFoundException();
            }
            catch (WordNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var exep = new MySystemException(ex.Message, "Service");
                exep.Handler();
                return word;
            }
        }

        public void AddDatakey(string word)
        {
            try
            {
                string culture = Thread.CurrentThread.CurrentCulture.Name;
                string localPath = $"{path}.{culture}";
                using (StreamWriter sw = new StreamWriter(localPath, true))
                {
                    sw.WriteLine($"{word}={word}");
                }
            }
            catch (Exception ex)
            {
                var exep = new MySystemException(ex.Message, "Service");
                exep.Handler();
            }
        }


    }
}
