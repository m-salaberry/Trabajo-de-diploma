using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.Logs
{
    public class ConsoleAppender : ILogAppender
    {
        public ConsoleAppender() { }


        public void Append(LogLevels level, string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            try
            {
                // Set the console color based on the log level
                switch (level)
                {
                    case LogLevels.Debug:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogLevels.Info:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogLevels.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogLevels.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogLevels.Fatal:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                }
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{level.ToString().ToUpper()}] {message}");
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }
    }
}
