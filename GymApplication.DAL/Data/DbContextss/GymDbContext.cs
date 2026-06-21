using GymApplication.DAL.Data.Configurations;
using GymApplication.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymApplication.DAL.Data.DbContextss
{
    public class GymDbContext : DbContext
    {


        public GymDbContext(DbContextOptions<GymDbContext>options):base(options) 
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //        "Server=.;Database=GymManagement;Trusted_Connection=true;TrustServerCertificate=true");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlanConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Booking> Bookings { get; set; }


    }
}
