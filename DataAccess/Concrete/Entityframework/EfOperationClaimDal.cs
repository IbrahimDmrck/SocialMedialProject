using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;

namespace DataAccess.Concrete.Entityframework
{
    public class EfOperationClaimDal : EfEntityRepositoryBase<OperationClaim, SocialMediaContext>, IOperationClaimDal
    {
    }
}
