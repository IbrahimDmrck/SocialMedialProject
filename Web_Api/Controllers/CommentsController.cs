using Business.Abstract;
using Core.Utilities.Result.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IResult = Core.Utilities.Result.Abstract.IResult;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        ICommentService _commentService;

        public CommentsController(ICommentService commentService) => _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            IDataResult<List<Comment>> comments = _commentService.GetAll();
            return comments.Success ? Ok(comments) : BadRequest(comments);
        }

        [HttpGet("getbyid")]
        public ActionResult GetById(int id)
        {
            IDataResult<Comment> comment = _commentService.GetEntityById(id);
            return comment.Success ? Ok(comment) : BadRequest(comment);
        }

        [HttpPost("add")]
        public IActionResult Add(Comment comment)
        {
            IResult result = _commentService.Add(comment);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(Comment comment)
        {
            IResult result = _commentService.Delete(comment);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(Comment comment)
        {
            IResult result = _commentService.Update(comment);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
