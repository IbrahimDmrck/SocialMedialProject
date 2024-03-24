using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Net.Http.Headers;
using System.Text;
using static SocialMedia_Web.Controllers.ArticleController;

namespace SocialMedia_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/Articles/getarticlewithdetails");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);

                return apiDataResponse.Success ? View(apiDataResponse.Data) : RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/Articles/getarticlewithdetailsbyid?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<ArticleDetailDto>>(jsonResponse);

                return apiDataResponse.Success ? View(apiDataResponse.Data) : View("Veri gelmiyor");
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetUpdateArticle(int id)
        {
            
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/Articles/getbyid?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<Article>>(responseContent);

                var responseMessage1 = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/Topics/getall");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonResponse1 = await responseMessage1.Content.ReadAsStringAsync();
                    var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<Topics>>(jsonResponse1);

                    var response = new ArticleTopicsResponse
                    {
                        Article = data.Data,
                        Topics = apiDataResponse.Data
                    };
                    return View(response);
                }

            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateContent(Article article)
        {
            var jsonArticle = JsonConvert.SerializeObject(article);
            var content = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _httpClientFactory.CreateClient().PutAsync("http://localhost:65526/api/Articles/update", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<Article>>(responseContent);
                var response = new
                {
                    Message = data.Message
                };
                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("http://localhost:65526/api/Articles/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<ArticleDetailDto>>(jsonResponse);

                var response = new
                {
                    Message = apiDataResponse.Message,
                    Url = "/Admin/Home/Index"
                };
                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> CommentDelete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("http://localhost:65526/api/Comments/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<Comment>>(jsonResponse);

                var response = new
                {
                    Message = apiDataResponse.Message
                };
                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateComment(CommentDetail commentDetail)
        {
            var jsonComment = JsonConvert.SerializeObject(commentDetail);
            var content = new StringContent(jsonComment, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _httpClientFactory.CreateClient().PutAsync("http://localhost:65526/api/Comments/update", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<CommentDetail>>(responseContent);
                var response = new
                {
                    Message = data.Message
                };
                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
