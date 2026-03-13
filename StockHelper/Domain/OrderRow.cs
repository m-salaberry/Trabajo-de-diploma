using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class OrderRow
    {
        public Guid Id { get; private set; }
        public ReplacementOrder ReplacementOrder { get; set; }
        public Item Item { get; set; }
        public decimal Quantity { get; set; }
    }
}
