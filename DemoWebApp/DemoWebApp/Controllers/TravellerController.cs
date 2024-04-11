using Microsoft.AspNetCore.Mvc;

namespace DemoWebApp.Controllers
{
    public class TravellerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
