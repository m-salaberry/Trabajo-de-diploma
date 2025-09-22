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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public User GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetByName(string name)
        {
            return _userRepository.GetByName(name);
        }

        public void Insert(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
