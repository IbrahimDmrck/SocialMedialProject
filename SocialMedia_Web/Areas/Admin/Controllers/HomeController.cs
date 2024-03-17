using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;

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
        public async Task<IActionResult> Index()
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65525/api/Articles/getarticlewithdetails");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);

                return apiDataResponse.Success ? View(apiDataResponse.Data) : View("Veri gelmiyor");
            }
            return View("Veri gelmiyor");
        }
    }
}
