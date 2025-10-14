using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain;

namespace Services.Contracts
{
    public interface IUserService: IGenericService<User>
    {
        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        User GetByName(string name);
        
    }
}
