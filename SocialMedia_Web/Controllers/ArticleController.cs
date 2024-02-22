using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace SocialMedia_Web.Controllers
{

    public class ArticleController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("share-content")]
        public async Task<IActionResult> SharingContent(Article article)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var jsonArticle = JsonConvert.SerializeObject(article);
            var content = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PostAsync("http://localhost:65525/api/Articles/add", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var sharedResponse = await GetSharedResponse(responseMessage);
                TempData["Message"] = sharedResponse.Message;
                TempData["Success"] = sharedResponse.Success;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("update-content")]
        public async Task<IActionResult> UpdateContent(Article article)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var jsonArticle = JsonConvert.SerializeObject(article);
            var content = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PutAsync("http://localhost:65525/api/Articles/update", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var sharedResponse = await GetSharedResponse(responseMessage);
                TempData["Message"] = sharedResponse.Message;
                TempData["Success"] = sharedResponse.Success;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("delete-article")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("http://localhost:65525/api/Articles/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["Message"] = "Paylaşım Silindi";
                TempData["Success"] = true;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getarticlebyid")]
        public async Task<IActionResult> GetUpdateArticle(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var responseMessage = await httpClient.GetAsync("http://localhost:65525/api/Articles/getbyid?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<Article>>(responseContent);

                var responseMessage1 = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65525/api/Topics/getall");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonResponse1 = await responseMessage1.Content.ReadAsStringAsync();
                    var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<Topics>>(jsonResponse1);

                    var response = new ArticleTopicsResponse
                    {
                        Article = data.Data,
                        Topics = apiDataResponse.Data
                    };
                    return Json(response);
                }

            }
            return RedirectToAction("Index", "Home");
        }

        public class ArticleTopicsResponse
        {
            public Article Article { get; set; }
            public List<Topics> Topics { get; set; }
        }

        private async Task<ApiDataResponse<Article>> GetSharedResponse(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<Article>>(responseContent);
        }
    }
}
