using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Net.Http;
using System.Text;

namespace SocialMedia_Web.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SettingsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("hesabım-bilgilerim")]
        public async Task<IActionResult> AccountSetting()
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

        //http://localhost:65525/api/UserImages/add
        [HttpPost]
        public async Task<IActionResult> UpdateAccountSetting(UserDto userDto)
        {
            var httpClient = _httpClientFactory.CreateClient();
            if (userDto.ImagePath != null)
            {
                var extension = Path.GetExtension(userDto.ImagePath.FileName);
                var newImageName = Guid.NewGuid().ToString() + extension;
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await userDto.ImagePath.CopyToAsync(stream);
                }

                // Profil güncelleme işlemi için API'ye isteği gönder
                var apiResponse = await UpdateProfileImageInApi(userDto.Id, newImageName);

                if (apiResponse.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Profil güncelleme başarılı.";
                }
                else
                {
                    TempData["Message"] = "Profil güncelleme başarısız.";
                }

            }

            return RedirectToAction("AccountSetting", "Settings"); // Profil sayfasına geri dön
        }

        private async Task<HttpResponseMessage> UpdateProfileImageInApi(int userId, string imageName)
        {
            var apiUrl = $"http://localhost:65525/api/UserImages/add/{userId}";
            var apiContent = new StringContent(JsonConvert.SerializeObject(new { ImageName = imageName }), Encoding.UTF8, "application/json");
            return await _httpClientFactory.CreateClient().PostAsync(apiUrl, apiContent);
        }
    }
}
