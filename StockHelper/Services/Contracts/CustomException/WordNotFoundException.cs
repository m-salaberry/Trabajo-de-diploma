using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.CustomException
{
    /// <summary>
    /// Exception thrown when a translation word/key is not found.
    /// </summary>
    public class WordNotFoundException : Exception
    {
        /// <summary>
        /// Initializes the exception with a default "Word not found" message and translation-related metadata.
        /// </summary>
        public WordNotFoundException() : base("Word not found")
        {
            this.Source = "LanguageRepository";
            this.HelpLink = "Check translation files";
        }

        /// <summary>
        /// Initializes the exception with a custom message and translation-related metadata.
        /// </summary>
        /// <param name="message">The message describing the missing word.</param>
        public WordNotFoundException(string message) : base(message)
        {
            this.Source = "LanguageRepository";
            this.HelpLink = "Check translation files";
        }

        /// <summary>
        /// Initializes the exception with a custom message, an inner exception and translation-related metadata.
        /// </summary>
        /// <param name="message">The message describing the missing word.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public WordNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
            this.Source = "LanguageRepository";
            this.HelpLink = "Check translation files";
        }
    }
}
