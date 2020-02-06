using DrinkAndGo.Data;
using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using DrinkAndGo.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DrinkAndGo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container and container will provide the instance when required.
        // Here we will also define the lifeline of the instance.

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DrinkDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Need to add this service for AppInsight
            services.AddApplicationInsightsTelemetry();

            services.AddMvc();

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DrinkDbContext>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient(typeof(IShoppingCartRepository), typeof(ShoppingCartRepository));

            services.AddTransient(typeof(IRepository<,>), typeof(EntityRepository<,>));

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage(); // It will show the exception during development.
            }
            else
            {
                app.UseExceptionHandler("/Error"); // Handling Exception globally.
            }

            // app.UseDefaultFiles(); //  You can set or use default page which is in wwwroot.

            app.UseStaticFiles(); // for CSS and Images.

            app.UseStatusCodePages(); // It will let u know the status of the HTTP request.

            app.UseSession();  // IMPORTANT: This session call MUST go before UseMvc()

            // app.UseMvcWithDefaultRoute();

            // This is to enable ASP.Net Identity, should be before UseMvc()
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "categoryFilter", template: "{controller=Drink}/{action=Index}/{category?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("This is Production");
            });
        }
    }
}
