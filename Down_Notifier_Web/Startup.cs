using Down_Notifier_Web.Data;
using Down_Notifier_Web.Util;
using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Down_Notifier_Web
{
    public class Startup
    {
        private static IServiceCollection Services { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            services.AddHttpClient();
            services.AddTransient<EmailService>();
            services.AddElmah();

            services.AddElmah<XmlFileErrorLog>(options =>
            {
                options.LogPath = "~/log"; // OR options.LogPath = "с:\errors";
            });
            Services = services;

            services.AddAuthentication()
               .AddGoogle(options =>
               {
                   options.ClientId = "393470205694-ce21st86ec9kq46vti1teek8hs4i7b5l.apps.googleusercontent.com";
                   options.ClientSecret = "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";
               });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseElmah();
            AppDomain.CurrentDomain.UnhandledException += (sender, ex) =>
            {

               

            };

            var initializer = new Initializer();
            Task.Run(() => initializer.InitializePingServices());
        }

        public static T GetRequiredService<T>(IServiceScope scope = null)
        {
            scope ??= Services.BuildServiceProvider().CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }
    }
}
