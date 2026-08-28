using E_Channeling.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Channeling.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        
    }
}
