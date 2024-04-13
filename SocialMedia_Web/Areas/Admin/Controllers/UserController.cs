using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using SocialMedia_Web.Models;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialMedia_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/Users/getalldto");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<UserDto>>(jsonResponse);

                return apiDataResponse.Success ? View(apiDataResponse.Data) : RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var responseMessage = await _httpClientFactory.CreateClient().DeleteAsync("http://localhost:65526/api/Users/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(jsonResponse);

                var response = new
                {
                    Message = apiDataResponse.Message
                };
                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/Users/getbyid?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(jsonResponse);
                ViewData["Email"] = HttpContext.Session.GetString("Email");
                return View(apiDataResponse.Data);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateAccountSetting(UserDto userDto)
        {
            var token = HttpContext.Session.GetString("Token");
            var jsonUserDto = JsonConvert.SerializeObject(userDto);
            var content = new StringContent(jsonUserDto, Encoding.UTF8, "application/json");
            _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("http://localhost:65526/api/Users/update", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var successUpdatedUser = await GetUpdateUserResponseMessage(responseMessage);
                var response = new
                {
                    Message = successUpdatedUser.Message,
                };
                return Json(response);
            }
            else
            {
                var successUpdatedUser = await GetUpdateUserResponseMessage(responseMessage);
                var response = new
                {
                    Message = successUpdatedUser.Message,
                };
                return Json(response);
            }

        }

        private async Task<ApiDataResponse<UserDto>> GetUpdateUserResponseMessage(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(responseContent);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
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
                    var responseMessage = await httpClient.PostAsync("http://localhost:65526/api/UserImages/update", formContent);

                    var successUpdatedUserImage = await GetUpdateUserImageResponseMessage(responseMessage);
                    TempData["Message"] = successUpdatedUserImage.Message;
                    TempData["Success"] = successUpdatedUserImage.Success;

                    return RedirectToAction("Detail", "User", new { area = "Admin", id = userImage.UserId });
                }
            }

            return RedirectToAction("Index", "User", new { area = "Admin" });
        }

        private async Task<ApiDataResponse<UserImage>> GetUpdateUserImageResponseMessage(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApiDataResponse<UserImage>>(responseContent);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> GetVerifyCode(VerificationCode verificationCode)
        {
            var jsonInfo = JsonConvert.SerializeObject(verificationCode);
            var content = new StringContent(jsonInfo, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("http://localhost:65526/api/VerificationCodes/sendcode", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<VerificationCode>>(responseContent);
                var response = new
                {
                    Message = data.Message,
                    Url = "/kod-doğrulama"
                };
                TempData["UserId"] = verificationCode.UserId;
                TempData["Email"] = verificationCode.Email;
                return Json(response);
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult VerifyCode()
        {
            VerificationCode verificationCode = new VerificationCode
            {
                UserId = (int)TempData["UserId"],
                Email = (string)TempData["Email"]
            };
            return View(verificationCode);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> VerifyCode(VerificationCode verificationCode)
        {
            var jsonInfo = JsonConvert.SerializeObject(verificationCode);
            var content = new StringContent(jsonInfo, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync($"http://localhost:65526/api/VerificationCodes/checkverifycode", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<VerificationCode>>(jsonResponse);

                var response = new
                {
                    Success = apiDataResponse.Success,
                    Message = apiDataResponse.Message,
                    Url = "/Admin/User/ChangePassword"
                };
                TempData["UserId"] = verificationCode.UserId;
                TempData["Email"] = verificationCode.Email;
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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            VerificationCode verificationCode = new VerificationCode
            {
                UserId = (int)TempData["UserId"],
                Email = (string)TempData["Email"]
            };
            return View(verificationCode);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var jsonData = JsonConvert.SerializeObject(changePassword);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync($"http://localhost:65526/api/Auth/changepassword", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<ChangePassword>>(jsonResponse);

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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> ExportUsers()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var responseMessage = await httpClient.GetAsync("http://localhost:65526/api/Users/getalldto");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                    var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<UserDto>>(jsonResponse);

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("kayitli_kullanicilar");
                        worksheet.Cell(1, 1).Value = "ID";
                        worksheet.Cell(1, 2).Value = "Ad";
                        worksheet.Cell(1, 3).Value = "Soyad";
                        worksheet.Cell(1, 4).Value = "Email";
                        worksheet.Cell(1, 5).Value = "TelNo";
                        worksheet.Cell(1, 6).Value = "Cinsiyet";
                        worksheet.Cell(1, 7).Value = "fotoğraf";

                        int row = 2;
                        foreach (var userDto in apiDataResponse.Data)
                        {
                            worksheet.Cell(row, 1).Value = userDto.Id;
                            worksheet.Cell(row, 2).Value = userDto.FirstName;
                            worksheet.Cell(row, 3).Value = userDto.LastName;
                            worksheet.Cell(row, 4).Value = userDto.Email;
                            worksheet.Cell(row, 5).Value = userDto.PhoneNumber;
                            worksheet.Cell(row, 6).Value = userDto.Gender;
                            worksheet.Cell(row, 7).Value = $"http://localhost:65526/{userDto.ImagePath}";
                            row++;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "kayitli_kullanicilar.xlsx");
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
