using Microsoft.AspNetCore.Mvc;
using SocialMedia_Web.Models;
using System.Diagnostics;

namespace SocialMedia_Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}