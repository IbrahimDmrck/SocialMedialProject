﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using SocialMedia_Web.Models;
using SocialMedia_Web.Utilities.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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
        public async Task<IActionResult> LoginPost(UserForLoginDto loginDto)
        {
            var jsonLoginDto = JsonConvert.SerializeObject(loginDto);
            var content = new StringContent(jsonLoginDto, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("http://localhost:65526/api/Auth/login", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                var userForLogin = await GetUserForLogin(responseMessage);

                TempData["Baslik"] = "Giriş Başarılı";
                TempData["Message"] = " Merhaba " + userForLogin.Message + ", hoş geldin.";
                TempData["Success"] = userForLogin.Success;

                if (userForLogin.Data != null)
                {
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(userForLogin.Data.Token);
                    var claims = token.Claims.ToList();

                    if (userForLogin.Data.Token != null)
                    {
                        _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userForLogin.Data.Token);
                        var userId = ExtractUserIdentityFromJwtToken.GetUserIdentityFromJwtToken(userForLogin.Data.Token);

                        HttpContext.Session.SetInt32("UserId", userId);
                        HttpContext.Session.SetString("Email", loginDto.Email);
                        HttpContext.Session.SetString("Token", userForLogin.Data.Token);

                        claims.Add(new Claim("socialmediawebsitetoken", userForLogin.Data.Token));
                        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                        var authProps = new AuthenticationProperties
                        {
                            ExpiresUtc = userForLogin.Data.Expiration,
                            IsPersistent = true
                        };

                        await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProps);
                        return RedirectToAction("Index", "Home");
                    }
                }
                return RedirectToAction("Login", "Auth");
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

        [HttpGet("aramiza-katil")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto registerDto)
        {
            var jsonRegisterDto = JsonConvert.SerializeObject(registerDto);
            var content = new StringContent(jsonRegisterDto, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("http://localhost:65526/api/Auth/register", content);
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


        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
