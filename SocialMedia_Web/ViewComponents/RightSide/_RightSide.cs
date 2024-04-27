using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using SocialMedia_Web.Models;

namespace SocialMedia_Web.ViewComponents.RightSide
{
    public class _RightSide : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _RightSide(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:44347/api/Topics/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<Topics>>(jsonResponse);
                var trueTopic = apiDataResponse.Data.Where(x => x.Status == true).ToList();
                var responseArticleMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:44347/api/Articles/getarticlewithdetails");
                if (responseArticleMessage.IsSuccessStatusCode)
                {
                    var jsonArticleResponse = await responseArticleMessage.Content.ReadAsStringAsync();
                    var apiArticleDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonArticleResponse);

                    var groupByTopicTitle = apiArticleDataResponse.Data.GroupBy(x=>x.TopicTitle);

                    var topicArticleCounts = trueTopic.Select(topic =>
                    {
                        var articleCount = groupByTopicTitle.FirstOrDefault(x => x.Key == topic.TopicTitle)?.Count() ?? 0;
                        return Tuple.Create(topic, articleCount);
                    }).ToList();

                    return View(topicArticleCounts);
                }
            }
            return View();
        }
    }
}
