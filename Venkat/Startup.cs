using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Venkat.Models;
using Venkat.Security;

namespace Venkat
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "207416778611-e4pkiov82qjt9lhj6g7j0ojktmj3ln5d.apps.googleusercontent.com";
                options.ClientSecret = "EUJjA-5_TAAdtLYUhwygisAj";
            });
            services.AddAuthorization(options => {
                options.AddPolicy("DeleteRolePolicy", 
                    policy => policy.RequireClaim("Delete Role"));

                options.AddPolicy("EditRolesPolicy",
                    policy => policy.RequireClaim("Edit Role"));

                //options.AddPolicy("EditRolePolicy", 
                //    policy => policy.RequireAssertion(context => 
                //    context.User.IsInRole("Admin") && 
                //    context.User.HasClaim(claim => claim.Type == "Edit Role") || 
                //    context.User.IsInRole("Super Admin")
                //));

                options.AddPolicy("EditRolePolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesClaimsRequirement())
                );

                //options.AddPolicy("AdminRolePolicy", 
                //    policy => policy.RequireRole("Admin"));

                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Admin") ||
                    context.User.IsInRole("Super Admin")
                ));
            });            
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseStaticFiles();
            app.UseAuthentication();            
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            //Used for attribute routing
            //app.UseMvc();
        }
    }
}
