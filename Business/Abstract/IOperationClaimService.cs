using Core.Entities.Concrete;
using Core.Service;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOperationClaimService : IServiceRepository<OperationClaim>
    {
    }
}
