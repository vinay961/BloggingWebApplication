using Microsoft.AspNetCore.Mvc;

namespace BloggingWebApplication.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Blog()
        {
            return View();
        }
    }
}
