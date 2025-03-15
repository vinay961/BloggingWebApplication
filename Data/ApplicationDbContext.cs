using BloggingWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BloggingWebApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BlogModel> Blogs { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Blog -> User (with Cascade Delete)
            modelBuilder.Entity<BlogModel>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // ✅ Works fine

            // Comment -> User (No Action on Delete to avoid foreign key conflict)
            modelBuilder.Entity<CommentModel>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);  // ✅ Works fine

            // Comment -> Blog (Cascade Delete when Blog is deleted)
            modelBuilder.Entity<CommentModel>()
                .HasOne(c => c.Blog)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade);  // ✅ Works fine
        }
    }
}
