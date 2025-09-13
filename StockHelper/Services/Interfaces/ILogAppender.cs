using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILogAppender
    {
        /// <summary>
        /// Method to append log messages to the console.
        /// </summary>
        /// <param name="level">Log level of the message.</param>
        /// <param name="message">Message to be logged.</param>
        void Append(LogLevels level, string message);
    }
}
