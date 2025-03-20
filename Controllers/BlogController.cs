using BloggingWebApplication.Data;
using BloggingWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace BloggingWebApplication.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        IWebHostEnvironment _env;
        public BlogController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            this.dbContext = dbContext;
            _env = env;
        }

        public IActionResult Details(int id)
        {
            var blog = dbContext.Blogs
                .Include(b => b.User)
                .Include(b => b.Comments)
                .ThenInclude(c => c!.User)
                .FirstOrDefault(b => b.Id == id);

            if (blog == null)
            {
                return NotFound(); 
            }

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
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill all the fields";
                return View(blog);
            }

            string fileName = "";

            if (blog.ImageFile != null)
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "images");

                // Ensure the directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Generate unique file name
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(blog.ImageFile.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    blog.ImageFile.CopyTo(fileStream);
                }

                // Store relative path in database
                blog.ImagePath = "/images/" + fileName;
            }
            else
            {
                ViewBag.Error = "Please upload an image";
                return View(blog);
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                blog.UserId = Convert.ToInt32(userId);
                dbContext.Blogs.Add(blog);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            ViewBag.Error = "User session expired";
            return View(blog);
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
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill all the fields";
                return View(blog);
            }

            var existingBlog = dbContext.Blogs.FirstOrDefault(b => b.Id == blog.Id);
            if (existingBlog == null)
            {
                return NotFound();
            }

            existingBlog.Title = blog.Title;
            existingBlog.Content = blog.Content;

            if (blog.ImageFile != null) // If user uploads a new image
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "images");

                // Ensure the directory exists
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Generate unique file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(blog.ImageFile.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(existingBlog.ImagePath))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, existingBlog.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the new file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    blog.ImageFile.CopyTo(fileStream);
                }

                // Update the database with the new image path
                existingBlog.ImagePath = "/images/" + fileName;
            }

            dbContext.SaveChanges();
            return RedirectToAction("Dashboard", "Home");
        }
    }
}
