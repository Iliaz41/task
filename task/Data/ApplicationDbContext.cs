using Microsoft.EntityFrameworkCore;
using task.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace task.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role[]
                {
                    new Role { Id=1, Status="User"},
                    new Role { Id=2, Status="Admin"},
                    new Role { Id=3, Status="Support"},
                    new Role { Id=4, Status="SuperAdmin"},
                });

            modelBuilder.Entity<User>()
                .HasMany(a => a.Roles)
                .WithMany(b => b.Users)
                .UsingEntity(j =>
                {
                    j.ToTable("UserRole");
                });

            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(e => e.Status)
                .IsUnique();
        }
    }
}
