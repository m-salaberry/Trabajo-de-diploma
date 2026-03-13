using Services.Contracts;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.DAL.Implementations.Repositories;
using Services.Contracts.CustomsException;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private UsersRepository _userRepository;
        private static UserService _instance = null;

        /// <summary>
        /// Private constructor to enforce Singleton pattern.
        /// </summary>
        private UserService()
        {
            _userRepository = new UsersRepository();
        }

        /// <summary>
        /// Gets the singleton instance of UserService.
        /// </summary>
        public static UserService Instance()
        {
            if (_instance == null)
            {
                _instance = new UserService();
            }
            return _instance;
        }

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        public void Delete(Guid id)
        {
            User userToDelete = _userRepository.GetById(id);
            if (userToDelete == null)
            {
                throw new MySystemException("User not found", "BLL");
            }
            _userRepository.DeleteRelationBetweenUserAndFamily(userToDelete, PermissionService.Instance().GetFamilyByName(userToDelete.Role));
            _userRepository.Delete(userToDelete);
        }

        /// <summary>
        /// Determines whether a user with the specified Id exists.
        /// </summary>
        public bool Exists(Guid id)
        {
            return _userRepository.GetById(id) != null;
        }

        /// <summary>
        /// Determines whether a user with the specified name exists.
        /// </summary>
        /// <param name="name">The name of the user to locate. Cannot be null.</param>
        /// <returns><see langword="true"/> if a user with the specified name exists; otherwise, <see langword="false"/>.</returns>
        public bool ExistsByName(string name)
        {
            return _userRepository.GetByName(name) != null;
        }

        /// <summary>
        /// Retrieves all users from the system.
        /// </summary>
        public List<User> GetAll()
        {
            return _userRepository.GetAll().ToList();
        }

        /// <summary>
        /// Retrieves a list of all active users.
        /// </summary>
        /// <returns>A list of <see cref="User"/> objects representing users who are currently active. The list is empty if no
        /// active users are found.</returns>
        public List<User> GetAllActive()
        {
            return _userRepository.GetAllActive().ToList();
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        public User GetById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        public User GetByName(string name)
        {
            return _userRepository.GetByName(name);
        }

        /// <summary>
        /// Inserts a new user into the system with hashed password and role assignment.
        /// </summary>
        public void Insert(User entity)
        {
            if (ExistsByName(entity.Name))
            {
                throw new MySystemException("User already exists", "BLL");
                
            }
            
            entity.Id = GenerateUniqueGuid();
            entity.Password = CryptographyService.HashMd5(entity.Password);
            _userRepository.Create(entity);
            Family family = PermissionService.Instance().GetFamilyByName(entity.Role);
            _userRepository.SaveRelatedFamilyOfUser(entity, family);
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        public void Update(User entity)
        {

            entity.Password = CryptographyService.HashMd5(entity.Password);
            _userRepository.Update(entity);
        }

        /// <summary>
        /// Generates a unique GUID that doesn't exist in the database.
        /// While GUID collisions are extremely rare, this ensures database integrity.
        /// </summary>
        /// <returns>A unique GUID</returns>
        private Guid GenerateUniqueGuid()
        {
            Guid newGuid;
            int maxAttempts = 10; // Límite de intentos por seguridad
            int attempts = 0;

            do
            {
                newGuid = Guid.NewGuid();
                attempts++;

                if (attempts >= maxAttempts)
                {
                    throw new MySystemException(
                        "Unable to generate unique GUID after multiple attempts",
                        "BLL");
                }
            }
            while (Exists(newGuid));

            return newGuid;
        }
    }
}
