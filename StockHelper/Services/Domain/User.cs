using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    /// <summary>
    /// Represents a system user with authentication and permission data.
    /// </summary>
    public class User
    {
        private Guid id;
        private string name;
        private string password;
        private List<Component> _permissions;
        private string role;
        private bool isActive;

        /// <summary>
        /// Initializes a new user with an empty permissions collection.
        /// </summary>
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

        public Guid Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Password { get => password; set => password = value; }
        public string Role { get => role; set => role = value; }
        public bool IsActive { get => isActive; set => isActive = value; }

        /// <summary>
        /// Returns the user's name as its string representation.
        /// </summary>
        /// <returns>The user's name.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
