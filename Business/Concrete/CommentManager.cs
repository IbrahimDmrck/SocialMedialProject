using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
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
        [CacheRemoveAspect("ICommentService.Get")]
        public IResult Add(Comment entity)
        {
            _commentDal.Add(entity);
            return new SuccessResult(Messages.Comment_Add);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("ICommentService.Get")]
        public IResult Delete(int id)
        {
            var deletedComment = _commentDal.Get(x => x.Id == id);
            _commentDal.Delete(deletedComment);
            return new SuccessResult(Messages.Comment_Delete);
        }

        public IDataResult<List<Comment>> FalseComment()
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetAll(x=>x.Status==false), Messages.FalseComment);
        }

        public IDataResult<List<Comment>> GetAll()
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetAll(),Messages.Comments_Listed);
        }

        public IDataResult<Comment> GetEntityById(int id)
        {
            return new SuccessDataResult<Comment>(_commentDal.Get(x=>x.Id == id),Messages.Comment_Listed);
        }

        public IDataResult<List<Comment>> NotSeen(int id)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Comment>> TrueComment()
        {

            return new SuccessDataResult<List<Comment>>(_commentDal.GetAll(x => x.Status == true), Messages.FalseComment);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin,user")]
        [CacheRemoveAspect("ICommentService.Get")]
        public IResult Update(Comment entity)
        {
            _commentDal.Update(entity);
            return new SuccessResult(Messages.Comment_Update);
        }
    }
}
