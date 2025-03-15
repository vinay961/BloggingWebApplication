using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingWebApplication.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentId { get; set; }
        public string? Content { get; set; }

        // Foreign key for Blog
        public int BlogId { get; set; }

        [ForeignKey("BlogId")]
        public BlogModel? Blog { get; set; }

        // Foreign key for User
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserModel? User { get; set; }
    }
}
