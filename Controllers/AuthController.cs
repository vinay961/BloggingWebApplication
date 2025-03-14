using Microsoft.AspNetCore.Mvc;

namespace BloggingWebApplication.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
