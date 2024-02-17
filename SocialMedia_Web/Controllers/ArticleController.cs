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
        [HttpPost("delete-article")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            //var jsonArticle = JsonConvert.SerializeObject(article);
            //var content = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("http://localhost:65525/api/Articles/delete?id="+id);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["Message"] = "Paylaşım Silindi";
                TempData["Success"] = true;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task<ApiDataResponse<Article>> GetSharedResponse(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<Article>>(responseContent);
        }
    }
}
