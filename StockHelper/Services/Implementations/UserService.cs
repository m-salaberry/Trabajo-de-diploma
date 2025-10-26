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
            if (this.Exists(entity.Id))
            {
                _userRepository.Create(entity);
            }
            else
            {
                throw new MySystemException("User already exists", "BLL");
            }
        }

        public void Update(User entity)
        {
            _userRepository.Update(entity);
        }
    }
}
