using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingWebApplication.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentId { get; set; }
        public string? Content { get; set; }

        // Foreign key for the blog which the comment belongs to
        public int BlogId { get; set; }
        [ForeignKey("BlogId")]
        public BlogModel? Blog { get; set; } // Navigation property, means that a comment belongs to a blog

        // Foreign key for the user who post the comment
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel? User { get; set; } // Navigation property, means that a comment belongs to a user, now question is that what happen if one comment belongs to multiple users, in that case we can use ICollection<UserModel> instead of UserModel
    }
}
