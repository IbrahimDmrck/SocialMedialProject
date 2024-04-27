using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia_Web.Models;

namespace SocialMedia_Web.Areas.Admin.ViewComponents.AssignAuthority
{
    public class _AssignAuthority : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public _AssignAuthority(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var responseMessage = await _httpClientFactory.CreateClient().GetAsync("https://localhost:44347/api/OperationClaims/getall");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                var apiDataResponse = JsonConvert.DeserializeObject<ApiListDataResponse<OperationClaim>>(jsonResponse);

                var responseMessageUser = await _httpClientFactory.CreateClient().GetAsync("https://localhost:44347/api/Users/getalldto");
                if (responseMessageUser.IsSuccessStatusCode)
                {
                    var jsonResponseuser = await responseMessageUser.Content.ReadAsStringAsync();
                    var apiDataResponseuser = JsonConvert.DeserializeObject<ApiListDataResponse<UserDto>>(jsonResponseuser);

                    var tuple = (apiDataResponse.Data, apiDataResponseuser.Data);
                    return View(tuple);
                }

            }
            return View();
        }
    }
}
