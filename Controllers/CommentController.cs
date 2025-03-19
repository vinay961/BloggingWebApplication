using BloggingWebApplication.Data;
using BloggingWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace BloggingWebApplication.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CommentController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Method for editing the comment
        public IActionResult EditComment(int id)
        {
            var comment = dbContext.Comments.Find(id);
            return View(comment);
        }
        public IActionResult EditComment(CommentModel comment)
        {
            var c = dbContext.Comments.Find(comment.CommentId);
            c.Content = comment.Content;
            dbContext.SaveChanges();
            return RedirectToAction("Details", "Blog", new { id = c.BlogId });
        }

        // Method for deleting the comment
        public IActionResult DeleteComment(int commentId)
        {
            if (commentId == 0)
            {
                TempData["Message"] = "Current comment Id is zero.";
                return RedirectToAction("Index", "Home");
            }

            var comment = dbContext.Comments.Find(commentId);
            if (comment == null)
            {
                TempData["Message"] = "Comment not found with the given commentId.";
                return RedirectToAction("Index", "Home");
            }

            dbContext.Comments.Remove(comment);
            dbContext.SaveChanges();
            return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
        }
    }
}
