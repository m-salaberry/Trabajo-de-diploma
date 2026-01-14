using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Domain
{
    /// <summary>
    /// Extension methods for User to simplify permission checking.
    /// </summary>
    public static class UserPermissionExtensions
    {
        /// <summary>
        /// Checks if the user has a specific permission.
        /// </summary>
        /// <param name="user">The user to check</param>
        /// <param name="permissionName">The name of the permission (e.g., "CreateUser")</param>
        /// <returns>True if user has the permission, false otherwise</returns>
        public static bool HasPermission(this User user, string permissionName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(permissionName))
            {
                return false;
            }

            return user.Permissions.Any(p => p.HasPermission(permissionName));
        }

        /// <summary>
        /// Checks if the user has ALL of the specified permissions.
        /// </summary>
        /// <param name="user">The user to check</param>
        /// <param name="permissionNames">Array of permission names</param>
        /// <returns>True if user has ALL permissions, false otherwise</returns>
        public static bool HasAllPermissions(this User user, params string[] permissionNames)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (permissionNames == null || permissionNames.Length == 0)
            {
                return false;
            }

            return permissionNames.All(perm => user.HasPermission(perm));
        }

        /// <summary>
        /// Checks if the user has ANY of the specified permissions.
        /// </summary>
        /// <param name="user">The user to check</param>
        /// <param name="permissionNames">Array of permission names</param>
        /// <returns>True if user has AT LEAST ONE permission, false otherwise</returns>
        public static bool HasAnyPermission(this User user, params string[] permissionNames)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (permissionNames == null || permissionNames.Length == 0)
            {
                return false;
            }

            return permissionNames.Any(perm => user.HasPermission(perm));
        }

        /// <summary>
        /// Gets all atomic permissions (Patents) that the user has.
        /// Flattens the hierarchy and returns only leaf permissions.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>List of all atomic permissions</returns>
        public static List<Patent> GetAllAtomicPermissions(this User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var atomicPermissions = new List<Patent>();

            foreach (var permission in user.Permissions)
            {
                CollectAtomicPermissions(permission, atomicPermissions);
            }

            return atomicPermissions.Distinct().ToList();
        }

        /// <summary>
        /// Gets all roles (Families) that the user belongs to.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>List of all roles</returns>
        public static List<Family> GetRoles(this User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.Permissions.OfType<Family>().ToList();
        }

        /// <summary>
        /// Checks if the user has a specific role.
        /// </summary>
        /// <param name="user">The user to check</param>
        /// <param name="roleName">The name of the role (e.g., "Administrator")</param>
        /// <returns>True if user has the role, false otherwise</returns>
        public static bool HasRole(this User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }

            return user.Permissions
                .OfType<Family>()
                .Any(family => family.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets a list of all permission names that the user has.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>List of permission names</returns>
        public static List<string> GetPermissionNames(this User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.GetAllAtomicPermissions()
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();
        }

        /// <summary>
        /// Recursive helper to collect all atomic permissions from the hierarchy.
        /// </summary>
        private static void CollectAtomicPermissions(Component component, List<Patent> result)
        {
            if (component is Patent patent)
            {
                result.Add(patent);
            }
            else if (component is Family family)
            {
                foreach (var child in family.Children)
                {
                    CollectAtomicPermissions(child, result);
                }
            }
        }
    }
}
