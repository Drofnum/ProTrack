using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ProTrack.Data;
using ProTrack.Data.Models;

[assembly: HostingStartup(typeof(ProTrack.Areas.Identity.IdentityHostingStartup))]
namespace ProTrack.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {


            builder.ConfigureServices((context, services) =>
            {
                var dbServer = context.Configuration.GetValue<string>("dbServer");
                var dbUsername = context.Configuration.GetValue<string>("dbUsername");
                var dbPassword = context.Configuration.GetValue<string>("dbPassword");

                services.AddDbContext<ApplicationDbContext>(options => options
                .UseMySql($"server={dbServer};port=3306;database=ProTrack;user={dbUsername};password={dbPassword}",
                mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(8, 0, 17), ServerType.MySql)
                    .DisableBackslashEscaping();

                }
                    ));


                services.AddDefaultIdentity<ApplicationUser>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

                services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));

            });
        }
    }
}