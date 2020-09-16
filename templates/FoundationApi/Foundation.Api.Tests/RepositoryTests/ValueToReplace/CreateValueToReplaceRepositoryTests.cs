namespace Foundation.Api.Tests.RepositoryTests
{
    using FluentAssertions;
    using Foundation.Api.Tests.Fakes.ValueToReplace;
    using Infrastructure.Persistence.Contexts;
    using Infrastructure.Persistence.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Linq;
    using Xunit;

    [Collection("Sequential")]
    public class CreateValueToReplaceRepositoryTests
    {
        [Fact]
        public void AddValueToReplace_NewRecordAddedWithProperValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ValueToReplaceDbContext>()
                .UseInMemoryDatabase(databaseName: $"ValueToReplaceDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeValueToReplace = new FakeValueToReplace { }.Generate();

            //Act
            using (var context = new ValueToReplaceDbContext(dbOptions))
            {
                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                service.AddValueToReplace(fakeValueToReplace);

                context.SaveChanges();
            }

            //Assert
            using (var context = new ValueToReplaceDbContext(dbOptions))
            {
                context.ValueToReplaces.Count().Should().Be(1);

                var valueToReplaceById = context.ValueToReplaces.FirstOrDefault(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceId == fakeValueToReplace.ValueToReplaceId);

                valueToReplaceById.Should().BeEquivalentTo(fakeValueToReplace);
                valueToReplaceById.ValueToReplaceId.Should().Be(fakeValueToReplace.ValueToReplaceId);
                valueToReplaceById.ValueToReplaceTextField1.Should().Be(fakeValueToReplace.ValueToReplaceTextField1);
                valueToReplaceById.ValueToReplaceTextField2.Should().Be(fakeValueToReplace.ValueToReplaceTextField2);
                valueToReplaceById.ValueToReplaceDateField1.Should().Be(fakeValueToReplace.ValueToReplaceDateField1);
            }
        }
    }
}
