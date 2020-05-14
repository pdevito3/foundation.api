namespace Foundation.Api.Tests
{
    using AutoMapper;
    using FluentAssertions;
    using Foundation.Api.Controllers;
    using Foundation.Api.Data;
    using Foundation.Api.Mediator.Commands;
    using Foundation.Api.Mediator.Queries;
    using Foundation.Api.Models;
    using Foundation.Api.Tests.Fakes;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Xsl;
    using Xunit;

    public class ValueToReplaceCommandTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public ValueToReplaceCommandTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly CustomWebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task CreateValueToReplace_NewValueToReplaceAddedAndReturned()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();
            var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
            context.Database.EnsureCreated();
            context.ValueToReplaces.RemoveRange(context.ValueToReplaces); // change this to use respawn?

            var valueToReplaceForCreationDto = mapper.Map<ValueToReplaceForCreationDto>(fakeValueToReplaceOne);
            var query = new CreateValueToReplaceCommand(valueToReplaceForCreationDto);
            var result = await mediator.Send(query);
            fakeValueToReplaceOne.ValueToReplaceId = result.ValueToReplaceId;

            var valueToReplaceDtoFromRepo = context.ValueToReplaces.ToList();

            valueToReplaceDtoFromRepo.Should().HaveCount(1);
            valueToReplaceDtoFromRepo.Should().ContainEquivalentOf(result);
            fakeValueToReplaceOne.Should().BeEquivalentTo(result);
        }
    }
}
