using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocialMedia_Web.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace SocialMedia_Web.Controllers
{
    // [Route("[controller]")]

    [AllowAnonymous]
    public class AuthController : Controller
    {
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

            var responseMessage = await httpClient.PostAsync("http://localhost:65525/api/Auth/login", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var userForLogin = JsonConvert.DeserializeObject<ApiAuthDataResponse<UserForLoginDto>>(responseContent);

                TempData["Message"] = userForLogin.Message;
                TempData["Success"] = userForLogin.Success;

                var jwtToken = userForLogin.Data.Token;
                var roleClaims = GetRoleClaims(jwtToken);

                if (roleClaims != null && roleClaims.Any())
                {
                    var rolesAsString = string.Join(", ", roleClaims);

                    if (roleClaims.Contains(AdminRole))
                    {
                        return await SignInUser(AdminRole);
                    }

                    if (roleClaims.Contains(UserRole))
                    {
                        return await SignInUser(UserRole);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                try
                {
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    var userForLogin = JsonConvert.DeserializeObject<ApiAuthDataResponse<UserForLoginDto>>(responseContent);

                    TempData["LoginFail"] = userForLogin.Message;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while processing the failed response: {ex.Message}");
                }
                return RedirectToAction("Login", "Auth");
            }
        }

        private const string AdminRole = "admin";
        private const string UserRole = "user";

        private List<string> GetRoleClaims(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;
            return jsonToken?.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList();
        }

        private async Task<IActionResult> SignInUser(string role)
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


        [HttpGet("Aramiza-Katil")]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(UserForRegisterDto registerDto) => View();
    }
}
