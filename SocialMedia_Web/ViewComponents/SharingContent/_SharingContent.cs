using Microsoft.AspNetCore.Mvc;

namespace SocialMedia_Web.ViewComponents.SharingContent
{
    public class _SharingContent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
