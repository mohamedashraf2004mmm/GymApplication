using GymApplication.BLL;
using GymApplication.BLL.Services.Attachments;
using GymApplication.BLL.Services.Classes;
using GymApplication.BLL.Services.Interfaces;
using GymApplication.DAL;
using GymApplication.DAL.Data.DataSeeding;
using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Classes;
using GymApplication.DAL.Repositories.Interfaces;
using GymApplication.PL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped(typeof(IGenericRepository<>) , typeof(GenericRepository<>));
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

           // builder.Services.AddScoped(typeof(IMemberService), typeof(MemberService));

            builder.Services.AddScoped<IMemberService , MemberService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService , TrainerService>();
            builder.Services.AddScoped<ISessionService , SessionService>();

            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository , SessionRepository>();

            builder.Services.AddScoped<IAttachmentService, AttachmentService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                //config.Password.RequireLowercase = true;
                //config.Password.RequireUppercase = true;
                //config.Password.RequiredLength = 6;

                config.User.RequireUniqueEmail = true;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                config.Lockout.MaxFailedAccessAttempts = 5;

            }).AddEntityFrameworkStores<GymDbContext>();

           

            var app = builder.Build();

            await app.MigrateAndSeedDatabaseAsync();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
