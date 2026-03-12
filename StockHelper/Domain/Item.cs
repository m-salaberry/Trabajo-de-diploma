using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Item
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public Dictionary<string, object> Unit { get; set; } = new Dictionary<string, object>();
        public ItemsCategory Category { get; set; }
        public decimal Stock { get; set; }
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Determines whether the current unit represents an integer value.
        /// </summary>
        /// <returns>true if the unit is classified as an integer; otherwise, false.</returns>
        public bool IsUnitInteger()
        {
            if (Unit.TryGetValue("IsInteger", out var val))
                return Convert.ToBoolean(val);
            return false;
        }
    }
}
