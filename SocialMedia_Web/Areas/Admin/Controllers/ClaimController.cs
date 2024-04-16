using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> Index()
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65527/api/OperationClaims/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<OperationClaim>>(jsonResponse);

                return apiDataResponse.Success ? View(apiDataResponse.Data) : View("Veri gelmiyor");
            }
            return RedirectToAction("Index", "Claim", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Add(OperationClaim operationClaim)
        {
            var jsonClaim = JsonConvert.SerializeObject(operationClaim);
            var content = new StringContent(jsonClaim, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("http://localhost:65527/api/OperationClaims/add", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Claim", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Claim", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65527/api/OperationClaims/getclaimbyusers?claimId=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.ClaimId = id;
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ClaimDto>>(jsonResponse);

                return View(apiDataResponse.Data);
            }
            return RedirectToAction("Index", "Claim", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UserClaimUpdate(UserOperationClaim userOperationClaim)
        {
            var jsonData = JsonConvert.SerializeObject(userOperationClaim);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _httpClientFactory.CreateClient().PutAsync("http://localhost:65527/api/UserOperationClaims/update", content);
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
            return RedirectToAction("Index", "Claim", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> UserClaimAdd(UserOperationClaim userOperationClaim)
        {
            var jsonData = JsonConvert.SerializeObject(userOperationClaim);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("http://localhost:65527/api/UserOperationClaims/add", content);
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
            return RedirectToAction("Index", "Claim", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> ClaimUpdate(OperationClaim operationClaim)
        {
            var jsonData = JsonConvert.SerializeObject(operationClaim);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _httpClientFactory.CreateClient().PutAsync("http://localhost:65527/api/OperationClaims/update", content);
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
            return RedirectToAction("Index", "Claim", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("http://localhost:65527/api/OperationClaims/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<ClaimDto>>(jsonResponse);

                var response = new
                {
                    Message = apiDataResponse.Message
                };
                return Json(response);
            }
            return RedirectToAction("Index", "Claim", new { area = "Admin" });
        }

        
        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> UserClaimDelete(int userId,int claimId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync($"http://localhost:65527/api/UserOperationClaims/delete?userId={userId}&claimId={claimId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<UserOperationClaim>>(jsonResponse);

                var response = new
                {
                    Message = apiDataResponse.Message
                };
                return Json(response);
            }
            return RedirectToAction("Index", "Claim", new { area = "Admin" });

        }
    }
}
