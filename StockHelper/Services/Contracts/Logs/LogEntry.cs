using System;

namespace Services.Contracts.Logs
{
    /// <summary>
    /// Represents a single parsed log record read from the log store.
    /// This is the read-side counterpart of what the appenders write.
    /// </summary>
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public LogLevels Level { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new log entry with the given timestamp, level and message.
        /// </summary>
        /// <param name="timestamp">The time the entry was recorded.</param>
        /// <param name="level">The severity level of the entry.</param>
        /// <param name="message">The message text of the entry.</param>
        public LogEntry(DateTime timestamp, LogLevels level, string message)
        {
            Timestamp = timestamp;
            Level = level;
            Message = message;
        }
    }
}
