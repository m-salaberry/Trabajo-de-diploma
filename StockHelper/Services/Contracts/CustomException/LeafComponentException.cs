using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts.CustomException
{
    public class LeafComponentException: Exception
    {
        /// <summary>
        /// Initializes the exception with a default message indicating a leaf component cannot have children.
        /// </summary>
        public LeafComponentException() : base("Cannot add or remove child to a leaf component.")
        {
        }
    }
}
