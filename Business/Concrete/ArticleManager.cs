using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ArticleManager : IArticleService
    {
        private readonly IArticleDal _articleDal;

        public ArticleManager(IArticleDal articleDal)
        {
            _articleDal = articleDal;
        }

        public IResult Add(Article entity)
        {
           _articleDal.Add(entity);
            return new SuccessResult(Messages.ArticleAdded);
        }

        public IResult Delete(Article entity)
        {
            var deletedArticle = _articleDal.Get(x=>x.Id==entity.Id);
            return new SuccessResult(Messages.ArticleDeleted);
        }

        public IDataResult<List<Article>> GetAll()
        {
            return new SuccessDataResult<List<Article>>(_articleDal.GetAll(),Messages.ArticlesListed);
        }

        public IDataResult<Article> GetEntityById(int id)
        {
            return new SuccessDataResult<Article>(_articleDal.Get(x=>x.Id==id),Messages.ArticleListed);
        }

        public IResult Update(Article entity)
        {
            _articleDal.Update(entity);
            return new SuccessResult(Messages.ArticleUpdated);
        }
    }
}
