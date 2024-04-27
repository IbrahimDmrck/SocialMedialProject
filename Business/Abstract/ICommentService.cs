using Core.Service;
using Core.Utilities.Result.Abstract;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICommentService : IServiceRepository<Comment>
    {
        IDataResult<List<Comment>> TrueComment();
        IDataResult<List<Comment>> NotSeen(int id);
        IDataResult<List<Comment>> FalseComment();

        IDataResult<List<Comment>> GetbyArticleId(int id);
        IResult AllCommentDeleteByUserId(int id);
    }
}
