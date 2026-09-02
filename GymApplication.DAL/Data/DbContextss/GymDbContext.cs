using GymApplication.DAL.Data.Configurations;
using GymApplication.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymApplication.DAL.Data.DbContextss
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
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
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PlanConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>(eb =>
            {
                eb.Property(x => x.FirstName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);

                eb.Property(x => x.LastName)
                  .HasColumnType("varchar")
                  .HasMaxLength(50);
            });
        }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }

        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }


    }
}
