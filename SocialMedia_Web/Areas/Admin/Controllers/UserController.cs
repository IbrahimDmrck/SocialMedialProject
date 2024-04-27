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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:44347/api/Users/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<UserDto>>(jsonResponse);
                return View(apiDataResponse.Data);
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync($"https://localhost:44347/api/Users/getbyid?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(jsonResponse);
                ViewData["Email"] = HttpContext.Session.GetString("Email");
                return View(apiDataResponse.Data);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo(UserDto userDto)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonUserDto = JsonConvert.SerializeObject(userDto);
            var content = new StringContent(jsonUserDto, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/Users/update", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(responseContent);
                var response = new
                {
                    Message = apiResponse.Message
                };

                return Json(response);
            }
            else
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(responseContent);
                var response = new
                {
                    Message = apiResponse.Message
                };

                return Json(response);
            }
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
                            ContentLength=userImage.ImagePath.Length,
                            ContentType=new MediaTypeHeaderValue(userImage.ImagePath.ContentType)
                        }
                    },
                    "ImageFile", userImage.ImagePath.FileName);
                    var httpClient = _httpClientFactory.CreateClient();
                    var token = HttpContext.Session.GetString("Token");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/UserImages/update", formContent);
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserImage>>(responseContent);
                    var response = new
                    {
                        Message = apiResponse.Message
                    };

                    return Json(response);
                }
            }

            return RedirectToAction("Index", "User", new { area = "Admin" });
        }

        [HttpPost]
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
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiDataResponse<VerificationCode>>(responseContent);
                var response = new
                {
                    Success = true,
                    Message = apiResponse.Message,
                    Url = "/Admin/User/GetVerifyCode"
                };
                TempData["UserId"] = verificationCodeDto.UserId;
                TempData["Email"] = verificationCodeDto.Email;
                return Json(response);
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetVerifyCode()
        {
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            var model = new VerificationCode
            {

                UserId = (int)TempData["UserId"],
                Email = (string)TempData["Email"]
            };
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
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
                    Url = "/Admin/User/ChangePassword"
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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult ChangePassword()
        {//https://localhost:44347/api/Auth/adminchangepassword
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonInfo = JsonConvert.SerializeObject(changePassword);
            var content = new StringContent(jsonInfo, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/Auth/adminchangepassword", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<ChangePassword>>(responseContent);

                var response = new
                {
                    Success = true,
                    Message = apiDataResponse.Message,
                    Url = "/Admin/User"
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

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("https://localhost:44347/api/Users/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<UserDto>>(responseContent);

                var response = new
                {
                    Message = data.Message
                };

                return Json(response);
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> ExportUsers()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var responseMessage = await httpClient.GetAsync("https://localhost:44347/api/Users/getalldto");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    var dataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<UserDto>>(responseContent);

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Kullanıcılar");
                        worksheet.Cell(1, 1).Value = "ID";
                        worksheet.Cell(1, 2).Value = "Ad";
                        worksheet.Cell(1, 3).Value = "Soyad";
                        worksheet.Cell(1, 4).Value = "Email";
                        worksheet.Cell(1, 5).Value = "TelNo";
                        worksheet.Cell(1, 6).Value = "Cinsiyet";

                        int row = 2;

                        foreach (var userDto in dataResponse.Data)
                        {
                            worksheet.Cell(row, 1).Value = userDto.Id;
                            worksheet.Cell(row, 2).Value = userDto.FirstName;
                            worksheet.Cell(row, 3).Value = userDto.LastName;
                            worksheet.Cell(row, 4).Value = userDto.Email;
                            worksheet.Cell(row, 5).Value = userDto.PhoneNumber;
                            worksheet.Cell(row, 6).Value = userDto.Gender;
                            row++;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Kullannıcılar.xlsx");
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
