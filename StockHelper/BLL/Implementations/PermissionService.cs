using BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain;
using DAL.Implementations.Repositories;

namespace BLL.Implementations
{
    public class PermissionService : IGenericService<Component>
    {
        private PermissionsRepository _permsRepository;
        public PermissionService()
        {
            _permsRepository = new PermissionsRepository();
        }
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Component> GetAll()
        {
            return _permsRepository.GetAll<Component>().ToList();
        }

        public Component GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Component entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Component entity)
        {
            throw new NotImplementedException();
        }
    }
}
