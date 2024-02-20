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
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("http://localhost:65525/api/Articles/getarticlewithdetails");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);
                ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");

                int myArticleCount = apiDataResponse.Data.Count(x => x.UserId == (int)ViewData["UserId"]);
                
                HttpContext.Session.SetInt32("MyArticle", apiDataResponse.Data.Count(x => x.UserId == (int)ViewData["UserId"]));
                ViewData["MyArticle"] = myArticleCount;

               

                return apiDataResponse.Success ? View(apiDataResponse.Data) : (IActionResult)View("Veri gelmiyor");
            }
            return View("Veri gelmiyor");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> PartialRightSide()
        {

            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("http://localhost:65525/api/Articles/getarticlewithdetails");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);

                //// TopicTitle'lara göre gruplama yap
                //var groupedByTopicTitle = apiDataResponse.Data.GroupBy(x => x.TopicId);

                //// Her TopicTitle için tekrar sayýsýný al
                //var topicTitleCounts = groupedByTopicTitle.Select(x => new
                //{
                //    TopicId = x.Key,
                //    Count = x.Count()
                //}).ToList();

                return PartialView() ;
            }
            return PartialView();
        }
    }
}
