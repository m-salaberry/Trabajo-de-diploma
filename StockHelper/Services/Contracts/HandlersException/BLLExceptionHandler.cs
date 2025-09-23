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
        
        public BLLExceptionHandler(string message):base(message) { }

        ///<summary>
        /// This method is used to handle the exception of the BLL layer
        ///</summary>
        public void Handler()
        {
            Logger.Current.Error(prefix + this.Message);
        }
    }
}
