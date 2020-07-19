using Foundation.Api.Data;
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

namespace Foundation.Api.Tests
{

    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        // checkpoint for respawn to clear the database when spenning up each time
        private static Checkpoint checkpoint = new Checkpoint
        {
            
        };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
            .ConfigureServices(async services =>
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
                // context (ValueToReplaceDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ValueToReplaceDbContext>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        await checkpoint.Reset(db.Database.GetDbConnection());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }

        //public async Task<HttpClient> GetAuthenticatedClientAsync()
        //{
        //    return await GetAuthenticatedClientAsync("jason@northwind", "Northwind1!");
        //}

        //public async Task<HttpClient> GetAuthenticatedClientAsync(string userName, string password)
        //{
        //    var client = CreateClient();

        //    var token = await GetAccessTokenAsync(client, userName, password);

        //    client.SetBearerToken(token);

        //    return client;
        //}

        //    private async Task<string> GetAccessTokenAsync(HttpClient client, string userName, string password)
        //    {
        //        var disco = await client.GetDiscoveryDocumentAsync();

        //        if (disco.IsError)
        //        {
        //            throw new Exception(disco.Error);
        //        }

        //        var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        //        {
        //            Address = disco.TokenEndpoint,
        //            ClientId = "Northwind.IntegrationTests",
        //            ClientSecret = "secret",

        //            Scope = "Northwind.WebUIAPI openid profile",
        //            UserName = userName,
        //            Password = password
        //        });

        //        if (response.IsError)
        //        {
        //            throw new Exception(response.Error);
        //        }

        //        return response.AccessToken;
        //    }
        //}
    }
}
