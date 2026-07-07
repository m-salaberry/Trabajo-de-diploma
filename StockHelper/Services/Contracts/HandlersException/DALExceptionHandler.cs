using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.CustomsException
{
    public class DALExceptionHandler : Exception
    {
        private const string prefix = "DAL Exception: ";

        /// <summary>
        /// Initializes the handler with the message of the DAL exception to be logged.
        /// </summary>
        /// <param name="message">The message describing the DAL error.</param>
        public DALExceptionHandler(string message) : base(message) { }

        /// <summary>
        /// Initializes the handler with the message and inner exception of the DAL exception to be logged.
        /// </summary>
        /// <param name="message">The message describing the DAL error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public DALExceptionHandler(string message, Exception innerException) 
            : base(message, innerException) { }

        /// <summary>
        /// This method is used to handle the exception of the DAL layer
        /// </summary>
        public void Handler()
        {
            if (this.InnerException != null)
            {
                Logger.Current.LogException(LogLevels.Error, prefix + this.Message, this.InnerException);
            }
            else
            {
                Logger.Current.Error(prefix + this.Message);
            }
        }
    }
}
