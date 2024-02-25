using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface ICommentDal : IEntityRepository<Comment>
    {
        //List<CommentDetail> NotSeenComment(Expression<Func<CommentDetail, bool>> filter = null);
    }
}
