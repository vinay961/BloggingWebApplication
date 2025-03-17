using BloggingWebApplication.Data;
using BloggingWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggingWebApplication.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public BlogController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IActionResult Details(int id)
        {
            var blog = dbContext.Blogs
                .Include(b => b.User)
                .Include(b => b.Comments)
                .ThenInclude(c => c!.User)
                .FirstOrDefault(b => b.Id == id);

            var viewModel = new BlogDetailsViewModel
            {
                Blog = blog,
                User = blog.User,
                Comments = blog.Comments
            };

            return View(viewModel);
        }
        public IActionResult AddBlog()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBlog(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("UserId");

                if(userId != null)
                {
                    blog.UserId = Convert.ToInt32(userId);
                    dbContext.Blogs.Add(blog);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
    }
}
