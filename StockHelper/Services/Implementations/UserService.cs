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

        private UserService()
        {
            _userRepository = new UsersRepository();
        }

        public static UserService Instance()
        {
            if (_instance == null)
            {
                _instance = new UserService();
            }
            return _instance;
        }

        public void Delete(Guid id)
        {
            User userToDelete = _userRepository.GetById<User>(id);
            if (userToDelete == null)
            {
                throw new MySystemException("User not found", "BLL");
            }
            _userRepository.DeleteRelationBetweenUserAndFamily(userToDelete, PermissionService.Instance().GetFamilyByName(userToDelete.Role));
            _userRepository.Delete(userToDelete);
        }

        public bool Exists(Guid id)
        {
            if (_userRepository.GetById<User>(id) != null)
            {
                return true;
            }
            return false;
        }

        public bool ExistsByName(string name)
        {
            if (_userRepository.GetByName(name) != null)
            {
                return true;
            }
            return false;
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll<User>().ToList();
        }

        public List<User> GetAllActive()
        {
            return _userRepository.GetAllActive<User>().ToList();
        }

        public User GetById(Guid id)
        {
            return _userRepository.GetById<User>(id);
        }

        public User GetByName(string name)
        {
            return _userRepository.GetByName(name);
        }

        public void Insert(User entity)
        {
            if (ExistsByName(entity.Name))
            {
                throw new MySystemException("User already exists", "BLL");
                
            }
            entity.Id = GenerateUniqueGuid();
            _userRepository.Create(entity);
            Family family = PermissionService.Instance().GetFamilyByName(entity.Role);
            _userRepository.SaveRelatedFamilyOfUser(entity, family);
        }

        public void Update(User entity)
        {
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
