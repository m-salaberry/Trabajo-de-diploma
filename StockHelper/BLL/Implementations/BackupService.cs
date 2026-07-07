using DAL.Implementations;

namespace BLL.Implementations
{
    /// <summary>
    /// Business-layer entry point for database backups (REQ.006). Delegates to the DAL backup service.
    /// </summary>
    public class BackupService
    {
        private static BackupService _instance;

        /// <summary>
        /// Gets the singleton instance of the backup service.
        /// </summary>
        public static BackupService Instance()
        {
            if (_instance == null)
            {
                _instance = new BackupService();
            }
            return _instance;
        }

        /// <summary>
        /// Triggers a full daily backup of the application databases when one is due (once per 24 hours).
        /// Safe to call on every login: it is a no-op if a recent backup already exists and never throws.
        /// </summary>
        public void RunDailyBackupIfNeeded()
        {
            DatabaseBackupService.Instance().RunDailyBackupIfNeeded();
        }
    }
}
