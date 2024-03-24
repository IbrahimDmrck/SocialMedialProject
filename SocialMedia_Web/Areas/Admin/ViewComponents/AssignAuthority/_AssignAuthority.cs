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
            var responseMessageClaim = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/OperationClaims/getall");
            if (responseMessageClaim.IsSuccessStatusCode)
            {
                var jsonResponseClaim = await responseMessageClaim.Content.ReadAsStringAsync();
                var apiDataResponseClaim = JsonConvert.DeserializeObject<ApiListDataResponse<OperationClaim>>(jsonResponseClaim);

                var responseMessageUser = await _httpClientFactory.CreateClient().GetAsync("http://localhost:65526/api/Users/getalldto");
                if (responseMessageUser.IsSuccessStatusCode)
                {
                    var jsonResponseUser = await responseMessageUser.Content.ReadAsStringAsync();
                    var apiDataResponseUser = JsonConvert.DeserializeObject<ApiListDataResponse<UserDto>>(jsonResponseUser);

                    var tuple = (apiDataResponseClaim.Data, apiDataResponseUser.Data);
                    return View(tuple);
                }

            }
            return View();
        }
    }
}
