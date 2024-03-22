using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;

namespace DataAccess.Concrete.Entityframework
{
    public class EfUserOperationClaimDal : EfEntityRepositoryBase<UserOperationClaim, SocialMediaContext>, IUserOperationClaimDal
    {
    }
    
}
