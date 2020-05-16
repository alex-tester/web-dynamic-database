using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutomationStandards.Models.DatabaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AutomationStandards
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
            services.AddControllersWithViews();
            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
            }).AddRazorRuntimeCompilation();

           // services.AddRazorRuntimeCompilation();
            services.AddMvc();
            //services.AddMvc(options => options.EnableEndpointRouting = false); //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddRoleManager<RoleManager<IdentityRole>>()
            //    .AddRoles<IdentityRole>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                AutomationStandardsContext.ConnectionString = Configuration.GetConnectionString("AutomationStandardsTest");
                
            }
            else
            {
                AutomationStandardsContext.ConnectionString = Configuration.GetConnectionString("AutomationStandardsProd");
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapRazorPages();
            });
        }
    }
}
