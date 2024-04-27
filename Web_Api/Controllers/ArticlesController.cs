using Business.Abstract;
using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IResult = Core.Utilities.Result.Abstract.IResult;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService) => _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));

        [HttpGet("getarticlewithdetails")]
        public ActionResult GetDetails()
        {
            IDataResult<List<ArticleDetailDto>> result = _articleService.GetArticleDetails();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getarticlewithdetailsbyid")]
        public ActionResult GetDetailsById(int id)
        {
            IDataResult<ArticleDetailDto> result = _articleService.GetArticleDetailsById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getarticlewithdetailsbyuserid")]
        public ActionResult GetDetailsByUserId(int id)
        {
            IDataResult<List<ArticleDetailDto>> result = _articleService.GetArticleDetailsByUserId(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            IDataResult<List<Article>> result = _articleService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getbyid")]
        public ActionResult GetById(int id)
        {
            IDataResult<Article> result = _articleService.GetEntityById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("add")]
        public ActionResult Add(Article article)
        {
            IResult result = _articleService.Add(article);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("update")]
        public ActionResult Update(Article article)
        {
            IResult result = _articleService.Update(article);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public ActionResult Delete(int id)
        {
            IResult result = _articleService.Delete(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
