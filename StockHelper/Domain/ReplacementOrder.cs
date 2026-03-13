using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ReplacementOrder
    {
        public Guid Id { get; private set; }
        public string ReplacementOrderNumber { get; set; }
        public Provider Provider { get; set; }
        public List<OrderRow> OrderRows { get; set; }

        public ReplacementOrder(Provider provider)
        {
            Provider = provider;
            OrderRows = new List<OrderRow>();
        }

        public string GenerateReplacementOrderNumber(int sequentialNumber)
        {
            string cuitBody = Provider.CUIT.Replace("-", "");
            if (cuitBody.Length > 2)
                cuitBody = cuitBody.Substring(2, cuitBody.Length - 3);

            return $"REP-{DateTime.Now:yyyyMM}-{cuitBody}-{sequentialNumber:D3}";
        }
    }
}
