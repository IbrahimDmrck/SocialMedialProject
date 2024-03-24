using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia_Web.ViewComponents.NotSeenCommentComponent
{
    //getarticlewithdetailsbyuserid
    public class _NotSeenCommentComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public _NotSeenCommentComponent(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            int userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId") ?? 0;

            var responseMessage = await httpClient.GetAsync("http://localhost:65526/api/Articles/getarticlewithdetailsbyuserid?id=" + userId);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponseMyArticle = JsonConvert.DeserializeObject<ApiListDataResponse<ArticleDetailDto>>(jsonResponse);

                int notSeenComment = apiDataResponseMyArticle.Data
                                     .SelectMany(article => article.CommentDetails)
                                     .Count(comment => comment.Status == false && comment.UserId !=userId && comment.Id != -1); 


                return View(notSeenComment);
            }

            return View();
        }
    }
}
