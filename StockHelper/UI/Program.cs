using System;
namespace UI
{
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        internal static extern bool AllocConsole();

        internal static bool testEnvironment = true;
    }
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (NativeMethods.testEnvironment)
            {
                NativeMethods.AllocConsole(); // Opens the console
                Console.WriteLine("Test environment");
            }
            Console.WriteLine("StockHelper initialized!");
            ApplicationConfiguration.Initialize();
            Application.Run(new frmLogIn());
            
        }
    }
}