using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggingWebApplication.Models;
using BloggingWebApplication.Data;
using Microsoft.EntityFrameworkCore;

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
        var allBlogs = dbContext.Blogs.ToList();

        return View(allBlogs);
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

        var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password); // it return null if no user found, else return the user
        if (userInDb != null)
        {
            HttpContext.Session.SetString("UserId", userInDb.UserId.ToString());
            HttpContext.Session.SetString("Username", userInDb.Username);
            return RedirectToAction("Dashboard");
        }
        return View("Login");
    }

    public IActionResult Dashboard()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if(userId == null)
        {
            return RedirectToAction("Login");
        }
        var user = dbContext.Users.FirstOrDefault(u => u.UserId == Convert.ToInt32(userId));
        var blogs = dbContext.Blogs.Include(b => b.User).Where(b => b.UserId == Convert.ToInt32(userId)).ToList();

        var model = new DashboardViewModel
        {
            User = user,
            Blogs = blogs
        };

        return View(model);
    }

    public IActionResult EditProfile(int id)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.UserId == id);
        return View(user);
    }
    [HttpPost]
    public IActionResult EditProfile(UserModel user)
    {
        if (user != null)
        {
            var existingUser = dbContext.Users.FirstOrDefault(u => u.UserId == user.UserId);
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;

            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        ViewBag.Message = "Invalid data";
        return View(user);
    }

    public IActionResult DeleteProfile(int id)
    {
        var user = dbContext.Users
            .Include(u => u.Blogs)
            .Include(u => u.Comments)
            .FirstOrDefault(u => u.UserId == id);

        try
        {
            dbContext.Comments.RemoveRange(user.Comments);
            dbContext.Blogs.RemoveRange(user.Blogs);
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
            return RedirectToAction("Logout");
        }
        catch (Exception ex)
        {
            ViewBag.Message = ex.Message;
            return View();
        }
    }
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
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
