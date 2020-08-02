using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Persistence;
using Infrastructure.Shared;
using Infrastructure.Persistence.Seeders;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Seeder;
using Infrastructure.Identity;
using WebApi.Extensions;
using Autofac;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config);
            services.AddPersistenceInfrastructure(_config);
            services.AddSharedInfrastructure(_config);
            //services.AddSwaggerExtension();
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
        }

        // https://autofaccn.readthedocs.io/en/latest/integration/aspnetcore.html
        /*public void ConfigureContainer(ContainerBuilder builder)
        {
        }*/

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var context = app.ApplicationServices.GetService<ValueToReplaceDbContext>())
                {
                    context.Database.EnsureCreated();

                    ValueToReplaceSeeder.SeedSampleValueToReplaceData(app.ApplicationServices.GetService<ValueToReplaceDbContext>());
                }

                var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
                var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();
                RoleSeeder.SeedDemoRolesAsync(roleManager);
                SuperAdminSeeder.SeedDemoSuperAdminsAsync(userManager);
                BasicUserSeeder.SeedDemoBasicUser(userManager);
            }

            app.UseCors("MyCorsPolicy");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseErrorHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
                endpoints.MapControllers();
            });
        }
    }
}
