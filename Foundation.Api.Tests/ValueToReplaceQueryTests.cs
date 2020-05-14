namespace Foundation.Api.Tests
{
    using FluentAssertions;
    using Foundation.Api.Controllers;
    using Foundation.Api.Data;
    using Foundation.Api.Mediator.Queries;
    using Foundation.Api.Models;
    using Foundation.Api.Tests.Fakes;
    using MediatR;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ValueToReplaceQueryTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public ValueToReplaceQueryTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task GetValueToReplaceQuery_ReturnsResourceWithProperFields()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
            context.Database.EnsureCreated();
            context.ValueToReplaces.RemoveRange(context.ValueToReplaces); // change this to use respawn?

            context.ValueToReplaces.AddRange(fakeValueToReplaceOne);
            context.SaveChanges();

            var query = new GetValueToReplaceQuery(fakeValueToReplaceOne.ValueToReplaceId);
            var result = await mediator.Send(query);

            result.Should().BeEquivalentTo(fakeValueToReplaceOne);
        }

        [Fact]
        public async Task GetAllValueToReplacesQuery_ReturnsResourcesWithProperFields()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
            context.Database.EnsureCreated();
            context.ValueToReplaces.RemoveRange(context.ValueToReplaces); // change this to use respawn?

            context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo);
            context.SaveChanges();

            var valueToReplaceParametersDto = new ValueToReplaceParametersDto { };
            var query = new GetAllValueToReplacesQuery(valueToReplaceParametersDto, new ValueToReplacesController(mediator));
            var result = await mediator.Send(query);

            result.ValueToReplaceDtoList.Should().HaveCount(2);
            result.ValueToReplaceDtoList.Should().ContainEquivalentOf(fakeValueToReplaceOne);
            result.ValueToReplaceDtoList.Should().ContainEquivalentOf(fakeValueToReplaceTwo);
        }

        [Fact]
        public async Task GetAllValueToReplacesQuery_ReturnsHeaderWithNextPageAndPreviousPageBoolWhenExpectedWithPageInfo()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
            context.Database.EnsureCreated();
            context.ValueToReplaces.RemoveRange(context.ValueToReplaces); // change this to use respawn?

            context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo);
            context.SaveChanges();

            var valueToReplaceParametersDto = new ValueToReplaceParametersDto { PageSize = 1 };
            var controller = new ValueToReplacesController(mediator);
            var mockUrl = new Mock<IUrlHelper>();
            controller.Url = mockUrl.Object;

            var query = new GetAllValueToReplacesQuery(valueToReplaceParametersDto, controller);
            var result = await mediator.Send(query);

            result.PaginationMetadata.HasNext.Should().Be(true);
            result.PaginationMetadata.HasPrevious.Should().Be(false);
        }

        //TODO: Add tests for PaginationMetadata.NextPageLink, PaginationMetadata.PreviousLink
        /* possible mocks for controllers
         
            //var mockUrl = new Mock<IUrlHelper>(MockBehavior.Strict);
            //Expression<Func<IUrlHelper, string>> urlSetup
            //    = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "Get"));
            //mockUrl.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();
            //mockUrl.SetupAllProperties();

         */
    }
}
