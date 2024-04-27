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
        private readonly ICommentService _commentService;

        public ArticleManager(IArticleDal articleDal, ICommentService commentService)
        {
            _articleDal = articleDal;
            _commentService = commentService;
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(ArticleValidator))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IArticleService.Get")]
        public IResult Add(Article entity)
        {
            _articleDal.Add(entity);
            return new SuccessResult(Messages.Article_Add);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IArticleService.Get")]
        public IResult Delete(int id)
        {
            var deleteArticle = _articleDal.Get(x => x.Id == id);
            if (deleteArticle != null)
            {
                var deledetComment = _commentService.GetbyArticleId(deleteArticle.Id);
                if (deledetComment != null)
                {
                    foreach (var item in deledetComment.Data)
                    {
                        _commentService.Delete(item.Id);
                    }
                }
                _articleDal.Delete(deleteArticle);
                return new SuccessResult(Messages.Article_Deleted);
            }
            return new ErrorResult(Messages.ArticleNotFound);

        }

        [CacheAspect(2)]
        public IDataResult<List<Article>> GetAll()
        {
            return new SuccessDataResult<List<Article>>(_articleDal.GetAll(), Messages.Articles_Listed);
        }

        [CacheAspect(2)]
        public IDataResult<List<ArticleDetailDto>> GetArticleDetails()
        {
            return new SuccessDataResult<List<ArticleDetailDto>>(_articleDal.GetArticleDetails(), Messages.ArticleWithDetailListed);
        }

        [CacheAspect(2)]
        public IDataResult<ArticleDetailDto> GetArticleDetailsById(int id)
        {
            return new SuccessDataResult<ArticleDetailDto>(_articleDal.GetArticleDetailsById(x => x.Id == id), Messages.ArticleWithDetailListed);
        }

        [CacheAspect(2)]
        public IDataResult<List<ArticleDetailDto>> GetArticleDetailsByUserId(int id)
        {
            return new SuccessDataResult<List<ArticleDetailDto>>(_articleDal.GetArticleDetails(x => x.UserId == id), Messages.ArticleWithDetailListed);
        }


        [CacheAspect(2)]
        public IDataResult<Article> GetEntityById(int id)
        {
            return new SuccessDataResult<Article>(_articleDal.Get(x => x.Id == id), Messages.Article_Listed);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IArticleService.Get")]
        public IResult Update(Article entity)
        {
            _articleDal.Update(entity);
            return new SuccessResult(Messages.Article_Edit);
        }
    }
}