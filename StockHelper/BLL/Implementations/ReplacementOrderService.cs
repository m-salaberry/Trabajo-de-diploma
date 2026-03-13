using DAL.Contracts;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class ReplacementOrderService : GenericBllService<ReplacementOrder, Guid>
    {
        public ReplacementOrderService(IRepository<ReplacementOrder, Guid> repository) : base(repository)
        {
        }

    }
}
