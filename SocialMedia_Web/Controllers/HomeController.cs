using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Diagnostics;

namespace SocialMedia_Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync("http://localhost:65525/api/Articles/getarticlewithdetails");
            if (responseMessage.IsSuccessStatusCode)
            {
                //var jsonEmployee = await responseMessage.Content.ReadAsStringAsync();
                //var values = JsonConvert.DeserializeObject<ArticleDetailDto>(jsonEmployee);
                //return View(values);
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiDataResponse>(jsonResponse);

                if (apiDataResponse.Success)
                {
                    return View(apiDataResponse.Data);
                }
                else
                {
                    return View("Veri gelmiyor");
                }
            }
            return View("Veri gelmiyor");
        }
    }
}