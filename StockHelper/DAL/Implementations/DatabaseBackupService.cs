using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.Data.SqlClient;
using Services.Contracts.Logs;

namespace DAL.Implementations
{
    /// <summary>
    /// Performs full database backups of the application databases (REQ.006 - Respaldo de Datos).
    /// Designed to be triggered at most once per day (checked at login); a failure never blocks the app.
    /// </summary>
    public class DatabaseBackupService
    {
        private static DatabaseBackupService _instance;

        private readonly string _serverConnectionString;
        private readonly string[] _databases = { "core_db", "iam_db" };

        /// <summary>
        /// Initializes the service, building a connection to the 'master' database from the configured
        /// credentials (BACKUP DATABASE runs from the server/master context).
        /// </summary>
        private DatabaseBackupService()
        {
            string baseConnString = ConfigurationManager.ConnectionStrings["coreDb"].ConnectionString
                .Replace("{sqlUser}", ConfigurationManager.AppSettings["sqlUser"])
                .Replace("{sqlPassword}", ConfigurationManager.AppSettings["sqlPassword"]);

            var builder = new SqlConnectionStringBuilder(baseConnString)
            {
                InitialCatalog = "master"
            };
            _serverConnectionString = builder.ConnectionString;
        }

        /// <summary>
        /// Gets the singleton instance of the backup service.
        /// </summary>
        public static DatabaseBackupService Instance()
        {
            if (_instance == null)
            {
                _instance = new DatabaseBackupService();
            }
            return _instance;
        }

        /// <summary>
        /// Runs a full backup of every application database if no backup has been made in the last 24 hours,
        /// then applies the retention policy. Any error is logged and swallowed so it never blocks login.
        /// </summary>
        public void RunDailyBackupIfNeeded()
        {
            try
            {
                string directory = ResolveBackupDirectory();
                Directory.CreateDirectory(directory);

                if (!IsBackupNeeded(directory))
                {
                    Logger.Current.Debug("[BACKUP] A recent backup already exists; skipping daily backup.");
                    return;
                }

                string stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                foreach (var database in _databases)
                {
                    string path = Path.Combine(directory, $"{database}_{stamp}.bak");
                    BackupDatabase(database, path);
                    Logger.Current.Info($"[BACKUP] Database '{database}' backed up to '{path}'.");
                }

                ApplyRetention(directory);
            }
            catch (Exception ex)
            {
                Logger.Current.LogException(LogLevels.Error, "[BACKUP] Daily backup failed", ex);
            }
        }

        /// <summary>
        /// Resolves the backup directory from configuration ('backupDirectory'), defaulting to a
        /// 'Backups' folder next to the application.
        /// </summary>
        /// <returns>The absolute backup directory path.</returns>
        private string ResolveBackupDirectory()
        {
            string directory = ConfigurationManager.AppSettings["backupDirectory"];

            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            }

            if (!Path.IsPathRooted(directory))
            {
                directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory);
            }

            return directory;
        }

        /// <summary>
        /// Determines whether a backup is due: true when no .bak exists or the newest is older than 24 hours.
        /// </summary>
        /// <param name="directory">The backup directory to inspect.</param>
        /// <returns><c>true</c> if a backup should be taken now; otherwise <c>false</c>.</returns>
        private bool IsBackupNeeded(string directory)
        {
            var files = Directory.GetFiles(directory, "*.bak");
            if (files.Length == 0)
            {
                return true;
            }

            DateTime newest = files.Max(f => File.GetLastWriteTime(f));
            return (DateTime.Now - newest) > TimeSpan.FromHours(24);
        }

        /// <summary>
        /// Executes a full BACKUP DATABASE to the given file path.
        /// </summary>
        /// <param name="databaseName">The database to back up (a controlled internal constant, not user input).</param>
        /// <param name="path">The destination .bak file path.</param>
        private void BackupDatabase(string databaseName, string path)
        {
            string command = $"BACKUP DATABASE [{databaseName}] TO DISK = @path WITH INIT, FORMAT, CHECKSUM";

            using (var connection = new SqlConnection(_serverConnectionString))
            using (var cmd = new SqlCommand(command, connection))
            {
                cmd.CommandTimeout = 300;
                cmd.Parameters.Add(new SqlParameter("@path", path));
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes backup files older than the configured retention window ('backupRetentionDays', default 14).
        /// </summary>
        /// <param name="directory">The backup directory to clean up.</param>
        private void ApplyRetention(string directory)
        {
            int retentionDays = 14;
            if (int.TryParse(ConfigurationManager.AppSettings["backupRetentionDays"], out int parsed) && parsed > 0)
            {
                retentionDays = parsed;
            }

            DateTime cutoff = DateTime.Now.AddDays(-retentionDays);
            foreach (var file in Directory.GetFiles(directory, "*.bak"))
            {
                try
                {
                    if (File.GetLastWriteTime(file) < cutoff)
                    {
                        File.Delete(file);
                        Logger.Current.Debug($"[BACKUP] Removed old backup '{file}'.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Current.Warning($"[BACKUP] Could not delete old backup '{file}': {ex.Message}");
                }
            }
        }
    }
}
