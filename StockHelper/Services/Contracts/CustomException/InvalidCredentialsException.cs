using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Contracts.CustomsException;

namespace Services.Contracts.CustomException
{
    public class InvalidCredentialsException : Exception
    {
        /// <summary>
        /// Initializes the exception with a default invalid-credentials message and routes it through the system exception handler.
        /// </summary>
        public InvalidCredentialsException() : base("The username and/or password entered is not valid")
        {
            new MySystemException(this.Message, "Services").Handler();
        }
    }
}
