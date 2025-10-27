using Services.Contracts.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.CustomException
{
    public class WordNotFoundException: Exception
    {
        public WordNotFoundException(): base("Word not found")
        {
            this.Source = "?";
            this.HelpLink = "?";
            Logger.Current.Fatal(this.Message);
        }
    }
}
