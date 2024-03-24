using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Diagnostics;

namespace SocialMedia_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var responseMessage = await httpClient.GetAsync("http://localhost:65526/api/Articles/getarticlewithdetails");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);
                ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");

                int myArticleCount = apiDataResponse.Data.Count(x => x.UserId == (int)ViewData["UserId"]);

                HttpContext.Session.SetInt32("MyArticle", apiDataResponse.Data.Count(x => x.UserId == (int)ViewData["UserId"]));
                ViewData["MyArticle"] = myArticleCount;

                return apiDataResponse.Success ? View(apiDataResponse.Data) : (IActionResult)View("Veri gelmiyor");
            }
            return View("Veri gelmiyor");
        }

    }
}
