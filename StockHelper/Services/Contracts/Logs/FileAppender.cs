using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace Services.Contracts.Logs
{
    public class FileAppender : ILogAppender
    {
        private readonly string _filePath;
        private readonly LogLevels _minimunLevel = LogLevels.Info;
        private readonly object _lock = new object(); //For synchronization with file access

        public FileAppender(string filePath)
        {
            var logDir = ConfigurationManager.AppSettings["logFileDirectory"];
            if (string.IsNullOrWhiteSpace(logDir))
            {
                logDir = "Logs\\";
            }
            _filePath = Path.Combine(logDir, "system.log");
            string directory = Path.GetDirectoryName(_filePath)!;
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void Append(LogLevels level, string message)
        {
            if (level < _minimunLevel)
            {
                return; // Ignore messages below the minimum level
            }
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";

            lock (_lock) // Ensure thread-safe access to the file
            {
                try
                {
                    File.AppendAllText(_filePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log to console or another fallback mechanism)
                    Console.Error.WriteLine($"ERROR: The log file could not be written {_filePath}. {ex.Message}");
                }
            }
        }
    }
}
