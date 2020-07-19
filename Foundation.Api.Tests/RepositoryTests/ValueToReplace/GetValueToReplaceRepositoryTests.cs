namespace Foundation.Api.Tests.RepositoryTests
{
    using FluentAssertions;
    using Foundation.Api.Data;
    using Foundation.Api.Models.ValueToReplace;
    using Foundation.Api.Services;
    using Foundation.Api.Services.ValueToReplace;
    using Foundation.Api.Tests.Fakes.ValueToReplace;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Linq;
    using Xunit;

    [Collection("Sequential")]
    public class GetValueToReplaceRepositoryTests
    {
        [Fact]
        public void GetValueToReplace_ParametersMatchExpectedValues()
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
                context.ValueToReplaces.AddRange(fakeValueToReplace);
                context.SaveChanges();

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                //Assert
                var valueToReplaceById = context.ValueToReplaces.FirstOrDefault(lambdaInitialsToReplace => lambdaInitialsToReplace.ValueToReplaceId == fakeValueToReplace.ValueToReplaceId);

                valueToReplaceById.Should().BeEquivalentTo(fakeValueToReplace);
                valueToReplaceById.ValueToReplaceId.Should().Be(fakeValueToReplace.ValueToReplaceId);
                valueToReplaceById.ValueToReplaceTextField1.Should().Be(fakeValueToReplace.ValueToReplaceTextField1);
                valueToReplaceById.ValueToReplaceTextField2.Should().Be(fakeValueToReplace.ValueToReplaceTextField2);
                valueToReplaceById.ValueToReplaceDateField1.Should().Be(fakeValueToReplace.ValueToReplaceDateField1);
            }
        }

        [Fact]
        public void GetValueToReplaces_CountMatchesAndContainsEvuivalentObjects()
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
                context.SaveChanges();

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                var valueToReplaceRepo = service.GetValueToReplaces(new ValueToReplaceParametersDto());

                //Assert
                valueToReplaceRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                valueToReplaceRepo.Should().ContainEquivalentOf(fakeValueToReplaceOne);
                valueToReplaceRepo.Should().ContainEquivalentOf(fakeValueToReplaceTwo);
                valueToReplaceRepo.Should().ContainEquivalentOf(fakeValueToReplaceThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetValueToReplaces_ReturnExpectedPageSize()
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
                context.SaveChanges();

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                var valueToReplaceRepo = service.GetValueToReplaces(new ValueToReplaceParametersDto { PageSize = 2 });

                //Assert
                valueToReplaceRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                valueToReplaceRepo.Should().ContainEquivalentOf(fakeValueToReplaceOne);
                valueToReplaceRepo.Should().ContainEquivalentOf(fakeValueToReplaceTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetValueToReplaces_ReturnExpectedPageNumberAndSize()
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
                context.SaveChanges();

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                var valueToReplaceRepo = service.GetValueToReplaces(new ValueToReplaceParametersDto { PageSize = 1, PageNumber = 2 });

                //Assert
                valueToReplaceRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                valueToReplaceRepo.Should().ContainEquivalentOf(fakeValueToReplaceTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetValueToReplaces_ListSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ValueToReplaceDbContext>()
                .UseInMemoryDatabase(databaseName: $"ValueToReplaceDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceOne.ValueToReplaceTextField1 = "Bravo";

            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceTwo.ValueToReplaceTextField1 = "Alpha";

            var fakeValueToReplaceThree = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceThree.ValueToReplaceTextField1 = "Charlie";

            //Act
            using (var context = new ValueToReplaceDbContext(dbOptions))
            {
                context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo, fakeValueToReplaceThree);
                context.SaveChanges();

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                var valueToReplaceRepo = service.GetValueToReplaces(new ValueToReplaceParametersDto { SortOrder = "ValueToReplaceTextField1" });

                //Assert
                valueToReplaceRepo.Should()
                    .ContainInOrder(fakeValueToReplaceTwo, fakeValueToReplaceOne, fakeValueToReplaceThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void GetValueToReplaces_ListSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ValueToReplaceDbContext>()
                .UseInMemoryDatabase(databaseName: $"ValueToReplaceDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceOne.ValueToReplaceTextField1 = "Bravo";

            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceTwo.ValueToReplaceTextField1 = "Alpha";

            var fakeValueToReplaceThree = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceThree.ValueToReplaceTextField1 = "Charlie";

            //Act
            using (var context = new ValueToReplaceDbContext(dbOptions))
            {
                context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo, fakeValueToReplaceThree);
                context.SaveChanges();

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                var valueToReplaceRepo = service.GetValueToReplaces(new ValueToReplaceParametersDto { SortOrder = "-ValueToReplaceTextField1" });

                //Assert
                valueToReplaceRepo.Should()
                    .ContainInOrder(fakeValueToReplaceThree, fakeValueToReplaceOne, fakeValueToReplaceTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("ValueToReplaceTextField1 == Alpha")]
        [InlineData("ValueToReplaceTextField2 == Bravo")]
        [InlineData("ValueToReplaceIntField1 == 5")]
        [InlineData("ValueToReplaceTextField1 == Charlie")]
        [InlineData("ValueToReplaceTextField2 == Delta")]
        [InlineData("ValueToReplaceIntField1 == 6")]
        [InlineData("ValueToReplaceTextField1 == Echo")]
        [InlineData("ValueToReplaceTextField2 == Foxtrot")]
        [InlineData("ValueToReplaceIntField1 == 7")]
        public void GetValueToReplaces_FilterListWithExact(string filters)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ValueToReplaceDbContext>()
                .UseInMemoryDatabase(databaseName: $"ValueToReplaceDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceOne.ValueToReplaceTextField1 = "Alpha";
            fakeValueToReplaceOne.ValueToReplaceTextField2 = "Bravo";
            fakeValueToReplaceOne.ValueToReplaceIntField1 = 5;

            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceTwo.ValueToReplaceTextField1 = "Charlie";
            fakeValueToReplaceTwo.ValueToReplaceTextField2 = "Delta";
            fakeValueToReplaceTwo.ValueToReplaceIntField1 = 6;

            var fakeValueToReplaceThree = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceThree.ValueToReplaceTextField1 = "Echo";
            fakeValueToReplaceThree.ValueToReplaceTextField2 = "Foxtrot";
            fakeValueToReplaceThree.ValueToReplaceIntField1 = 7;

            //Act
            using (var context = new ValueToReplaceDbContext(dbOptions))
            {
                context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo, fakeValueToReplaceThree);
                context.SaveChanges();

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                var valueToReplaceRepo = service.GetValueToReplaces(new ValueToReplaceParametersDto { Filters = filters });

                //Assert
                valueToReplaceRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("ValueToReplaceTextField1@=Hart", 1)]
        [InlineData("ValueToReplaceTextField2@=Fav", 1)]
        [InlineData("ValueToReplaceTextField1@=*hart", 2)]
        [InlineData("ValueToReplaceTextField2@=*fav", 2)]
        public void GetValueToReplaces_FilterListWithContains(string filters, int expectedCount)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ValueToReplaceDbContext>()
                .UseInMemoryDatabase(databaseName: $"ValueToReplaceDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceOne.ValueToReplaceTextField1 = "Alpha";
            fakeValueToReplaceOne.ValueToReplaceTextField2 = "Bravo";

            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceTwo.ValueToReplaceTextField1 = "Hartsfield";
            fakeValueToReplaceTwo.ValueToReplaceTextField2 = "Favaro";

            var fakeValueToReplaceThree = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceThree.ValueToReplaceTextField1 = "Bravehart";
            fakeValueToReplaceThree.ValueToReplaceTextField2 = "Jonfav";

            //Act
            using (var context = new ValueToReplaceDbContext(dbOptions))
            {
                context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo, fakeValueToReplaceThree);
                context.SaveChanges();

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                var valueToReplaceRepo = service.GetValueToReplaces(new ValueToReplaceParametersDto { Filters = filters });

                //Assert
                valueToReplaceRepo.Should()
                    .HaveCount(expectedCount);

                context.Database.EnsureDeleted();
            }
        }

        [Theory]
        [InlineData("hart", 1)]
        [InlineData("fav", 1)]
        [InlineData("Fav", 0)]
        public void GetValueToReplaces_SearchQueryReturnsExpectedRecordCount(string queryString, int expectedCount)
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<ValueToReplaceDbContext>()
                .UseInMemoryDatabase(databaseName: $"ValueToReplaceDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeValueToReplaceOne = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceOne.ValueToReplaceTextField1 = "Alpha";
            fakeValueToReplaceOne.ValueToReplaceTextField2 = "Bravo";

            var fakeValueToReplaceTwo = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceTwo.ValueToReplaceTextField1 = "Hartsfield";
            fakeValueToReplaceTwo.ValueToReplaceTextField2 = "White";

            var fakeValueToReplaceThree = new FakeValueToReplace { }.Generate();
            fakeValueToReplaceThree.ValueToReplaceTextField1 = "Bravehart";
            fakeValueToReplaceThree.ValueToReplaceTextField2 = "Jonfav";

            //Act
            using (var context = new ValueToReplaceDbContext(dbOptions))
            {
                context.ValueToReplaces.AddRange(fakeValueToReplaceOne, fakeValueToReplaceTwo, fakeValueToReplaceThree);
                context.SaveChanges();

                var service = new ValueToReplaceRepository(context, new SieveProcessor(sieveOptions));

                var valueToReplaceRepo = service.GetValueToReplaces(new ValueToReplaceParametersDto { QueryString = queryString });

                //Assert
                valueToReplaceRepo.Should()
                    .HaveCount(expectedCount);

                context.Database.EnsureDeleted();
            }
        }
    }
}
