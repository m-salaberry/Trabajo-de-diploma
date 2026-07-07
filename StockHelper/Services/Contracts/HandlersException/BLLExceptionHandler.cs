using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.CustomsException
{
    public class BLLExceptionHandler : Exception
    {
        private const string prefix = "BLL Exception: ";

        /// <summary>
        /// Initializes the handler with the message of the BLL exception to be logged.
        /// </summary>
        /// <param name="message">The message describing the BLL error.</param>
        public BLLExceptionHandler(string message) : base(message) { }

        /// <summary>
        /// Initializes the handler with the message and inner exception of the BLL exception to be logged.
        /// </summary>
        /// <param name="message">The message describing the BLL error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public BLLExceptionHandler(string message, Exception innerException) 
            : base(message, innerException) { }

        /// <summary>
        /// This method is used to handle the exception of the BLL layer
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
