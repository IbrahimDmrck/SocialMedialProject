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

        [HttpGet("profilim")]
        public async Task<IActionResult> Profile()
        {

            var userId = HttpContext.Session.GetInt32("UserId");

            var httpClient = _httpClientFactory.CreateClient();
            var responseMessage = await httpClient.GetAsync("http://localhost:65525/api/Users/getbyid?id=" + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(jsonResponse);
                return apiDataResponse.Success ? View(apiDataResponse.Data) : (IActionResult)View("Veri gelmiyor");
            }
            return View("Veri gelmiyor");
        }
    }
}
