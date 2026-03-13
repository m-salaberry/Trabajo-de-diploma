using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public static class PurchaseOrderStatus
    {
        public const string SentToProvider = "Sent to provider";
        public const string Cancelled = "Order cancelled";
        public const string BillReceived = "Bill received";
    }
}
