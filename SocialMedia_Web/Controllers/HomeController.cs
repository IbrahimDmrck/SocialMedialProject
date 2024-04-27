using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Diagnostics;

namespace SocialMedia_Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("https://localhost:44347/api/Articles/getarticlewithdetails");
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");


                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);

                int myArticle = apiDataResponse.Data.Count(x => x.UserId == (int)ViewData["UserId"]);
                HttpContext.Session.SetInt32("MyArticle", myArticle);
                ViewData["MyArticle"] = myArticle;

                return apiDataResponse.Success ? View(apiDataResponse.Data) : (IActionResult)View("Veri gelmiyor");
            }

            return View("Veri gelmiyor");
        }


    }
}
