using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Text;

namespace SocialMedia_Web.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        [HttpGet("giris-yap")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("LoginPost")]
        public async Task<IActionResult> LoginPost(UserForLoginDto loginDto)
        {
            var httpClient = new HttpClient();
            var jsonLoginDto = JsonConvert.SerializeObject(loginDto);
            StringContent content = new StringContent(jsonLoginDto, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("http://localhost:65525/api/Auth/login",content);
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                var userForLogin = JsonConvert.DeserializeObject<ApiAuthDataResponse<UserForLoginDto>>(responseContent);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet("Aramiza-Katil")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserForRegisterDto registerDto)
        {
            return View();
        }
    }
}
