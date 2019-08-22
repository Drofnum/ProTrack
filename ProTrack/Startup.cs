using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(o => {
                o.ExpireTimeSpan = TimeSpan.FromDays(5);
                o.SlidingExpiration = true;
            });

            var dbServer = Configuration.GetValue<string>("dbServer");
            var dbUsername = Configuration.GetValue<string>("dbUsername");
            var dbPassword = Configuration.GetValue<string>("dbPassword");

            services.AddDbContext<ApplicationDbContext>(options => options
            .UseMySql($"server={dbServer};port=3306;database=ProTrack;user={dbUsername};password={dbPassword}",
            mySqlOptions =>
            {
                mySqlOptions.ServerVersion(new Version(8, 0, 17), ServerType.MySql)
                .DisableBackslashEscaping();

            }
                ));

            services.AddScoped<IApplicationUser, ApplicationUserService>();
            services.AddScoped<IDevice, DeviceService>();
            services.AddScoped<IEntry, EntryService>();
            services.AddScoped<ILocation, LocationService>();
            services.AddScoped<IProduct, ProductService>();

            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<AuthMessageSenderOptions>(Configuration);


            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDataProtection()
                .SetApplicationName("Protrack")
                .PersistKeysToFileSystem(new DirectoryInfo(@"/var/dpkeys/"));

            /*
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);

            });
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            
            //CreateUsers(serviceProvider).Wait();
            CreateUserRoles(serviceProvider).Wait();

        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var roleCheckActive = await RoleManager.RoleExistsAsync("Active");
            if (!roleCheckActive)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Active"));
            }

            var roleCheckPending = await RoleManager.RoleExistsAsync("PendingActivation");
            if (!roleCheckPending)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("PendingActivation"));
            }

            var roleCheckInactive = await RoleManager.RoleExistsAsync("Inactive");
            if (!roleCheckInactive)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Inactive"));
            }

            //Assign Admin to main user
            ApplicationUser user = await UserManager.FindByEmailAsync("amunford@control4.com");
            await UserManager.AddToRoleAsync(user, "Admin");
            await UserManager.AddToRoleAsync(user, "Active");

            //ApplicationUser user2 = await UserManager.FindByEmailAsync("echaosaj@gmail.com");
            //await UserManager.AddToRoleAsync(user2, "PendingActivation");
        }

        private async Task CreateUsers(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            using (var reader = new StreamReader("/Users.csv"))
                using (var csv = new CsvReader(reader))
            {
                var records = new List<ApplicationUser>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new ApplicationUser
                    {
                        UserName = csv.GetField("Email"),
                        Email = csv.GetField("Email"),
                        FirstName = csv.GetField("FullName"),
                        LastName = csv.GetField("FullName"),
                        EmailConfirmed = true
                    };
                    if (!context.Users.Any(u => u.UserName == record.UserName))
                    {
                        var result = await UserManager.CreateAsync(record, "Super duper Mega secret passw0rd$");
                    }
                }

            }
        }

            public void ApplyMigrations(ApplicationDbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
