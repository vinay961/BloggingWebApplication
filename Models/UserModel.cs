using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingWebApplication.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        [InverseProperty("User")]
        public List<BlogModel>? Blogs { get; set; }

        [InverseProperty("User")]
        public List<CommentModel>? Comments { get; set; }
    }
}
