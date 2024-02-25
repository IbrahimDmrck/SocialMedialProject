using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SocialMedia_Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CommentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("post-comment")]
        public async Task<IActionResult> Comment(Comment comment)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            var jsonComment = JsonConvert.SerializeObject(comment);
            var content = new StringContent(jsonComment, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PostAsync("http://localhost:65525/api/Comments/add", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var successComment = await GetCommentResponseMessage(responseMessage);
                TempData["Message"] = successComment.Message;
                TempData["Success"] = successComment.Success;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var failComment = await GetCommentResponseMessage(responseMessage);
                TempData["Message"] = failComment.Message;
                TempData["Success"] = failComment.Success;
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("delete-comment")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("http://localhost:65525/api/Comments/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["Message"] = "Yorum Silindi";
                TempData["Success"] = true;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("notifications")]
        public async Task<IActionResult> Notifications()
        {
            var httpClient = _httpClientFactory.CreateClient();
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var responseMessage = await httpClient.GetAsync("http://localhost:65525/api/Articles/getarticlewithdetailsbyuserid?id=" + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponseMyArticle = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);

                return apiDataResponseMyArticle.Success ? View(apiDataResponseMyArticle.Data) : RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task<ApiDataResponse<CommentDetail>> GetCommentResponseMessage(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<CommentDetail>>(responseContent);
        }
    }
}
