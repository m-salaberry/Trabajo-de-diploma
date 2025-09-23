using BLL.Contracts;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Implementations.Repositories;

namespace BLL.Implementations
{
    public class UserService : IUserService
    {
        private UsersRepository _userRepository;

        public UserService()
        {
            _userRepository = new UsersRepository();
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
            _userRepository.Create(entity);
        }

        public void Update(User entity)
        {
            _userRepository.Update(entity);
        }
    }
}
