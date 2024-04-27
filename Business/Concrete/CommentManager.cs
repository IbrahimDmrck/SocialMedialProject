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
using DataAccess.Concrete.Entityframework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly ICommentDal _commentDal;

        public CommentManager(ICommentDal commentDal) => _commentDal = commentDal;

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [ValidationAspect(typeof(CommentValidator))]
        [CacheRemoveAspect("IComemntService.Get")]
        public IResult Add(Comment entity)
        {
            _commentDal.Add(entity);
            return new SuccessResult(Messages.Comment_Add);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("IComemntService.Get")]
        public IResult AllCommentDeleteByUserId(int id)
        {
            var deleteComments = _commentDal.GetAll(x => x.UserId == id);
            if (deleteComments != null)
            {
                foreach (var item in deleteComments)
                {
                    var comment = new Comment
                    {
                        UserId = item.UserId,
                        ArticleId = item.ArticleId,
                        CommentDate = item.CommentDate,
                        CommentText = item.CommentText,
                        Id = item.Id,
                        Status = item.Status,
                    };
                    _commentDal.Delete(comment);

                }
                return new SuccessResult(Messages.Comment_Delete);
            }
            return new SuccessResult(Messages.Comment_Delete);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("ICommentService.Get")]
        public IResult Delete(int id)
        {
            var deleteComment = _commentDal.Get(x => x.Id == id);
            if (deleteComment != null)
            {
                _commentDal.Delete(deleteComment);

                return new SuccessResult(Messages.Comment_Delete);
            }
            return new ErrorResult(Messages.CommentNotFound);
        }
        [CacheAspect(1)]
        public IDataResult<List<Comment>> FalseComment()
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetAll(x=>x.Status==false), Messages.FalseComment);
        }
        [CacheAspect(1)]
        public IDataResult<List<Comment>> GetAll()
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetAll(),Messages.Comments_Listed);
        }

        public IDataResult<List<Comment>> GetbyArticleId(int id)
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetAll(x => x.ArticleId == id), Messages.Comments_List);
        }

        public IDataResult<Comment> GetEntityById(int id)
        {
            return new SuccessDataResult<Comment>(_commentDal.Get(x=>x.Id == id),Messages.Comment_Listed);
        }

        public IDataResult<List<Comment>> NotSeen(int id)
        {
            throw new NotImplementedException();
        }
        [CacheAspect(1)]
        public IDataResult<List<Comment>> TrueComment()
        {

            return new SuccessDataResult<List<Comment>>(_commentDal.GetAll(x => x.Status == true), Messages.FalseComment);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [ValidationAspect(typeof(CommentValidator))]
        [CacheRemoveAspect("IComemntService.Get")]
        public IResult Update(Comment entity)
        {
            var updatedComment = _commentDal.Get(x => x.Id == entity.Id);
            if (updatedComment != null)
            {
                _commentDal.Update(entity);
                return new SuccessResult(Messages.Comment_Update);
            }
            return new ErrorResult(Messages.CommentNotFound);
        }
    }
}
