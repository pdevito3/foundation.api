namespace Foundation.Api
{
    using AutoBogus;
    using Autofac;
    using AutoMapper;
    using Foundation.Api.Data;
    using Foundation.Api.Data.Entities;
    using Foundation.Api.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Sieve.Services;
    using System;

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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<SieveProcessor>();
            
            services.AddScoped<IValueToReplaceRepository, ValueToReplaceRepository>();

            services.AddDbContext<ValueToReplaceDbContext>(opt => 
                opt.UseInMemoryDatabase("ValueToReplaceDb"));

            services.AddControllers()
                .AddNewtonsoftJson();
        }

        // https://autofaccn.readthedocs.io/en/latest/integration/aspnetcore.html
        public void ConfigureContainer(ContainerBuilder builder)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var context = app.ApplicationServices.GetService<ValueToReplaceDbContext>())
            {
                context.Database.EnsureCreated();

                context.ValueToReplaces.Add(new AutoFaker<ValueToReplace>().RuleFor(fake => fake.ValueToReplaceId, 1));
                context.ValueToReplaces.Add(new AutoFaker<ValueToReplace>());
                context.ValueToReplaces.Add(new AutoFaker<ValueToReplace>());

                context.SaveChanges();
            }
        }
    }
}
