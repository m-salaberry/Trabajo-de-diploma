using Services.Contracts.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class Patent: Component
    {
        public override IList<Component> Children
        {
            get
            {
                return new List<Component>();
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
