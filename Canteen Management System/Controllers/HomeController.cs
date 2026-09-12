using Microsoft.AspNetCore.Mvc;

namespace CanteenWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Customer()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }
    }
}