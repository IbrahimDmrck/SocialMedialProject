using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IResult = Core.Utilities.Result.Abstract.IResult;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        ITopicService _topicService;

        public TopicsController(ITopicService topicService) => _topicService = topicService ?? throw new ArgumentNullException(nameof(topicService));

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            IDataResult<List<Topic>> topics = _topicService.GetAll();
            return topics.Success ? Ok(topics) : BadRequest(topics);
        }

        [HttpGet("getbyid")]
        public ActionResult GetById(int id)
        {
            IDataResult<Topic> topic = _topicService.GetEntityById(id);
            return topic.Success ? Ok(topic) : BadRequest(topic);
        }

        [HttpPost("add")]
        public IActionResult Add(Topic topic)
        {
            IResult result = _topicService.Add(topic);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            IResult result = _topicService.Delete(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(Topic topic)
        {
            IResult result = _topicService.Update(topic);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
