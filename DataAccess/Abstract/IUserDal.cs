

using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.DTOs;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
        List<UserDto> GetUsersDtos(Expression<Func<UserDto, bool>> filter = null);
    }

    
}
