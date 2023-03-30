using SocialProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public IEnumerable<object> Usermodel { get; internal set; }
    }
}