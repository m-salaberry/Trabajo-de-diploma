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

        /// <summary>
        /// Initializes a new leaf component with its own read-only, empty children collection.
        /// </summary>
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

        /// <summary>
        /// Not supported on a leaf component; always throws to prevent adding children.
        /// </summary>
        /// <param name="c">The child component that would be added.</param>
        public override void AddChild(Component c)
        {
            throw new LeafComponentException();
        }

        /// <summary>
        /// Not supported on a leaf component; always throws to prevent removing children.
        /// </summary>
        /// <param name="c">The child component that would be removed.</param>
        public override void RemoveChild(Component c)
        {
            throw new LeafComponentException();
        }
    }
}
