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
using Hangfire;
namespace Business.Concrete
{
    public class TopicManager : ITopicService
    {
        ITopicDal _topicDal;

        public TopicManager(ITopicDal topicDal)
        {
            _topicDal = topicDal;
        }

        [LogAspect(typeof(FileLogger))]
        [CacheRemoveAspect("ITopicService.Get")]
        public void DeleteTopicDaily()
        {
            var allTopic = _topicDal.GetAll(x => x.Status == true);
            if (allTopic != null)
            {
                foreach (var topic in allTopic)
                {
                    Topic updatedTopic = new Topic
                    {
                        Id = topic.Id,
                        TopicTitle = topic.TopicTitle,
                        Status = false
                    };
                    _topicDal.Update(updatedTopic);
                }
            }

        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(TopicCValidator))]
        [SecuredOperation("admin")]
        [CacheRemoveAspect("ITopicService.Get")]
        public IResult Add(Topic entity)
        {
            _topicDal.Add(entity);
            return new SuccessResult(Messages.Topic_Add);
        }

        [LogAspect(typeof(FileLogger))]
        [SecuredOperation("admin")]
        [CacheRemoveAspect("ITopicService.Get")]
        public IResult Delete(int id)
        {
            var deleteTopic = _topicDal.Get(x => x.Id == id);
            if (deleteTopic != null)
            {
                _topicDal.Delete(deleteTopic);
                return new SuccessResult(Messages.Topic_Delete);
            }
            return new ErrorResult(Messages.TopicNotFound);

        }
        [CacheAspect(2)]
        public IDataResult<List<Topic>> GetAll()
        {
            return new SuccessDataResult<List<Topic>>(_topicDal.GetAll(),Messages.Topics_Listed);
        }

        public IDataResult<Topic> GetEntityById(int id)
        {
            return new SuccessDataResult<Topic>(_topicDal.Get(x=>x.Id==id),Messages.Topic_Listed);
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(TopicCValidator))]
        [SecuredOperation("admin")]
        [CacheRemoveAspect("ITopicService.Get")]
        public IResult Update(Topic entity)
        {
            _topicDal.Update(entity);
            return new SuccessResult(Messages.Topic_Update);
        }
    }
}
