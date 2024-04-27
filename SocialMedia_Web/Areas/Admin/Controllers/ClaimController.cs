using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using SocialMedia_Web.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SocialMedia_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClaimController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClaimController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:44347/api/OperationClaims/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<Models.OperationClaim>>(jsonResponse);
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");
                return View(apiDataResponse.Data);
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {

            var responseMessage = await _httpClientFactory.CreateClient().GetAsync($"https://localhost:44347/api/OperationClaims/getclaimsbyid?claimId={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ClaimDto>>(jsonResponse);
                ViewBag.ClaimId = id;
                return View(apiDataResponse.Data);
            }
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("https://localhost:44347/api/OperationClaims/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<Models.OperationClaim>>(responseContent);

                var response = new
                {
                    Message = data.Message
                };

                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Models.OperationClaim oparationClaim)
        {

            var jsonClaim = JsonConvert.SerializeObject(oparationClaim);
            var content = new StringContent(jsonClaim, Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/OperationClaims/add", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Claim", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }


        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update(Models.OperationClaim oparationClaim)
        {
            var jsonClaim = JsonConvert.SerializeObject(oparationClaim);
            var content = new StringContent(jsonClaim, Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PutAsync("https://localhost:44347/api/OperationClaims/update", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<Models.OperationClaim>>(responseContent);

                var response = new
                {
                    Message = data.Message
                };

                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> UserClaimAdd(UserOperationClaim userOparationClaim)
        {
            var jsonClaim = JsonConvert.SerializeObject(userOparationClaim);
            var content = new StringContent(jsonClaim, Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PostAsync("https://localhost:44347/api/UserOperationClaims/add", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<UserOperationClaim>>(responseContent);

                var response = new
                {
                    Message = data.Message
                };

                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UserClaimUpdate(UserOperationClaim userOparationClaim)
        {
            var jsonClaim = JsonConvert.SerializeObject(userOparationClaim);
            var content = new StringContent(jsonClaim, Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.PutAsync("https://localhost:44347/api/UserOperationClaims/update", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<UserOperationClaim>>(responseContent);

                var response = new
                {
                    Message = data.Message
                };

                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> UserClaimDelete(int userId, int claimId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync($"https://localhost:44347/api/UserOperationClaims/delete?userId={userId}&claimId={claimId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<UserOperationClaim>>(responseContent);
                var UserId = HttpContext.Session.GetInt32("UserId");
                if (userId == UserId)
                {
                    var response = new
                    {
                        Message = "Kendi Yetkinizi Kaldırdınız,Lütfen Tekrar Giriş Yapın !",
                        Url = "/Auth/LogOut"
                    };
                    return Json(response);
                }
                var responsee = new
                {
                    Message = data.Message,
                };



                return Json(responsee);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
