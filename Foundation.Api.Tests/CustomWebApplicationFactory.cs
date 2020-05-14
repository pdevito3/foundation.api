using Foundation.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Foundation.Api.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
            .ConfigureServices(services =>
            {
                // Remove the app's ValueToReplaceDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ValueToReplaceDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ValueToReplaceDbContext using an in-memory database for testing.
                services.AddDbContext<ValueToReplaceDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestingDb");
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ValueToReplaceDbContext>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        db.RemoveRange(db.ValueToReplaces);
                        // Seed the database with test data.
                        //Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        }
    }
}
