using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class Family: Component
    {
        private IList<Component> _children;
        /// <summary>
        /// Initializes a new composite component with an empty children collection.
        /// </summary>
        public Family()
        {
            _children = new List<Component>();
        }
        public override IList<Component> Children
        {
            get
            {
                return _children.AsReadOnly();
            }
        }
        /// <summary>
        /// Adds a child component to this composite.
        /// </summary>
        /// <param name="c">The child component to add.</param>
        public override void AddChild(Component c)
        {
            _children.Add(c);
        }
        /// <summary>
        /// Removes a child component from this composite.
        /// </summary>
        /// <param name="c">The child component to remove.</param>
        public override void RemoveChild(Component c)
        {
            _children.Remove(c);
        }

    }
}
