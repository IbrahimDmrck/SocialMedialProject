using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface IArticleDal : IEntityRepository<Article>
    {
        List<ArticleDetailDto> GetArticleDetails(Expression<Func<ArticleDetailDto, bool>> filter=null);
        ArticleDetailDto GetArticleDetailsById(Expression<Func<ArticleDetailDto, bool>> filter);
    }
}
