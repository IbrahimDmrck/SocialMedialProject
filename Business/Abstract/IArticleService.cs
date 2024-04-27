using Core.Service;
using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.DTOs;
namespace Business.Abstract
{
    public interface IArticleService : IServiceRepository<Article>
    {
        IDataResult<List<ArticleDetailDto>> GetArticleDetails();
        IDataResult<List<ArticleDetailDto>> GetArticleDetailsByUserId(int id);
        IDataResult<ArticleDetailDto> GetArticleDetailsById(int id);
    }
}
