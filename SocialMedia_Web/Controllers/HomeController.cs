using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Diagnostics;

namespace SocialMedia_Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Index(string? message, bool? success)
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("http://localhost:65525/api/Articles/getarticlewithdetails");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse<ArticleDetailDto>>(jsonResponse);
                ViewData["Message"] = TempData["Message"];
                ViewData["Success"] = TempData["Success"];
                return apiDataResponse.Success ? View(apiDataResponse.Data) : (IActionResult)View("Veri gelmiyor");
            }
            return View("Veri gelmiyor");
        }
    }
}