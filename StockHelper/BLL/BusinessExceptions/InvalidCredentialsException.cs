using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Contracts.CustomsException;

namespace BLL.BusinessExceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("The username and/or password entered is not valid")
        {
            new MySystemException(this.Message, "BLL").Handler();
        }
    }
}
