namespace Infrastructure.Persistence
{
    using Application.Interfaces.ValueToReplace;
    using Infrastructure.Persistence.Contexts;
    using Infrastructure.Persistence.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sieve.Services;
    using System;

    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext -- Do Not Delete
            services.AddDbContext<ValueToReplaceDbContext>(opt =>
                opt.UseInMemoryDatabase("ValueToReplaceDb"));
            /*if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ValueToReplaceDbContext>(options =>
                    options.UseInMemoryDatabase($"Database{Guid.NewGuid()}"));
            }
            else
            {
                services.AddDbContext<ValueToReplaceDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        builder => builder.MigrationsAssembly(typeof(ValueToReplaceDbContext).Assembly.FullName)));
            }*/
            #endregion

            services.AddScoped<SieveProcessor>();

            #region Repositories -- Do Not Delete
            services.AddScoped<IValueToReplaceRepository, ValueToReplaceRepository>();
            //services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            //services.AddTransient<IValueToReplaceRepositoryAsync, ValueToReplaceRepositoryAsync>();
            #endregion
        }
    }
}
