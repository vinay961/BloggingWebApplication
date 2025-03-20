namespace BloggingWebApplication.Models
{
    public class DashboardViewModel
    {
        public UserModel User { get; set; } // User details
        public List<BlogModel> Blogs { get; set; } // List of blogs
    }
}
