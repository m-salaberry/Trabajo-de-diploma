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
        public Family()
        {
            _children = new List<Component>();
        }
        public override IList<Component> Children
        {
            get
            {
                return _children;
            }
        }
        public override void AddChild(Component c)
        {
            _children.Add(c);
        }
        public override void RemoveChild(Component c)
        {
            _children.Remove(c);
        }

    }
}
