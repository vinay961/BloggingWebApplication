using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggingWebApplication.Models;
using BloggingWebApplication.Data;

namespace BloggingWebApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext dbContext;

    public HomeController(ILogger<HomeController> logger,ApplicationDbContext dbContext)
    {
        _logger = logger;
        this.dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var sampleBlogs = new List<BloggingWebApplication.Models.BlogModel>
        {
        new BlogModel { Id = 1, Title = "C# for Beginners", Content = "Learn the basics of C# programming." },
        new BlogModel { Id = 2, Title = "ASP.NET Core MVC", Content = "Step-by-step guide to building web applications." },
        new BlogModel { Id = 3, Title = "Entity Framework", Content = "How to handle database operations in ASP.NET." }
        };

        return View(sampleBlogs);
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register(UserModel user)
    {
        if (ModelState.IsValid)
        {
            // Save user to database
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }
        return View(); 
    }
    
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(UserModel user)
    {
        // Sample data for checking the functionality

        var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
        if (userInDb != null)
        {
            return RedirectToAction("Dashboard");
        }
        return View();
    }

    public IActionResult Dashboard()
    {
        var sampleBlogs = new List<BloggingWebApplication.Models.BlogModel>
        {
        new BlogModel { Id = 1, Title = "C# for Beginners", Content = "Learn the basics of C# programming." },
        new BlogModel { Id = 2, Title = "ASP.NET Core MVC", Content = "Step-by-step guide to building web applications." },
        new BlogModel { Id = 3, Title = "Entity Framework", Content = "How to handle database operations in ASP.NET." }
        };
        
        return View(sampleBlogs);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
