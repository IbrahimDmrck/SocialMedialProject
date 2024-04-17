using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;
using System.Net.Http;

namespace SocialMedia_Web.Areas.Admin.ViewComponents.GetClaimComponent
{
  
    public class _GetClaimComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _GetClaimComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65527/api/OperationClaims/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<OperationClaim>>(jsonResponse);

                return View(apiDataResponse.Data);
            }
            return View("Veri gelmiyor");
        }
    }
}
