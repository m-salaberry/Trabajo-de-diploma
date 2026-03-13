using DAL.Contracts;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementations
{
    public class PurchaseOrderService : GenericBllService<PurchaseOrder, Guid>
    {
        public PurchaseOrderService(IRepository<PurchaseOrder, Guid> repository) : base(repository)
        {
        }
    }
}
