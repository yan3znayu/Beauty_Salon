using Microsoft.EntityFrameworkCore;
using Beauty_Salon.Models;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Beauty_Salon.DAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Statuses> Statuses { get; set; }
        public DbSet<Specialists> Specialists { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Favorites> Favorites { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.Role_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Users>()
                .HasOne(u => u.Status)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.Status_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grades>()
                .HasOne(g => g.Specialist)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.Specialists_ID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Grades>()
                .HasOne(g => g.User)
                .WithMany(u => u.Grades)
                .HasForeignKey(g => g.User_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservations>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.User_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservations>()
                .HasOne(r => r.Specialist)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.Specialist_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservations>()
                .HasOne(r => r.Service)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.Service_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorites>()
                .HasOne(f => f.User) 
                .WithMany(u => u.Favorites) 
                .HasForeignKey(f => f.User_ID) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorites>()
                .HasOne(f => f.Service)
                .WithMany(s => s.Favorites)
                .HasForeignKey(f => f.Service_ID)
                .OnDelete(DeleteBehavior.Cascade);

        }


        public DatabaseContext() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}
