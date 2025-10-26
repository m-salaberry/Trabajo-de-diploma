using Services.Contracts.CustomException;
using Services.Contracts.CustomsException;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
namespace Services.Implementations
{
    public class LoginService
    {
        private UserService _userService;
        public LoginService()
        {
            _userService = UserService.Instance();
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
                new MySystemException(ex.Message, "Services").Handler();
                return false;
            }

        }

    }
}
