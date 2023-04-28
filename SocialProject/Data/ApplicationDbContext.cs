using SocialProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace SocialProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        
        {

        }
        public DbSet<UserModel> UserModels { get; set; }
       // public IEnumerable<object> Usermodel { get; internal set; }
		public DbSet<PostModel> PostModel{ get; set; }
		//public IEnumerable<object> PostModelss { get; internal set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<PostCommentModel> Comments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PostModel>()
				.HasMany(p => p.Comments)
				.WithOne(c => c.Post)
				.HasForeignKey(c => c.PostId);

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IdentityUserLogin<string>>()
				.HasKey(l => new { l.LoginProvider, l.ProviderKey });
		}
	}
}