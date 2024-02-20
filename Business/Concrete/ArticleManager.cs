using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingconcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
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

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [ValidationAspect(typeof(ArticleValidator))]
        [CacheRemoveAspect("IArticleService.Get")]
        public IResult Add(Article entity)
        {
           _articleDal.Add(entity);
            return new SuccessResult(Messages.ArticleAdded);
        }

        [SecuredOperation("admin,user")]
        //[ValidationAspect(typeof(ArticleValidator))]
        [CacheRemoveAspect("IArticleService.Get")]
        public IResult Delete(int id)
        {
            var deletedArticle = _articleDal.Get(x=>x.Id==id);
            _articleDal.Delete(deletedArticle);
            return new SuccessResult(Messages.ArticleDeleted);
        }

        [CacheAspect(1)]
        public IDataResult<List<Article>> GetAll()
        {
            return new SuccessDataResult<List<Article>>(_articleDal.GetAll(),Messages.ArticlesListed);
        }

        //[CacheAspect(1)]
        public IDataResult<List<ArticleDetailDto>> GetArticleDetails()
        {
            return new SuccessDataResult<List<ArticleDetailDto>>(_articleDal.GetArticleDetails(), Messages.ArticleWithDetailListed);
        }

        //[CacheAspect(1)]
        public IDataResult<List<ArticleDetailDto>> GetArticleDetailsByUserId(int id)
        {
            return new SuccessDataResult<List<ArticleDetailDto>>(_articleDal.GetArticleDetails(x=>x.UserId==id), Messages.ArticleWithDetailListed);
        }

        [CacheAspect(10)]
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
