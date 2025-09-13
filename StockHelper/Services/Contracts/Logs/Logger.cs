using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.Logs
{
    public sealed class Logger
    {
        private readonly static Logger _instance = new Logger();
        private readonly List<ILogAppender> _appenders;

        public static Logger Current
        {
            get
            {
                return _instance;
            }
        }

        private Logger()
        {
            _appenders = new List<ILogAppender>();
        }

        /// <summary>
        /// Adds an appender to the logger.
        /// </summary>
        /// <param name="appender">Appender to be added.</param>
        public void AddAppender(ILogAppender appender)
        {
            if (appender == null)
            {
                throw new ArgumentNullException(nameof(appender), "Appender cannot be null");
            }
            _appenders.Add(appender);
        }

        /// <summary>
        /// Private method to process log messages.
        /// </summary>
        /// <param name="level">Log level of the message.</param>
        /// <param name="message">Message to be logged.</param>
        private void Log(LogLevels level, string message)
        {
            foreach (var appender in _appenders)
            {
                appender.Append(level, message);
            }
        }

        /// <summary>
        /// Convenience methods for logging at different levels.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public void Debug(string message) => Log(LogLevels.Debug, message);
        public void Info(string message) => Log(LogLevels.Info, message);
        public void Warning(string message) => Log(LogLevels.Warning, message);
        public void Error(string message) => Log(LogLevels.Error, message);
        public void Fatal(string message) => Log(LogLevels.Fatal, message);
    }
}
