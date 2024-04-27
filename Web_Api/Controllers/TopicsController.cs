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
        private readonly ITopicService _topicService;

        public TopicsController(ITopicService topicService) => _topicService = topicService ?? throw new ArgumentNullException(nameof(topicService));

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            IDataResult<List<Topic>> result = _topicService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("getbyid")]
        public ActionResult GetById(int id)
        {
            IDataResult<Topic> result = _topicService.GetEntityById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("add")]
        public ActionResult Add(Topic topic)
        {
            IResult result = _topicService.Add(topic);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public ActionResult Update(Topic topic)
        {
            IResult result = _topicService.Update(topic);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public ActionResult Delete(int id)
        {
            IResult result = _topicService.Delete(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
