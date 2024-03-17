using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;

namespace SocialMedia_Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [Authorize(Roles = "admin,user")]
        [HttpGet("profilim")]
        public async Task<IActionResult> Profile()
        {

            var userId = HttpContext.Session.GetInt32("UserId");
            ViewData["MyArticle"] = HttpContext.Session.GetInt32("MyArticle");
            ViewData["UserId"] = userId;
            var httpClient = _httpClientFactory.CreateClient();
            var responseMessage = await httpClient.GetAsync("http://localhost:65525/api/Articles/getarticlewithdetailsbyuserid?id=" + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);
                return apiDataResponse.Success ? View(apiDataResponse.Data) : (IActionResult)View("Veri gelmiyor");
            }
            return View("Veri gelmiyor");
        }
    }
}
