using Microsoft.AspNetCore.Mvc;

namespace SocialMedia_Web.Controllers
{
    public class ProfileController : Controller
    {
        [HttpGet("profilim")]
        public IActionResult Profile()
        {
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            return View();
        }
    }
}
