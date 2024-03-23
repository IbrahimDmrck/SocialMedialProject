using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using Entities.DTOs;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Concrete.Entityframework
{
    public class EfOperationClaimDal : EfEntityRepositoryBase<OperationClaim, SocialMediaContext>, IOperationClaimDal
    {
        public List<ClaimDto> GetClaims(Expression<Func<ClaimDto, bool>> filter = null)
        {
            using (var context = new SocialMediaContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims on operationClaim.Id equals userOperationClaim.OperationClaimId
                             join user in context.Users on userOperationClaim.UserId equals user.Id
                             select new ClaimDto
                             {
                                 Id=userOperationClaim.Id,
                                 UserId=user.Id,
                                 UserName=user.FirstName + " " + user.LastName,
                                 OperationClaimId = operationClaim.Id,
                                 ClaimName = operationClaim.Name
                             };
                return filter == null
                    ? result.ToList()
                    : result.Where(filter).ToList();
            }
        }
    }
}
