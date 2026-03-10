using DAL.Contracts;
using DAL.Implementations;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class ProviderService : GenericBllService<Provider, Guid>
    {
        private ProviderService(IRepository<Provider, Guid> repository) : base(repository)
        {
        }

        private static ProviderService _instance = null;

        public static ProviderService Instance()
        {
            if (_instance == null)
            {
                _instance = new ProviderService(new ProviderRepository());
            }
            return _instance;
        }

        /// <summary>
        /// Gets all providers associated with a given category.
        /// </summary>
        public IEnumerable<Provider> GetProvidersByCategory(int categoryId)
        {
            return GetAll().Where(p => p.Category != null && p.Category.Id == categoryId);
        }
    }
}

