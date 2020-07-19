namespace Foundation.Api.Tests.IntegrationTests.ValueToReplace
{
    using Foundation.Api.Tests.Fakes.ValueToReplace;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using System.Net.Http;
    using Foundation.Api.Models.ValueToReplace;
    using FluentAssertions;
    using System.Dynamic;
    using FluentValidation.Results;

    [Collection("Sequential")]
    public class CreateValueToReplaceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CreateValueToReplaceIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostValueToReplaceReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeValueToReplace = new FakeValueToReplaceDto().Generate();

            // Act
            var httpResponse = await client.PostAsJsonAsync("api/v1/ValueToReplaceLowers", fakeValueToReplace)
                .ConfigureAwait(false);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<ValueToReplaceDto>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);
            resultDto.ValueToReplaceTextField1.Should().Be(fakeValueToReplace.ValueToReplaceTextField1);
            resultDto.ValueToReplaceTextField2.Should().Be(fakeValueToReplace.ValueToReplaceTextField2);
            resultDto.ValueToReplaceDateField1.Should().Be(fakeValueToReplace.ValueToReplaceDateField1);
        }

        [Fact]
        public async Task PostInvalidValueToReplaceTextField1ReturnsBadRequestCode()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            
            var invalidValueToReplace = new FakeValueToReplaceDto().Generate();
            invalidValueToReplace.ValueToReplaceTextField1 = null;

            // Act
            var httpResponse = await client.PostAsJsonAsync("api/v1/ValueToReplaceLowers", invalidValueToReplace)
                .ConfigureAwait(false);

            // add something like this to read the errors in the body?
            //var body = JsonConvert.DeserializeObject<ValidationResult>(await httpResponse.Content.ReadAsStringAsync()
            //    .ConfigureAwait(false));

            // Assert
            httpResponse.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PostInvalidValueToReplaceDateField1ReturnsBadRequestCode()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // intentionally bad date field, can't use normal object because c# will yell about the date value not being valid
            var invalidValueToReplace = new
            {
                ValueToReplaceId = 1783336605,
                ValueToReplaceIntField1 = 0,
                ValueToReplaceTextField1 = "Investor",
                ValueToReplaceTextField2 = "focus group",
                ValueToReplaceDateField1 = "InvalidDateValue"
            };

            // Act
            var httpResponse = await client.PostAsJsonAsync("api/v1/ValueToReplaceLowers", invalidValueToReplace)
                .ConfigureAwait(false);

            // Assert
            httpResponse.StatusCode.Should().Be(400);
        }
    }
}
