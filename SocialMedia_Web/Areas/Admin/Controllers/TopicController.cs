using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialMedia_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopicController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TopicController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65527/api/Topics/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<Topics>>(jsonResponse);
                return apiDataResponse.Success ? View(apiDataResponse.Data) : View("Veri gelmiyor");
            }
            return View("Veri gelmiyor");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await httpClient.DeleteAsync("http://localhost:65527/api/Topics/delete?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<Topics>>(jsonResponse);

                var response = new
                {
                    Message = apiDataResponse.Message,
                    Url = "/Admin/Topic/Index"
                };
                return Json(response);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetUpdateTopic(int id)
        {

            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65527/api/Topics/getbyid?id="+id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse1 = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<Topics>>(jsonResponse1);


                return View(apiDataResponse.Data);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateTopic(Topics topics)
        {
            var jsonTopic = JsonConvert.SerializeObject(topics);
            var content = new StringContent(jsonTopic, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _httpClientFactory.CreateClient().PutAsync("http://localhost:65527/api/Topics/update", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiDataResponse<Topics>>(responseContent);
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
        public async Task<IActionResult> AddTopic(Topics topics)
        {
            var jsonTopic = JsonConvert.SerializeObject(topics);
            var content = new StringContent(jsonTopic, Encoding.UTF8, "application/json");
            var token = HttpContext.Session.GetString("Token");
            _httpClientFactory.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _httpClientFactory.CreateClient().PostAsync("http://localhost:65527/api/Topics/add",content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Topic", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home", new {area="Admin"});
        }
    }
}
