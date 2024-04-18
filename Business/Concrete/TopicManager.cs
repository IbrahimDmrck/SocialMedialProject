using Business.Abstract;
using Business.Constants;
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

        public void DeleteTopicDaily()
        {

            var allTopics = _topicDal.GetAll(x=>x.Status==true);

            // Geçerlilik süresi dolmuş kodları sil
            foreach (var topic in allTopics)
            {
                Topic updatedTopic = new Topic
                {
                    Id = topic.Id,
                    TopicTitle = topic.TopicTitle,
                    Date = topic.Date,
                    Status = false
                };
                _topicDal.Update(updatedTopic);
            }

        }



        public IResult Add(Topic entity)
        {
            _topicDal.Add(entity);
            return new SuccessResult(Messages.Topic_Add);
        }

        public IResult Delete(int id)
        {
            var deletedTopic = _topicDal.Get(x => x.Id == id);
            _topicDal.Delete(deletedTopic);
            return new SuccessResult(Messages.Topic_Delete);
        }

        public IDataResult<List<Topic>> GetAll()
        {
            return new SuccessDataResult<List<Topic>>(_topicDal.GetAll(),Messages.Topics_Listed);
        }

        public IDataResult<Topic> GetEntityById(int id)
        {
            return new SuccessDataResult<Topic>(_topicDal.Get(x=>x.Id==id),Messages.Topic_Listed);
        }

        public IResult Update(Topic entity)
        {
            _topicDal.Update(entity);
            return new SuccessResult(Messages.Topic_Update);
        }
    }
}
