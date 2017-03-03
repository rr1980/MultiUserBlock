using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MultiUserBlock.Common.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using MultiUserBlock.Web.Authorization;
using MultiUserBlock.Common.Enums;
using MultiUserBlock.DB;
using MultiUserBlock.WebSockets;
using System.Reflection;
using MultiUserBlock.Web.WebSocketHandlers;

namespace MultiUserBlock.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.

            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ReadPolicy", policy => policy.Requirements.Add(new AuthPolicyRequirement(UserRoleType.Default)));
                options.AddPolicy("AdminPolicy", policy => policy.Requirements.Add(new AuthPolicyRequirement(UserRoleType.Admin)));
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationHandler, AuthPolicyHandler>();
            // Add framework services.


            services.AddMvc();

            services.AddWebSocketManager(GetType().GetTypeInfo().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, DataContext ctx, IServiceProvider serviceProvider, IHostingEnvironment env = null, ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory != null)
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            app.UseWebSockets();

            if (env != null)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseDatabaseErrorPage();
                    app.UseBrowserLink();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                LoginPath = "/Account/Login",
                AccessDeniedPath = "/Account/Login",
                LogoutPath = "/Account/Logout",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieSecure = CookieSecurePolicy.SameAsRequest,
                //CookiePath = "/",
                CookieHttpOnly = true,
                SlidingExpiration = true,
                CookieName = "rrAuth",
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapWebSocketManager("/admins", serviceProvider.GetService<AdminMessageHandler>());
            app.MapWebSocketManager("/notifications", serviceProvider.GetService<NotificationsMessageHandler>());


            SeedData.Seed(ctx);
        }
    }
}
