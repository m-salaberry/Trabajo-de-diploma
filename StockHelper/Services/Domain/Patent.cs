using Services.Contracts.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    /// <summary>
    /// Represents a leaf component in the Composite pattern (cannot have children).
    /// Used to define atomic permissions that cannot be subdivided.
    /// </summary>
    public class Patent : Component
    {
        // Prevents shared state between instances
        private readonly IList<Component> _emptyChildren;

        public Patent()
        {
            // Create a readonly empty collection for each instance
            _emptyChildren = new List<Component>().AsReadOnly();
        }

        public override IList<Component> Children
        {
            get
            {
                return _emptyChildren;
            }
        }

        public override void AddChild(Component c)
        {
            throw new LeafComponentException();
        }

        public override void RemoveChild(Component c)
        {
            throw new LeafComponentException();
        }
    }
}
