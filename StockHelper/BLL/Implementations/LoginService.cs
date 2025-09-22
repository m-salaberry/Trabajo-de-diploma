using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain;
using BLL.BusinessExceptions;
using Services.Contracts.CustomsException;

namespace BLL.Implementations
{
    public class LoginService
    {
        private UserService _userService;
        public LoginService()
        {
            _userService = new UserService();
        }
        public bool Authenticate(string username, string password)
        {
            try
            {
                User dbUser = _userService.GetByName(username);
                if (dbUser.Name != username || dbUser.Password != password)
                {
                    throw new InvalidCredentialsException();
                }
                return true;
            }
            catch (InvalidCredentialsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                new MySystemException(ex.Message, "BLL").Handler();
                return false;
            }

        }

    }
}
