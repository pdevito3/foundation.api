namespace Foundation.Api.Tests
{
    using AutoBogus;
    using AutoMapper;
    using FluentAssertions;
    using Foundation.Api.Controllers;
    using Foundation.Api.Data;
    using Foundation.Api.Mediator.Commands;
    using Foundation.Api.Models;
    using Foundation.Api.Tests.Fakes;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
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

        [Fact]
        public async Task DeleteValueToReplace_RemovesValueToReplace()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
            context.Database.EnsureCreated();
            context.ValueToReplaces.RemoveRange(context.ValueToReplaces); // change this to use respawn?
            context.ValueToReplaces.Add(fakeValueToReplaceOne);
            context.SaveChanges();

            var valueToReplaceDtoFromRepo = context.ValueToReplaces.FirstOrDefault();

            var query = new DeleteValueToReplaceCommand(valueToReplaceDtoFromRepo.ValueToReplaceId);
            var result = await mediator.Send(query);

            var valueToReplaceDtosFromRepo = context.ValueToReplaces.ToList();

            valueToReplaceDtosFromRepo.Should().HaveCount(0);
            valueToReplaceDtosFromRepo.Should().NotContain(valueToReplaceDtoFromRepo);
        }

        [Fact]
        public async Task DeleteValueToReplaceWithNonExistantId_DoesNotRemoveRemovesValueToReplace()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
            context.Database.EnsureCreated();
            context.ValueToReplaces.RemoveRange(context.ValueToReplaces); // change this to use respawn?
            context.ValueToReplaces.Add(fakeValueToReplaceOne);
            context.SaveChanges();

            var valueToReplaceDtoFromRepo = context.ValueToReplaces.FirstOrDefault();

            var query = new DeleteValueToReplaceCommand(0);
            _ = await mediator.Send(query);

            var valueToReplaceDtosFromRepo = context.ValueToReplaces.ToList();

            valueToReplaceDtosFromRepo.Should().HaveCount(1);
            valueToReplaceDtosFromRepo.Should().ContainEquivalentOf(valueToReplaceDtoFromRepo);
        }

        [Fact]
        public async Task UpdateEntireValueToReplace_NewValueToReplaceAddedAndReturned()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            var valueToReplaceForUpdateDto = new AutoFaker<ValueToReplaceForUpdateDto> { }.Generate();

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
            context.Database.EnsureCreated();
            context.ValueToReplaces.RemoveRange(context.ValueToReplaces); // change this to use respawn?
            context.ValueToReplaces.Add(fakeValueToReplaceOne);
            context.SaveChanges();

            var valueToReplaceDtoFromRepo = context.ValueToReplaces.FirstOrDefault();
            var query = new UpdateEntireValueToReplaceCommand(valueToReplaceDtoFromRepo.ValueToReplaceId, valueToReplaceForUpdateDto);
            _ = await mediator.Send(query);

            valueToReplaceDtoFromRepo = context.ValueToReplaces.FirstOrDefault();

            valueToReplaceDtoFromRepo.ValueToReplaceTextField1.Should().Be(valueToReplaceForUpdateDto.ValueToReplaceTextField1);
            valueToReplaceDtoFromRepo.ValueToReplaceTextField2.Should().Be(valueToReplaceForUpdateDto.ValueToReplaceTextField2);
            valueToReplaceDtoFromRepo.ValueToReplaceIntField1.Should().Be(valueToReplaceForUpdateDto.ValueToReplaceIntField1);
            valueToReplaceDtoFromRepo.ValueToReplaceDateField1.Should().Be(valueToReplaceForUpdateDto.ValueToReplaceDateField1);
        }

        [Fact]
        public async Task UpdatePartialValueToReplace_NewValueToReplaceAddedAndReturned()
        {
            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            var valueToReplacePatchDoc = new JsonPatchDocument<ValueToReplaceForUpdateDto> { };
            valueToReplacePatchDoc.Replace(p => p.ValueToReplaceTextField1, "New Val1");
            valueToReplacePatchDoc.Replace(p => p.ValueToReplaceTextField2, "New Val2");
            valueToReplacePatchDoc.Replace(p => p.ValueToReplaceIntField1, 1);

            var scope = _factory.Services.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();
            var context = scope.ServiceProvider.GetRequiredService<ValueToReplaceDbContext>();
            context.Database.EnsureCreated();
            context.ValueToReplaces.RemoveRange(context.ValueToReplaces); // change this to use respawn?
            context.ValueToReplaces.Add(fakeValueToReplaceOne);
            context.SaveChanges();

            var controller = new ValueToReplacesController(mediator);
            new Helpers.ControllerBuilder(controller).WithContext().WithRequest().WithResponse().WithResponseHeaders(new HeaderDictionary()).WithUrlHelper().Build();

            var valueToReplaceDtoFromRepo = context.ValueToReplaces.FirstOrDefault();
            var query = new UpdatePartialValueToReplaceCommand(valueToReplaceDtoFromRepo.ValueToReplaceId, valueToReplacePatchDoc, controller);
            _ = await mediator.Send(query);

            valueToReplaceDtoFromRepo = context.ValueToReplaces.FirstOrDefault();

            valueToReplaceDtoFromRepo.ValueToReplaceTextField1.Should().Be("New Val1");
            valueToReplaceDtoFromRepo.ValueToReplaceTextField2.Should().Be("New Val2");
            valueToReplaceDtoFromRepo.ValueToReplaceIntField1.Should().Be(1);
        }
    }
}
