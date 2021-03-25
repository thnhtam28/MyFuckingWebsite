using Microsoft.EntityFrameworkCore;
using Stories.Models;

namespace Stories.Models
{
    public class ApplicationDbContext : DbContext
    {
        #region  Model 
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Authors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}