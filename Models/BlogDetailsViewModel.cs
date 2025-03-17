namespace BloggingWebApplication.Models
{
    public class BlogDetailsViewModel
    {
        public BlogModel Blog { get; set; }
        public UserModel User { get; set; }
        public List<CommentModel> Comments { get; set; }
    }
}
