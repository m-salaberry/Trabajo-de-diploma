using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Provider
    {
        public Guid Id { get; private set; }
        public long CUIT { get; set; }
        public string Name { get; set; }
        public ItemsCategory Category { get; set; }
        public string CompanyName { get; set; }
        public string ContactTel { get; set; }
        public string Email { get; set; }

    }
}
