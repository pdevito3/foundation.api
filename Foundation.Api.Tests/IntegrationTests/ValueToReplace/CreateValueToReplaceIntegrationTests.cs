namespace Foundation.Api.Tests.IntegrationTests.ValueToReplace
{
    using Foundation.Api.Tests.Fakes.ValueToReplace;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using System.Net.Http;
    using Application.Dtos.ValueToReplace;
    using FluentAssertions;
    using WebApi;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

    [Collection("Sequential")]
    public class CreateValueToReplaceIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateValueToReplaceIntegrationTests(CustomWebApplicationFactory factory)
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
            var httpResponse = await client.PostAsJsonAsync("api/ValueToReplaceLowers", fakeValueToReplace)
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
            var httpResponse = await client.PostAsJsonAsync("api/ValueToReplaceLowers", invalidValueToReplace)
                .ConfigureAwait(false);

            // add something like this to read the errors in the body?
            //var body = JsonConvert.DeserializeObject<ValidationResult>(await httpResponse.Content.ReadAsStringAsync()
            //    .ConfigureAwait(false));

            // Assert
            httpResponse.StatusCode.Should().Be(400);
        }

        [Theory]
        [MemberData(nameof(AnonymousTypeObjects))]
        public async Task PostInvalidValueToReplaceDateField1ReturnsBadRequestCode(object invalidValueToReplace)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // intentionally bad date field, can't use normal object because c# will yell about the date value not being valid
/*            var invalidValueToReplace = new
            {
                
            };*/

            // Act
            var httpResponse = await client.PostAsJsonAsync("api/ValueToReplaceLowers", invalidValueToReplace)
                .ConfigureAwait(false);

            // Assert
            httpResponse.StatusCode.Should().Be(400);
        }

        public static IEnumerable<object[]> AnonymousTypeObjects()
        {
            return new List<object[]>
            {
                new object[] { new { } },
                new object[] { new { RandomFieldThatDoesntExist = "" } },
                new object[] { new { ValuetoReplaceDateField1 = "" } },
            };
        }
    }
}
