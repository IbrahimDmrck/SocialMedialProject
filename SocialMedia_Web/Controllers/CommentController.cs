using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Net.Http;
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

        [Authorize(Roles = "admin,user")]
        [HttpPost("post-comment")]
        public async Task<IActionResult> Comment(Comment comment)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var jsonComment = JsonConvert.SerializeObject(comment);
            var content = new StringContent(jsonComment, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/Comments/add", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin,user")]
        [HttpPost("delete-comment")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("https://localhost:44347/api/Comments/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["Message"] = "Yorum silindi";
                TempData["Success"] = true;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("notification")]
        public async Task<IActionResult> Notification()
        {

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            ViewData["MyArticle"] = HttpContext.Session.GetInt32("MyArticle");
            ViewData["UserId"] = userId;
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:44347/api/Articles/getarticlewithdetailsbyuserid?id=" + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();

                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");

                return apiDataResponse.Success ? View(apiDataResponse.Data) : RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> ReadAllNotification()
        {
            var httpClient = _httpClientFactory.CreateClient();
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            ViewData["UserId"] = userId;
            var responseMessage = await httpClient.GetAsync("https://localhost:44347/api/Articles/getarticlewithdetailsbyuserid?id=" + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);

                var comments = apiDataResponse.Data.SelectMany(x => x.CommentDetails);

                foreach (var item in comments)
                {
                    if (item.Status == false && item.Id != -1)
                    {
                        CommentDetail commentDetail = new CommentDetail
                        {
                            Id = item.Id,
                            ArticleId = item.ArticleId,
                            UserId = item.UserId,
                            CommentText = item.CommentText,
                            CommentDate = item.CommentDate,
                            Status = true

                        };

                        var jsonComment = JsonConvert.SerializeObject(commentDetail);
                        var content = new StringContent(jsonComment, Encoding.UTF8, "application/json");
                        var token = HttpContext.Session.GetString("Token");
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var responseReadAllNotifications = await httpClient.PostAsync("https://localhost:44347/api/Comments/update", content);
                        if (responseReadAllNotifications.IsSuccessStatusCode)
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return RedirectToAction("Notification", "Comment");
        }


    }
}
