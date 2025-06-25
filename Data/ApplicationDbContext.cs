using BE_MEGA_PROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_MEGA_PROJECT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options) { }
        public DbSet<User> Users { get; set; }
       
    }
}
