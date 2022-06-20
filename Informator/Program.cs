using Informator.Database;
using Informator.Hubs;
using Informator.Models;
using Informator.SendServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Informator;
public class Program {
    public static IServiceProvider ServiceProvider;

    public static void Main(string[]? args) {
        var builder = WebApplication.CreateBuilder(args);

        Configure(builder);

        var app = builder.Build();

        ServiceProvider = app.Services;

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        else {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        
        app.UseEndpoints(endpoints => 
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=SendMessPage}/{id?}");
            endpoints.MapHub<NotificationHub>("/notification");
        });
        app.Run();
    }

    private static void Configure(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var services = builder.Services;

        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        services
            .AddDbContext<InformatorContext>(opt => {
                opt.UseSqlServer(connectionString);
                opt.EnableSensitiveDataLogging();
            });

        services
            .AddIdentity<UserIdentity, UserIdentityRole>(opt => {
                // Password settings.
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.AllowedForNewUsers = true;

                // User settings.
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opt.User.RequireUniqueEmail = true;

            })
            .AddEntityFrameworkStores<InformatorContext>()
            .AddDefaultTokenProviders();

        services
            .ConfigureApplicationCookie(options => {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.SlidingExpiration = true;
            });

        services
            .AddAuthentication(opt => {
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

        services.AddSignalR();
        services.AddControllersWithViews();
        services.AddSingleton<HandlerManager>();
    }
}