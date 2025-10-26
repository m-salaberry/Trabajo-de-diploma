using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public abstract class Component
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        /// <summary>
        /// List of children components. If the component is a leaf, this list will be empty.
        /// </summary>
        public abstract IList<Component> Children { get; }
        
        /// <summary>
        /// Add a child component to this component. If the component is a leaf, this method will throw an exception.
        /// </summary>
        /// <param name="component"></param>
        public abstract void AddChild(Component component);

        /// <summary>
        /// Remove a child component from this component. If the component is a leaf, this method will throw an exception.
        /// </summary>
        /// <param name="component"></param>
        public abstract void RemoveChild(Component component);

        public bool HasPermission (string permissionName)
        {
            if (this.Name == permissionName)
            {
                return true;
            }

            foreach (var child in Children)
            {
                if (child.HasPermission(permissionName))
                {
                    return true;
                }
            }

            return false;

        }

        public override string ToString()
        {
            return Name;
        }
    }
}
