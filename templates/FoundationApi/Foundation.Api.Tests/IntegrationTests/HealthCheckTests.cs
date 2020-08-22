namespace Foundation.Api.Tests.IntegrationTests
{
    using FluentAssertions;
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
    public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory>
    {
        public HealthCheckTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory _factory;
        [Fact]
        public async Task HealthCheckReturn200Code()
        {
            var appFactory = _factory;
            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/health")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}
