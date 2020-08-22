
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi;

namespace Foundation.Api.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        // checkpoint for respawn to clear the database when spenning up each time
        private static Checkpoint checkpoint = new Checkpoint
        {

        };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddEntityFrameworkInMemoryDatabase();

                // Create a new service provider.
                var provider = services
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<ValueToReplaceDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ValueToReplaceDbContext>();
                    /*                    var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();

                                        var logger = scopedServices
                                            .GetRequiredService<ILogger<ApiTestFixture>>();*/

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        //ValueToReplaceDbContextSeed.SeedAsync(db, loggerFactory).Wait();

                        // seed sample user data
                        /*var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
                        AppIdentityDbContextSeed.SeedAsync(userManager, roleManager).Wait();*/
                    }
                    catch (Exception ex)
                    {
                        //logger.LogError(ex, $"An error occurred seeding the " +
                        //    "database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }
    }
}
