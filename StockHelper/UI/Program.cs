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
                // Initialize Logger based on environment
                InitializeLogger();

                Logger.Current.Info("StockHelper application starting...");

                // Initialize WinForms application
                ApplicationConfiguration.Initialize();

                Logger.Current.Info("ApplicationConfiguration initialized successfully");

                // Run the main form
                Application.Run(new frmLogIn());

                Logger.Current.Info("StockHelper application shutting down normally");
            }
            catch (Exception ex)
            {
                // Log fatal errors
                Logger.Current.LogException(LogLevels.Fatal, "Fatal error during application startup", ex);

                MessageBox.Show(
                    $"A fatal error occurred during application startup:\n\n{ex.Message}\n\nPlease check the log files for more details.",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                // Cleanup logger resources
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
                // Test/Development environment configuration
                NativeMethods.AllocConsole(); // Opens the console for debugging
                config = LoggerConfiguration.CreateTestConfiguration();
                Console.WriteLine("=== TEST ENVIRONMENT ===");
                Console.WriteLine("Console appender enabled for debugging");
            }
            else if (NativeMethods.productionEnvironment)
            {
                // Production environment configuration
                config = LoggerConfiguration.CreateProductionConfiguration();
            }
            else
            {
                // Default configuration (file only)
                config = new LoggerConfiguration
                {
                    EnableConsoleLogging = false,
                    EnableFileLogging = true,
                    LogFilePath = "system.log",
                    MinimumLogLevel = LogLevels.Info
                };
            }

            // Initialize logger with the selected configuration
            config.InitializeLogger();
        }

        
    }
}