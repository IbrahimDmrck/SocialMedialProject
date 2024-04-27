using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using NuGet.Common;
using SocialMedia_Web.Models;
using SocialMedia_Web.Utilities.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Web;

namespace SocialMedia_Web.Controllers
{
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
        public async Task<IActionResult> LoginPost(UserForLoginDto userForLogin)
        {
            var jsonLogin = JsonConvert.SerializeObject(userForLogin);
            StringContent conten = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("https://localhost:44347/api/Auth/login", conten);

            if (responseMessage.IsSuccessStatusCode)
            {
                var userForLoginSuccess = await GetUserForLogin(responseMessage);

                TempData["Message"] = userForLoginSuccess.Message;
                TempData["Success"] = userForLoginSuccess.Success;

                if (userForLoginSuccess.Data != null && userForLoginSuccess.Data.Token != null)
                {
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(userForLoginSuccess.Data.Token);
                    var claims = token.Claims.ToList();

                    var userName = ExtractUserNameFromJwtToken.GetUserNameFromJwtToken(userForLoginSuccess.Data.Token);
                    var userId = ExtractUserIdentityFromJwtToken.GetUserIdentityFromJwtToken(userForLoginSuccess.Data.Token);

                    HttpContext.Session.SetString("Token", userForLoginSuccess.Data.Token);
                    HttpContext.Session.SetString("UserName", userName);
                    HttpContext.Session.SetInt32("UserId", userId);
                    HttpContext.Session.SetString("Email", userForLogin.Email);

                    claims.Add(new Claim("sosyalmediyasitetoken", userForLoginSuccess.Data.Token));

                    var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                    var authProps = new AuthenticationProperties
                    {
                        ExpiresUtc = userForLoginSuccess.Data.Expiration,
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProps);
                    return RedirectToAction("Index", "Home");
                }


                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var userForLoginError = await GetUserForLogin(responseMessage);
                TempData["LoginFail"] = userForLoginError.Message;
                return RedirectToAction("Login", "Auth");
            }
        }

        [HttpGet("aramiza-katil")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var jsonRegister = JsonConvert.SerializeObject(userForRegisterDto);
            StringContent content = new StringContent(jsonRegister, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("https://localhost:44347/api/Auth/register", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var response = new
                {
                    Success = true,
                    Message = "Başarılı bir şekilde kayıt oldunuz. Giriş sayfasına yönlendiriliyorsunuz",
                    Url = "/giris-yap"
                };
                return Json(response);
            }
            else
            {
                var response = new
                {
                    Success = false,
                    Message = "Bilgilerinizi kontrol edip tekrar deneyin"
                };
                return Json(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ResetPassword resetPassword)
        {
            var jsonRegister = JsonConvert.SerializeObject(resetPassword);
            StringContent content = new StringContent(jsonRegister, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("https://localhost:44347/api/VerificationCodes/sendcodeforgotpassword", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var response = new
                {
                    Success = true,
                    Message = "E-posta adresinize bir doğrulama kodu gönderildi",
                    Url = "/Auth/CheckCode"
                };
                TempData["UserEmail"] = resetPassword.Email;
                return Json(response);
            }
            else
            {
                var response = new
                {
                    Success = false,
                    Message = "Bilgilerinizi kontrol edip tekrar deneyin"
                };
                return Json(response);
            }
        }

        [HttpGet]
        public IActionResult CheckCode()
        {
            ViewData["Email"] = TempData["UserEmail"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckCode(ResetPassword resetPassword)
        {
            var jsonRegister = JsonConvert.SerializeObject(resetPassword);
            StringContent content = new StringContent(jsonRegister, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("https://localhost:44347/api/VerificationCodes/checkcodeforgotpassword", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<ResetPassword>>(responseContent);
                var response = new
                {
                    Success = apiDataResponse.Success,
                    Message = apiDataResponse.Message,
                    Url = "/Auth/ResetPassword"
                };
                TempData["UserEmail"] = resetPassword.Email;
                return Json(response);
            }
            else
            {
                var response = new
                {
                    Success = false,
                    Message = "Bilgilerinizi kontrol edip tekrar deneyin"
                };
                return Json(response);
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            ViewData["Email"] = TempData["UserEmail"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(Models.ChangePassword resetPassword)
        {
            var jsonRegister = JsonConvert.SerializeObject(resetPassword);
            StringContent content = new StringContent(jsonRegister, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("https://localhost:44347/api/Auth/adminchangepassword", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<Models.ChangePassword>>(responseContent);
                var response = new
                {
                    Success = apiDataResponse.Success,
                    Message = apiDataResponse.Message,
                    Url = "/giris-yap"
                };
                return Json(response);
            }
            else
            {
                var response = new
                {
                    Success = false,
                    Message = "Şifre güncellenemedi, lütfen tekrara deneyin"
                };
                return Json(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }

        private async Task<ApiDataResponse<UserForLoginDto>> GetUserForLogin(HttpResponseMessage responseMessage)
        {
            string responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<UserForLoginDto>>(responseContent);
        }
    }
}
