using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Data.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(RoleManager<IdentityRole>roleManager ,
            UserManager<ApplicationUser>userManager ,
            ILogger logger,
            CancellationToken ct = default)
        {
            try
            {
                bool hasUsers = await userManager.Users.AnyAsync(ct);
                bool hasRoles = await roleManager.Roles.AnyAsync(ct);

                if (hasUsers && hasRoles) return;


                var roles = new List<IdentityRole>()
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Admin")
            };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed to create role {role.Name} : {string.Join(" ; ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                if (!hasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "AshrafAdmin",
                        UserName = "MohamedAshraf22",
                        Email = "mohamedash@gmail.com",
                        PhoneNumber = "01029207281"
                    };

                    await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Nada",
                        LastName = "AshrafAdmin",
                        UserName = "NadaAshraf22",
                        Email = "Nadaash@gmail.com",
                        PhoneNumber = "01029207243"
                    };

                    await userManager.CreateAsync(Admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(Admin, "Admin");

                    logger.LogInformation("Identity Data seeded");
                }

                return;
            }

            catch(Exception ex)
            {
                logger.LogError(ex, "Identity seeding failed");
                return;
            }
           

            
        }
    }
}
