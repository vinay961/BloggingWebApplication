using BloggingWebApplication.Data;
using BloggingWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

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

        [HttpPost]
        public IActionResult AddComment(int BlogId, string Content)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if(userId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var comment = new CommentModel
            {
                Content = Content,
                BlogId = BlogId,
                UserId = Convert.ToInt32(userId)
            };
            dbContext.Comments.Add(comment);
            dbContext.SaveChanges();
            return RedirectToAction("Details", new { id = BlogId });
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
        public IActionResult DeleteBlog(int id)
        {
            var blog = dbContext.Blogs.FirstOrDefault(b => b.Id == id);
            dbContext.Blogs.Remove(blog);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard", "Home");
        }
        public IActionResult EditBlog(int id)
        {
            var blog = dbContext.Blogs.FirstOrDefault(b => b.Id == id);
            return View(blog);
        }
        [HttpPost]
        public IActionResult EditBlog(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                var existingBlog = dbContext.Blogs.FirstOrDefault(b => b.Id == blog.Id);
                existingBlog.Title = blog.Title;
                existingBlog.Content = blog.Content;
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }
            return View(blog);
        }
    }
}
