using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static SocialMedia_Web.Controllers.ArticleController;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialMedia_Web.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SettingsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("hesap-bilgilerim")]
        public async Task<IActionResult> AccountSetting()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:44347/api/Users/getbyid?id=" + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(jsonResponse);
                ViewData["MyArticle"] = HttpContext.Session.GetInt32("MyArticle");
                return apiDataResponse.Success ? View(apiDataResponse.Data) : (IActionResult)View("Veri gelmiyor");
            }
            return View("Veri Gelmiyor");
        }

        [Authorize(Roles = "admin,user")]
        [HttpPost("bilgileri-guncelle")]
        public async Task<IActionResult> UpdateAccountSetting(UserDto userDto)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonUserDto = JsonConvert.SerializeObject(userDto);
            var content = new StringContent(jsonUserDto, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/Users/update", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                var successUpdatedUser = await GetUpdateUserResponseMessage(responseMessage);
                TempData["Message"] = successUpdatedUser.Message;
                TempData["Success"] = successUpdatedUser.Success;
                return RedirectToAction("AccountSetting", "Settings");
            }
            else
            {
                var successUpdatedUser = await GetUpdateUserResponseMessage(responseMessage);
                TempData["Message"] = successUpdatedUser.Message;
                TempData["Success"] = successUpdatedUser.Success;
                return View();
            }
        }

        [Authorize(Roles = "admin,user")]
        [HttpPost("photo-update")]
        public async Task<IActionResult> UpdateUserImage(UserImage userImage)
        {
            if (userImage.ImagePath != null)
            {
                using (var formContent = new MultipartFormDataContent())
                {
                    formContent.Add(new StringContent(userImage.Id.ToString()), "Id");
                    formContent.Add(new StringContent(userImage.UserId.ToString()), "UserId");
                    formContent.Add(new StringContent(userImage.ImagePath.FileName), "ImagePath");
                    formContent.Add(new StreamContent(userImage.ImagePath.OpenReadStream())
                    {
                        Headers =
                        {
                            ContentLength=userImage.ImagePath.Length,
                            ContentType=new MediaTypeHeaderValue(userImage.ImagePath.ContentType)
                        }
                    },
                    "ImageFile", userImage.ImagePath.FileName);

                    var httpClient = _httpClientFactory.CreateClient();
                    var token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var responseMesssage = await httpClient.PostAsync("https://localhost:44347/api/UserImages/update", formContent);
                    var successUpdatedUserImage = await GetUpdateUserImageResponseMessage(responseMesssage);
                    TempData["Message"] = successUpdatedUserImage.Message;
                    TempData["Success"] = successUpdatedUserImage.Success;

                    return RedirectToAction("AccountSetting", "Settings");
                }
            }

            return RedirectToAction("AccountSetting", "Settings");
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("kod-dogrulama")]
        public async Task<IActionResult> GetVerifyCode()
        {
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            ViewData["Email"] = HttpContext.Session.GetString("Email");
            return View();
        }

        [Authorize(Roles = "admin,user")]
        [HttpPost("kod")]
        public async Task<IActionResult> GetVerifyCode(VerificationCode verificationCodeDto)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonInfo = JsonConvert.SerializeObject(verificationCodeDto);
            var content = new StringContent(jsonInfo, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/VerificationCodes/sendcode", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var response = new
                {
                    Success = true,
                    Url = "kod-dogrulama"
                };

                return Json(response);
            }
            return RedirectToAction("AccountSetting", "Settings");
        }
        [Authorize(Roles = "admin,user")]
        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode(VerificationCode verificationCodeDto)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonInfo = JsonConvert.SerializeObject(verificationCodeDto);
            var content = new StringContent(jsonInfo, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/VerificationCodes/checkcode", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<VerificationCode>>(responseContent);

                var response = new
                {
                    Success = true,
                    Message = apiDataResponse.Message,
                    Url = "sifre-guncelle"
                };

                return Json(response);
            }
            else
            {
                var response = new
                {
                    Message = "Kod doğrulanamadı ! . Lütfen tekrar deneyin",
                };
                return Json(response);
            }

        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("sifre-guncelle")]
        public async Task<IActionResult> ChangePassword()
        {
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            ViewData["Email"] = HttpContext.Session.GetString("Email");
            return View();
        }
        [Authorize(Roles = "admin,user")]
        [HttpPost("sifre-guncelle")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonInfo = JsonConvert.SerializeObject(changePassword);
            var content = new StringContent(jsonInfo, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/Auth/changepassword", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<ChangePassword>>(responseContent);

                var response = new
                {
                    Success = true,
                    Message = apiDataResponse.Message,
                    Url = "/"
                };

                return Json(response);
            }
            else
            {
                var response = new
                {
                    Message = "Şifre Güncellenemedi , lütfen tekrar deneyin",
                };
                return Json(response);
            }

        }
        [Authorize(Roles = "admin,user")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync($"https://localhost:44347/api/Users/delete?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(responseContent);

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
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(responseContent);
                var response = new
                {
                    Success = apiDataResponse.Success,
                    Message = apiDataResponse.Message
                };
                return Json(response);
            }

        }

        private async Task<ApiDataResponse<UserImage>> GetUpdateUserImageResponseMessage(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<UserImage>>(responseContent);
        }
        private async Task<ApiDataResponse<UserDto>> GetUpdateUserResponseMessage(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(responseContent);
        }

    }
}
