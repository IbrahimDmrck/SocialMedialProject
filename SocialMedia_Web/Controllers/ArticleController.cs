using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
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

        [HttpPost("share-content")]
        public async Task<IActionResult> SharingContent(Article article)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var jsonArticle = JsonConvert.SerializeObject(article);
            var content = new StringContent(jsonArticle, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("http://localhost:65525/api/Articles/add", content);
            if (responseMessage.IsSuccessStatusCode)
            {

                return RedirectToAction("Index", "Home");
            }
            return View("Veri Gelmiyor");
        }
    }
}
