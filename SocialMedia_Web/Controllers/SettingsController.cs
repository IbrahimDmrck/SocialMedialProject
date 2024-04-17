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
            ViewData["UserId"] = userId;
            ViewData["MyArticle"] = HttpContext.Session.GetInt32("MyArticle");
            var httpClient = _httpClientFactory.CreateClient();
            var responseMessage = await httpClient.GetAsync("http://localhost:65527/api/Users/getbyid?id=" + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(jsonResponse);
                return apiDataResponse.Success ? View(apiDataResponse.Data) : (IActionResult)View("Veri gelmiyor");
            }
            return View("Veri gelmiyor");
        }

        [Authorize(Roles = "admin,user")]
        [HttpPost("bilgileri-guncelle")]
        public async Task<IActionResult> UpdateAccountSetting(UserDto userDto)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            var jsonUserDto = JsonConvert.SerializeObject(userDto);
            var content = new StringContent(jsonUserDto, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PostAsync("http://localhost:65527/api/Users/update", content);
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
                            ContentLength = userImage.ImagePath.Length,
                            ContentType = new MediaTypeHeaderValue(userImage.ImagePath.ContentType)
                        }
                    }, "ImageFile", userImage.ImagePath.FileName);
                    var token = HttpContext.Session.GetString("Token");
                    var httpClient = _httpClientFactory.CreateClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var responseMessage = await httpClient.PostAsync("http://localhost:65527/api/UserImages/update", formContent);

                    var successUpdatedUserImage = await GetUpdateUserImageResponseMessage(responseMessage);
                    TempData["Message"] = successUpdatedUserImage.Message;
                    TempData["Success"] = successUpdatedUserImage.Success;

                    return RedirectToAction("AccountSetting", "Settings");
                }
            }

            return RedirectToAction("AccountSetting", "Settings");
        }

        [HttpGet("kod-doğrulama")]
        public IActionResult GetVerifyCode()
        {
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            ViewData["Email"] = HttpContext.Session.GetString("Email");
            return View();
        }

        [HttpPost("kod")]
        public async Task<IActionResult> GetVerifyCode(VerificationCode verificationCode)
        {
           // var token = HttpContext.Session.GetString("Token");
         //   httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpClient = _httpClientFactory.CreateClient();
            var jsonInfo = JsonConvert.SerializeObject(verificationCode);
            var content = new StringContent(jsonInfo, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("http://localhost:65527/api/VerificationCodes/sendcode", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var response = new 
                {
                    Success= true,
                    Url= "/kod-doğrulama"
                };
                return Json(response);
            }
            return RedirectToAction("AccountSetting", "Settings");
        }


        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode(VerificationCode verificationCode)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var jsonInfo = JsonConvert.SerializeObject(verificationCode);
            var content = new StringContent(jsonInfo, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync($"http://localhost:65527/api/VerificationCodes/checkverifycode", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<VerificationCode>>(jsonResponse);
                var message = apiDataResponse.Message;
                var success = apiDataResponse.Success;

                var response = new
                {
                    Success = apiDataResponse.Success,
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

        [HttpGet("sifre-guncelle")]
        public async Task<IActionResult> ChangePassword()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");
            return View();
        }

        [HttpPost("sifre-guncelle")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(changePassword);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync($"http://localhost:65527/api/Auth/changepassword", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<ChangePassword>>(jsonResponse);
                var message = apiDataResponse.Message;
                var success = apiDataResponse.Success;

                var response = new
                {
                    Success = apiDataResponse.Success,
                    Message = apiDataResponse.Message,
                    
                };
                return Json(response);

            }
            else
            {
                var response = new
                {
                    Message = "Şifre güncellenemedi, lütfen tekrar deneyin",
                };
                return Json(response);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var responseMessage = await _httpClientFactory.CreateClient().DeleteAsync("http://localhost:65527/api/Users/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(jsonResponse);

                var response = new
                {
                    Success= apiDataResponse.Success,
                    Message = apiDataResponse.Message
                };
                return Json(response);
            }
            else
            {
                var response = new
                {
                    Success =false,
                    Message ="Bir hata oluştu , lütfen tekrar deneyin"
                };
                return Json(response);
            }
        }

        private async Task<ApiDataResponse<UserDto>> GetUpdateUserResponseMessage(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(responseContent);
        }

        private async Task<ApiDataResponse<UserImage>> GetUpdateUserImageResponseMessage(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<UserImage>>(responseContent);
        }

    }
}
