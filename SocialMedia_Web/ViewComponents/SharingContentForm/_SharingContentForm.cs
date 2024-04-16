using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;

namespace SocialMedia_Web.ViewComponents.SharingContentForm
{
    public class _SharingContentForm : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _SharingContentForm(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
    }
}
