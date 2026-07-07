using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Contracts.Logs
{
    /// <summary>
    /// Read-side of the logging subsystem. Reads and parses the log files produced
    /// by <see cref="FileAppender"/> so they can be displayed and filtered in the UI.
    /// </summary>
    public sealed class LogReaderService
    {
        private static LogReaderService _instance = null;

        private readonly string _logDirectory;
        private readonly string _baseFileName;

        // Matches the format written by FileAppender: "2026-07-06 14:30:00 [Info] message"
        private static readonly Regex _lineHeader = new Regex(
            @"^(?<ts>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) \[(?<level>[^\]]+)\] (?<msg>.*)$",
            RegexOptions.Compiled);

        /// <summary>
        /// Initializes the service, resolving the log directory and base file name from configuration.
        /// </summary>
        private LogReaderService()
        {
            _logDirectory = ResolveLogDirectory();
            _baseFileName = ConfigurationManager.AppSettings["logFileName"] ?? "system.log";
        }

        /// <summary>
        /// Gets the singleton instance of the LogReaderService.
        /// </summary>
        public static LogReaderService Instance()
        {
            if (_instance == null)
            {
                _instance = new LogReaderService();
            }
            return _instance;
        }

        /// <summary>
        /// Resolves the log directory using the same rules as <see cref="FileAppender"/>.
        /// </summary>
        private static string ResolveLogDirectory()
        {
            var logDir = ConfigurationManager.AppSettings["logFileDirectory"];

            if (string.IsNullOrWhiteSpace(logDir))
            {
                logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            }

            if (!Path.IsPathRooted(logDir))
            {
                logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logDir);
            }

            return logDir;
        }

        /// <summary>
        /// Reads all log entries matching the given date range and levels.
        /// </summary>
        /// <param name="from">Inclusive lower bound for the entry timestamp.</param>
        /// <param name="to">Inclusive upper bound for the entry timestamp.</param>
        /// <param name="levels">Levels to include. If null or empty, no entries are returned.</param>
        /// <returns>Entries ordered by timestamp (oldest first).</returns>
        public List<LogEntry> GetLogs(DateTime from, DateTime to, IEnumerable<LogLevels> levels)
        {
            var levelSet = levels != null
                ? new HashSet<LogLevels>(levels)
                : new HashSet<LogLevels>();

            if (levelSet.Count == 0)
            {
                return new List<LogEntry>();
            }

            return ReadAllEntries()
                .Where(e => e.Timestamp >= from && e.Timestamp <= to && levelSet.Contains(e.Level))
                .OrderBy(e => e.Timestamp)
                .ToList();
        }

        /// <summary>
        /// Reads and parses every entry from the current log file and its rotated backups.
        /// </summary>
        public List<LogEntry> ReadAllEntries()
        {
            var entries = new List<LogEntry>();

            foreach (var file in GetLogFiles())
            {
                ParseFile(file, entries);
            }

            return entries;
        }

        /// <summary>
        /// Returns the current log file plus any rotated backups (system.log, system.log.1, ...).
        /// </summary>
        private IEnumerable<string> GetLogFiles()
        {
            if (!Directory.Exists(_logDirectory))
            {
                return Enumerable.Empty<string>();
            }

            // Matches the base file and rotation backups produced by FileAppender.
            return Directory
                .EnumerateFiles(_logDirectory, _baseFileName + "*")
                .Where(f =>
                {
                    var name = Path.GetFileName(f);
                    if (name.Equals(_baseFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    // Only accept "<base>.<number>" backups, not unrelated files.
                    var suffix = name.Substring(_baseFileName.Length);
                    return suffix.StartsWith(".") && int.TryParse(suffix.Substring(1), out _);
                });
        }

        /// <summary>
        /// Parses a single log file, joining continuation lines (e.g. exception stack
        /// traces written by <see cref="Logger.LogException"/>) into the preceding entry.
        /// </summary>
        private void ParseFile(string path, List<LogEntry> entries)
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines(path, Encoding.UTF8);
            }
            catch (IOException)
            {
                return; // File in use or unreadable; skip it rather than break the view.
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }

            LogEntry current = null;
            var messageBuilder = new StringBuilder();

            foreach (var line in lines)
            {
                var match = _lineHeader.Match(line);
                if (match.Success && DateTime.TryParse(match.Groups["ts"].Value, out var ts)
                    && Enum.TryParse(match.Groups["level"].Value, out LogLevels level))
                {
                    FlushCurrent(current, messageBuilder, entries);

                    current = new LogEntry(ts, level, null);
                    messageBuilder.Clear();
                    messageBuilder.Append(match.Groups["msg"].Value);
                }
                else if (current != null)
                {
                    // Continuation line of a multiline message (stack trace, etc.).
                    messageBuilder.Append(Environment.NewLine).Append(line);
                }
            }

            FlushCurrent(current, messageBuilder, entries);
        }

        /// <summary>
        /// Finalizes the current entry by assigning its accumulated message and adding it to the result list.
        /// </summary>
        /// <param name="current">The entry being built, or null if there is none.</param>
        /// <param name="messageBuilder">The builder holding the entry's accumulated message text.</param>
        /// <param name="entries">The list the completed entry is added to.</param>
        private static void FlushCurrent(LogEntry current, StringBuilder messageBuilder, List<LogEntry> entries)
        {
            if (current != null)
            {
                current.Message = messageBuilder.ToString();
                entries.Add(current);
            }
        }
    }
}
