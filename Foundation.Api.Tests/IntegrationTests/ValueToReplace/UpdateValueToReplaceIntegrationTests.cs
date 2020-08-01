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
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;    
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Linq;
    using AutoMapper;
    using Bogus;
    using WebApi;
    using Application.Mappings;
    using Application.Dtos.ValueToReplace;
    using Infrastructure.Persistence.Contexts;

    [Collection("Sequential")]
    public class UpdateValueToReplaceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public UpdateValueToReplaceIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PatchValueToReplaceReturns204AndFieldsWereSuccessfullyUpdated()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ValueToReplaceProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
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
            var getResult = await client.GetAsync($"api/ValueToReplaceLowers/?filters=ValueToReplaceTextField1=={fakeValueToReplaceOne.ValueToReplaceTextField1}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<IEnumerable<ValueToReplaceDto>>(getResponseContent);
            var id = getResponse.FirstOrDefault().ValueToReplaceId;

            // patch it
            var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/ValueToReplaceLowers/{id}")
            {
                Content = new StringContent(serializedValueToReplaceToUpdate,
                Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/ValueToReplaceLowers/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<ValueToReplaceDto>(checkResponseContent);

            // Assert
            patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task BadPatchValueToReplaceReturns400BadRequest()
        {
            //Arrange
            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();

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

            var manuallyCreatedInvalidPatchDoc = "[{\"value\":\"" + lookupVal + "\",\"path\":\"/ValueToReplaceIntField1\",\"op\":\"replace\"}]";

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/ValueToReplaceLowers/?filters=ValueToReplaceTextField1=={fakeValueToReplaceOne.ValueToReplaceTextField1}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<IEnumerable<ValueToReplaceDto>>(getResponseContent);
            var id = getResponse.FirstOrDefault().ValueToReplaceId;

            // patch it
            var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/ValueToReplaceLowers/{id}")
            {
                Content = new StringContent(manuallyCreatedInvalidPatchDoc,
                Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

            // Assert
            patchResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PutValueToReplaceReturnsBodyAndFieldsWereSuccessfullyUpdated()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ValueToReplaceProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var newString = "New Val";
            var newInt = 12;
            var newDate = new Faker("en").Date.Past();
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            var expectedFinalObject = mapper.Map<ValueToReplaceDto>(fakeValueToReplaceOne);
            expectedFinalObject.ValueToReplaceTextField1 = lookupVal;
            expectedFinalObject.ValueToReplaceTextField2 = newString;
            expectedFinalObject.ValueToReplaceIntField1 = newInt;
            expectedFinalObject.ValueToReplaceDateField1 = newDate;

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

            var serializedValueToReplaceToUpdate = JsonConvert.SerializeObject(expectedFinalObject);

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/ValueToReplaceLowers/?filters=ValueToReplaceTextField1=={fakeValueToReplaceOne.ValueToReplaceTextField1}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<IEnumerable<ValueToReplaceDto>>(getResponseContent);
            var id = getResponse.FirstOrDefault().ValueToReplaceId;

            // put it
            var patchResult = await client.PutAsJsonAsync($"api/ValueToReplaceLowers/{id}", expectedFinalObject)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/ValueToReplaceLowers/{id}")
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
