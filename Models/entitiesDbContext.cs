using Microsoft.EntityFrameworkCore;
using projectX.Models;

namespace projectX.Models
{
    public class entitiesDbContext : DbContext
    {
        public entitiesDbContext () { 

        }

        //constructor
        //to pass the options to the base class
        public entitiesDbContext (DbContextOptions option) : base (option)
        {

        }
        public DbSet<Student> students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
       public DbSet<UserRole> RoleUsers { get; set; }


        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer ("Server=SC-202502021905;Database=Edu;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True");
            base.OnConfiguring (optionsBuilder);
        }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship
            modelBuilder.Entity<UserRole> ()
                .HasKey (ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole> ()
                .HasOne (ur => ur.User)
                .WithMany (u => u.UserRoles)
                .HasForeignKey (ur => ur.UserId);

            modelBuilder.Entity<UserRole> ()
                .HasOne (ur => ur.Role)
                .WithMany (r => r.UserRoles)
                .HasForeignKey (ur => ur.RoleId);

            // Seed roles
            modelBuilder.Entity<Role> ()
                .HasData (
                    new Role { Id = 1, Name = "Admin" },
                    new Role { Id = 2, Name = "Student" },
                    new Role { Id = 3, Name = "Instructor" }
                );

        }    
       
    }
}
