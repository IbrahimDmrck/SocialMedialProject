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
        IArticleService _articleService;

        public ArticlesController(IArticleService articleService) => _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));

        [HttpGet("getarticlewithdetails")]
        public ActionResult GetDetails()
        {
            IDataResult<List<ArticleDetailDto>> articles = _articleService.GetArticleDetails();
            return articles.Success ? Ok(articles) : BadRequest(articles);
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
            IDataResult<List<Article>> articles = _articleService.GetAll();
            return articles.Success ? Ok(articles) : BadRequest(articles);
        }

        [HttpGet("getbyid")]
        public ActionResult GetById(int id)
        {
            IDataResult<Article> articles = _articleService.GetEntityById(id);
            return articles.Success ? Ok(articles) : BadRequest(articles);
        }

        [HttpPost("add")]
        public IActionResult Add(Article article)
        {
            IResult articles = _articleService.Add(article);
            return articles.Success ? Ok(articles) : BadRequest(articles);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            IResult articles = _articleService.Delete(id);
            return articles.Success ? Ok(articles) : BadRequest(articles);
        }

        [HttpPut("update")]
        public IActionResult Update(Article article)
        {
            IResult articles = _articleService.Update(article);
            return articles.Success ? Ok(articles) : BadRequest(articles);
        }
    }
}
