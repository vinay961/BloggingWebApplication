using System.ComponentModel.DataAnnotations;

namespace BloggingWebApplication.Models
{
    public class UserModel
    {

        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public List<BlogModel>? Blogs { get; set; }
        public List<CommentModel>? Comments { get; set; } 
    }
}
