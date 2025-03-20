using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggingWebApplication.Models
{
    public class BlogModel
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }

        // Foreign key for User
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        // One blog can have many comments
        public List<CommentModel>? Comments { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
