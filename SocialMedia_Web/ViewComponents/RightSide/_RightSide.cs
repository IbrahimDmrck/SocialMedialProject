using Microsoft.AspNetCore.Mvc;

namespace SocialMedia_Web.ViewComponents.RightSide
{
    public class _RightSide : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
