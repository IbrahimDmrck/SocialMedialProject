using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;

namespace SocialMedia_Web.ViewComponents.ArticleUpdateComponent
{
    public class _ArticleUpdateComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _ArticleUpdateComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65527/api/Topics/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<Topics>>(jsonResponse);

                var responseMessage1 = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65527/api/Articles/getbyid?id=" + id);
                if (responseMessage1.IsSuccessStatusCode)
                {
                    var jsonResponse1 = await responseMessage1.Content.ReadAsStringAsync();
                    var apiDataResponse1 = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse1);

                   

                    return View();
                }
            }
            return View();
        }
    }
}
