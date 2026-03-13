using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class DetailProduct
    {
        public int Id { get; private set; }
        public Item Item { get; set; }
        public decimal QuantityToConsume { get; set; }
    }
}
