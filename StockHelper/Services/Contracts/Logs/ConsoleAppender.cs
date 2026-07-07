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
        /// <summary>
        /// Initializes a new console appender.
        /// </summary>
        public ConsoleAppender() { }


        /// <summary>
        /// Writes a timestamped log line to the console, coloring the output according to the level
        /// and restoring the original console color afterwards.
        /// </summary>
        /// <param name="level">The severity level of the message.</param>
        /// <param name="message">The message to write.</param>
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
