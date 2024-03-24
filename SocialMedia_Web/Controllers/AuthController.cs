using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using SocialMedia_Web.Models;
using SocialMedia_Web.Utilities.Helpers;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
namespace SocialMedia_Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private const string AdminRole = "admin";
        private const string UserRole = "user";
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("giris-yap")]
        public IActionResult Login() => View();

        [HttpPost("LoginPost")]
        public async Task<IActionResult> LoginPost(UserForLoginDto loginDto)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var jsonLoginDto = JsonConvert.SerializeObject(loginDto);
            var content = new StringContent(jsonLoginDto, Encoding.UTF8, "application/json");

            var responseMessage = await httpClient.PostAsync("http://localhost:65526/api/Auth/login", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                var userForLogin = await GetUserForLogin(responseMessage);
                TempData["Baslik"] = "Giriş Başarılı";
                TempData["Message"] = " Merhaba "+userForLogin.Message+", hoş geldin.";
                TempData["Success"] = userForLogin.Success;

                var jwtToken = userForLogin.Data.Token;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var roleClaims = ExtractRoleClaimsFromJwtToken.GetRoleClaims(jwtToken);
                var userId = ExtractUserIdentityFromJwtToken.GetUserIdentityFromJwtToken(jwtToken);

                HttpContext.Session.SetInt32("UserId", userId);
                HttpContext.Session.SetString("Email", loginDto.Email);
                HttpContext.Session.SetString("Token", jwtToken);

                return await SignInUserByRole(roleClaims);
            }
            else
            {
                var userForLogin = await GetUserForLogin(responseMessage);

                TempData["LoginFail"] = userForLogin.Message;
                return RedirectToAction("Login", "Auth");
            }
        }


        private async Task<ApiDataResponse<UserForLoginDto>> GetUserForLogin(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<UserForLoginDto>>(responseContent);
        }

        private async Task<IActionResult> SignInUserByRole(List<string> roleClaims)
        {
            if (roleClaims != null && roleClaims.Any())
            {
                if (roleClaims.Contains(AdminRole))
                {
                    return await SignInUserByRoleClaim(AdminRole);
                }

                if (roleClaims.Contains(UserRole))
                {
                    return await SignInUserByRoleClaim(UserRole);
                }
            }
            return RedirectToAction("Login", "Auth");
        }

        private async Task<IActionResult> SignInUserByRoleClaim(string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role),
            };
            var userIdentity = new ClaimsIdentity(claims, role);
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(userPrincipal);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("aramiza-katil")]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(UserForRegisterDto registerDto) => View();

        [HttpPost]
        public IActionResult LogOut() => View();
    }
}
