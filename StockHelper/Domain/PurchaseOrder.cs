using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PurchaseOrder
    {
        public Guid Id { get; private set; }
        public ReplacementOrder ReplacementOrder { get; set; }
        public string Status { get; set; }
        public string BillFilePath { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
