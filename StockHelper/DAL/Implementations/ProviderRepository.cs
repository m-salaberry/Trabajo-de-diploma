using DAL.Contracts;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class ProviderRepository : IRepository<Provider, Guid>
    {
        public void Create(Provider entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Provider entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Provider> GetAll()
        {
            throw new NotImplementedException();
        }

        public Provider GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Provider entity)
        {
            throw new NotImplementedException();
        }
    }
}
