namespace Foundation.Api.Tests.RepositoryTests
{
    using FluentAssertions;
    using Foundation.Api.Tests.Fakes.ValueToReplace;
    using Infrastructure.Persistence.Contexts;
    using Infrastructure.Persistence.Repositories;
    using Infrastructure.Shared.Shared;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    [Collection("Sequential")]
    public class DeleteValueToReplaceRepositoryTests
    {
        [Fact]
        public void DeleteValueToReplace_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ValueToReplaceDbContext>()
                .UseInMemoryDatabase(databaseName: $"ValueToReplaceDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();
            var fakeValueToReplaceThree = new FakeValueToReplace { }.Generate();

            //Act
            using (var context = new ValueToReplaceDbContext(dbOptions))
            {
                context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo, fakeValueToReplaceThree);

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteValueToReplace(fakeValueToReplaceTwo);

                context.SaveChanges();

                //Assert
                var valueToReplaceList = context.ValueToReplaces.ToList();

                valueToReplaceList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                valueToReplaceList.Should().ContainEquivalentOf(fakeValueToReplaceOne);
                valueToReplaceList.Should().ContainEquivalentOf(fakeValueToReplaceThree);
                Assert.DoesNotContain(valueToReplaceList, lambdaInitialsToReplace => lambdaInitialsToReplace == fakeValueToReplaceTwo);

                context.Database.EnsureDeleted();
            }
        }
    }
}
