using Services.Interfaces;
using System;
using System.Configuration;

namespace Services.Contracts.Logs
{
    /// <summary>
    /// Configuration class for Logger initialization
    /// </summary>
    public class LoggerConfiguration
    {
        public bool EnableConsoleLogging { get; set; }
        public bool EnableFileLogging { get; set; }
        public string LogFilePath { get; set; }
        public LogLevels MinimumLogLevel { get; set; }

        public LoggerConfiguration()
        {
            EnableConsoleLogging = false;
            EnableFileLogging = true;
            LogFilePath = "system.log";
            MinimumLogLevel = LogLevels.Info;
        }

        /// <summary>
        /// Creates a configuration for test/development environment
        /// </summary>
        public static LoggerConfiguration CreateTestConfiguration()
        {
            return new LoggerConfiguration
            {
                EnableConsoleLogging = true,
                EnableFileLogging = true,
                LogFilePath = "test-system.log",
                MinimumLogLevel = LogLevels.Debug
            };
        }

        /// <summary>
        /// Creates a configuration for production environment
        /// </summary>
        public static LoggerConfiguration CreateProductionConfiguration()
        {
            return new LoggerConfiguration
            {
                EnableConsoleLogging = false,
                EnableFileLogging = true,
                LogFilePath = "system.log",
                MinimumLogLevel = LogLevels.Info
            };
        }

        /// <summary>
        /// Initializes the Logger instance with the current configuration
        /// </summary>
        public void InitializeLogger()
        {
            var logger = Logger.Current;

            if (EnableConsoleLogging)
            {
                var consoleAppender = new ConsoleAppender();
                logger.AddAppender(consoleAppender);
            }

            if (EnableFileLogging)
            {
                var fileAppender = new FileAppender(LogFilePath);
                logger.AddAppender(fileAppender);
            }

            logger.Info($"Logger initialized - Console: {EnableConsoleLogging}, File: {EnableFileLogging}, MinLevel: {MinimumLogLevel}");
        }
    }
}
