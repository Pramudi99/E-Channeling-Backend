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
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId);

            modelBuilder.Entity<Doctor>()
               .Property(d => d.ConsultationFee)
               .HasPrecision(18, 2);
        }


    }
}
