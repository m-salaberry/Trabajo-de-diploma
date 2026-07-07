using Services.Domain;
using System;
using Services.Contracts.Logs;
using System.Windows.Forms;

namespace UI
{
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        internal static extern bool AllocConsole();

        internal static bool testEnvironment = false;      
        internal static bool productionEnvironment = true;  
    }

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                InitializeLogger();

                Logger.Current.Info("StockHelper application starting...");

                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("STOCK_HELPER_SECRET_KEY")))
                {
                    Logger.Current.Fatal(
                        "STOCK_HELPER_SECRET_KEY environment variable is not set. Sensitive data cannot be read or written.");
                    MessageBox.Show(
                        "The encryption key (STOCK_HELPER_SECRET_KEY) is not configured on this machine.\n\n" +
                        "The application cannot start because sensitive data cannot be read or written without it.\n" +
                        "Please set the STOCK_HELPER_SECRET_KEY environment variable and try again.",
                        "Configuration Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                ApplicationConfiguration.Initialize();

                Logger.Current.Info("ApplicationConfiguration initialized successfully");

                Application.Run(new frmLogIn());

                Logger.Current.Info("StockHelper application shutting down normally");
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Fatal, "Fatal error during application startup", ex);

                MessageBox.Show(
                    $"A fatal error occurred during application startup:\n\n{ex.Message}\n\nPlease check the log files for more details.",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Logger.Current.Info("Application terminated");
            }
        }

        /// <summary>
        /// Initializes the logger based on the current environment.
        /// </summary>
        private static void InitializeLogger()
        {
            LoggerConfiguration config;

            if (NativeMethods.testEnvironment)
            {
                NativeMethods.AllocConsole();
                config = LoggerConfiguration.CreateTestConfiguration();
                Console.WriteLine("=== TEST ENVIRONMENT ===");
                Console.WriteLine("Console appender enabled for debugging");
            }
            else if (NativeMethods.productionEnvironment)
            {
                config = LoggerConfiguration.CreateProductionConfiguration();
            }
            else
            {
                config = new LoggerConfiguration
                {
                    EnableConsoleLogging = false,
                    EnableFileLogging = true,
                    LogFilePath = "system.log",
                    MinimumLogLevel = LogLevels.Info
                };
            }

            config.InitializeLogger();
        }

        
    }
}