using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.CustomsException
{
    public class MySystemException: Exception
    {
        private string layerOrigin;
        public MySystemException(string message, string layer) : base(message) {
            this.layerOrigin = layer;
        }

        ///<summary>
        /// This method is used to handle the exception of the system
        /// </summary>
        public void Handler()
        {
            switch (layerOrigin)
            {
                case "UI":
                    break;
                case "BLL":
                    new BLLExceptionHandler(this.Message).Handler();
                    break;
                case "DAL":
                    break;
                default:
                    break;
            }
        }

    }
}
