namespace Foundation.Api.Tests.IntegrationTests
{
    using FluentAssertions;
    using Foundation.Api.Data;
    using Foundation.Api.Models;
    using Foundation.Api.Tests.Fakes;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class GetValueToReplaceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public GetValueToReplaceIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task GetValueToReplaces_ReturnsSuccessCodeProperCountAndResourcesWithAccurateFields()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();

            var appFactory = _factory;
            var scope = appFactory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
            context.Database.EnsureCreated();

            context.ValueToReplaces.RemoveRange(context.ValueToReplaces);
            context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo);
            context.SaveChanges();

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
            response.Should().HaveCount(2);
            response.Should().ContainEquivalentOf(fakeValueToReplaceOne);
            response.Should().ContainEquivalentOf(fakeValueToReplaceTwo);
        }
    }
}
