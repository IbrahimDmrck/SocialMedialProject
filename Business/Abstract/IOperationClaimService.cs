using Core.Entities.Concrete;
using Core.Service;
using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOperationClaimService : IServiceRepository<OperationClaim>
    {
        IDataResult<List<ClaimDto>> GetClaimsById(int claimId);
    }
}
