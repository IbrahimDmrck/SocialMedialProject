using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Net.Http;

namespace SocialMedia_Web.ViewComponents.RightSide
{
    public class _RightSide : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _RightSide(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65525/api/Topics/getall");
        //    if (responseMessage.IsSuccessStatusCode)
        //    {
        //        var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
        //        var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<Topics>>(jsonResponse);


        //        var responseMessage1 = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65525/api/Articles/getarticlewithdetails");
        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            var jsonResponse1 = await responseMessage.Content.ReadAsStringAsync();
        //            var apiDataResponse1 = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);


        //        }

        //        return apiDataResponse.Success ? View(apiDataResponse.Data) : View();
        //    }
        //    return View();
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/Topics/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<Topics>>(jsonResponse);

                var responseMessage1 = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/Articles/getarticlewithdetails");
                if (responseMessage1.IsSuccessStatusCode)
                {
                    var jsonResponse1 = await responseMessage1.Content.ReadAsStringAsync();
                    var apiDataResponse1 = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse1);

                    // TopicTitle'lara göre gruplama yap
                    var groupedByTopicTitle = apiDataResponse1.Data.GroupBy(x => x.TopicTitle);

                    // Her TopicTitle için Article sayısını ve Topic'i bul
                    var topicArticleCounts = apiDataResponse.Data.Select(topic =>
                    {
                        var articleCount = groupedByTopicTitle.FirstOrDefault(x => x.Key == topic.TopicTitle)?.Count() ?? 0;
                        return Tuple.Create(topic, articleCount);
                    }).ToList();

                    return View(topicArticleCounts);
                }
            }
            return View();
        }

    }
}
