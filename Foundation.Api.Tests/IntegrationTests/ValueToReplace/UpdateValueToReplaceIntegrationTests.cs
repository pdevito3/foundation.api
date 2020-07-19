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
    using Microsoft.Extensions.DependencyInjection;
    using Foundation.Api.Data;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Linq;
    using AutoMapper;
    using Foundation.Api.Configuration;

    public class UpdateValueToReplaceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UpdateValueToReplaceIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostValueToReplaceReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ValueToReplaceProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceOne.ValueToReplaceTextField1 = lookupVal;
            var expectedFinalObject = mapper.Map<ValueToReplaceDto>(fakeValueToReplaceOne);
            expectedFinalObject.ValueToReplaceTextField1 = lookupVal;

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
                context.Database.EnsureCreated();

                context.ValueToReplaces.RemoveRange(context.ValueToReplaces);
                context.ValueToReplaces.AddRange(fakeValueToReplaceOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var patchDoc = new JsonPatchDocument<ValueToReplaceForUpdateDto>();
            patchDoc.Replace(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceTextField1, lookupVal);
            var serializedValueToReplaceToUpdate = JsonConvert.SerializeObject(patchDoc);

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/v1/ValueToReplaceLowers/?filters=valueToReplaceTextField1=={lookupVal}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<IEnumerable<ValueToReplaceDto>>(getResponseContent);
            var id = getResponse.FirstOrDefault().ValueToReplaceId;

            // patch it
            var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/v1/ValueToReplaceLowers/{id}")
            {
                Content = new StringContent(serializedValueToReplaceToUpdate,
                System.Text.Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/v1/ValueToReplaceLowers/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<ValueToReplaceDto>(checkResponseContent);

            // Assert
            patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
    }
}
