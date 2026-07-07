using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.CustomsException
{
    public class MySystemException : Exception
    {
        private string layerOrigin;

        /// <summary>
        /// Initializes the exception with a message and the layer of the system it originated from.
        /// </summary>
        /// <param name="message">The message describing the error.</param>
        /// <param name="layer">The layer that raised the exception (for example "UI", "BLL", "DAL" or "Services").</param>
        public MySystemException(string message, string layer) : base(message)
        {
            this.layerOrigin = layer;
        }

        /// <summary>
        /// Initializes the exception with a message, the originating layer and an inner exception.
        /// </summary>
        /// <param name="message">The message describing the error.</param>
        /// <param name="layer">The layer that raised the exception (for example "UI", "BLL", "DAL" or "Services").</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public MySystemException(string message, string layer, Exception innerException) 
            : base(message, innerException)
        {
            this.layerOrigin = layer;
        }

        /// <summary>
        /// This method is used to handle the exception of the system
        /// </summary>
        public void Handler()
        {
            switch (layerOrigin)
            {
                case "UI":
                    new UIExceptionHandler(this.Message).Handler();
                    break;
                case "BLL":
                    new BLLExceptionHandler(this.Message).Handler();
                    break;
                case "DAL":
                    new DALExceptionHandler(this.Message).Handler();
                    break;
                case "Services":
                    Logger.Current.Error($"Services Exception: {this.Message}");
                    if (this.InnerException != null)
                    {
                        Logger.Current.LogException(LogLevels.Error, "Inner exception details", this.InnerException);
                    }
                    break;
                default:
                    Logger.Current.Warning($"System Exception from unknown layer '{layerOrigin}': {this.Message}");
                    if (this.InnerException != null)
                    {
                        Logger.Current.LogException(LogLevels.Warning, "Inner exception details", this.InnerException);
                    }
                    break;
            }
        }
    }
}
