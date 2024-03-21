using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.DTOs;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface IOperationClaimDal : IEntityRepository<OperationClaim>
    {
        List<ClaimDto> GetClaims(Expression<Func<ClaimDto, bool>> filter = null);
    }
}
