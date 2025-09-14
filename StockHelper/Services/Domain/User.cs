using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class User
    {
        private Guid id;
        private string name;
        private string password;
        private List<Component> _permissions;

        public User()
        {
            _permissions = new List<Component>();
        }

        /// <summary>
        /// Returns the Permissions of the user.
        /// </summary>
        public List<Component> Permissions
        {
            get
            {
                return _permissions;
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
