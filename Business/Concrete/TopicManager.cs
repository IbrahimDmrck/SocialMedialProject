using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TopicManager : ITopicService
    {
        ITopicDal _topicDal;

        public TopicManager(ITopicDal topicDal)
        {
            _topicDal = topicDal;
        }

        public IResult Add(Topic entity)
        {
            _topicDal.Add(entity);
            return new SuccessResult(Messages.Topic_Add);
        }

        public IResult Delete(Topic entity)
        {
            _topicDal.Delete(entity);
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
