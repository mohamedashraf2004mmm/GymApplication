using GymApplication.DAL.Data.DataSeeding;
using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymApplication.PL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Applying {pendingMigrations.Count()} pending migrations");
                await dbContext.Database.MigrateAsync();
            }


            //C:\Users\FIRST\source\repos\GymApplication\GymApplication\wwwroot\files\plans.json
            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "files");
            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);

           await IdentityDataSeeding.SeedIdentityDataAsync(roleManager, userManager, logger);
        }
    }
}
