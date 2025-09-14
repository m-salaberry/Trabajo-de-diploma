using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.CustomException
{
    public class LeafComponentException: Exception
    {
        public LeafComponentException() : base("Cannot add or remove child to a leaf component.")
        {
        }
    }
}
