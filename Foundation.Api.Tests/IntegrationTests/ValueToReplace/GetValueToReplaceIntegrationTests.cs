namespace Foundation.Api.Tests.IntegrationTests.ValueToReplace
{
    using Application.Dtos.ValueToReplace;
    using FluentAssertions;
    using Foundation.Api.Tests.Fakes.ValueToReplace;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WebApi;
    using Xunit;

    [Collection("Sequential")]
    public class GetValueToReplaceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetValueToReplaceIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory<Startup> _factory;
        [Fact]
        public async Task GetValueToReplaces_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
                context.Database.EnsureCreated();

                //context.ValueToReplaces.RemoveRange(context.ValueToReplaces);
                context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/v1/ValueToReplaceLowers")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<IEnumerable<ValueToReplaceDto>>(responseContent);

            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeValueToReplaceOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeValueToReplaceTwo, options =>
                options.ExcludingMissingMembers());
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
