using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingWebApplication.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

        [InverseProperty("User")]
        public List<BlogModel>? Blogs { get; set; }

        [InverseProperty("User")]
        public List<CommentModel>? Comments { get; set; }
    }
}
