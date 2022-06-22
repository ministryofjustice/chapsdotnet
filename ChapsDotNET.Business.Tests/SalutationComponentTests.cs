using System.Linq;
using System.Threading.Tasks;
using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Business.Tests.Common;
using ChapsDotNET.Data.Entities;
using FluentAssertions;
using Xunit;

namespace ChapsDotNET.Business.Tests
{
    public class SalutationComponentTests
    {
        [Fact(DisplayName = "Get a list of salutations when GetSalutationsAsync is called")]
        public async Task GetAListOfSalutationsWhenGetSalutationsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 1,
                Detail = "Mr",
                active = true
            });
            await context.SaveChangesAsync();

            var salutationComponent = new SalutationComponent(context);

            // Act
            var result = await salutationComponent.GetSalutationsAsync(new SalutationRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            result.Results?.First().Detail.Should().Be("Mr");
            result.Results?.First().SalutationId.Should().Be(1);
            result.Results?.First().Active.Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of only active salutations when GetSalutationsAsync is called without true in the parameter")]
        public async Task GetAListOfOnlyActiveSalutationsWhenGetSalutationsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 1,
                Detail = "Mr",
                active = true
            });
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 2,
                Detail = "Mrs",
                active = true
            });
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 3,
                Detail = "Dr",
                active = false
            });
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 4,
                Detail = "Miss",
                active = true
            });

            await context.SaveChangesAsync();

            var salutationComponent = new SalutationComponent(context);

            // Act
            var result = await salutationComponent.GetSalutationsAsync(new SalutationRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(3);
            result.Results?.All(x => x.Active).Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of only all salutations when GetSalutationsAsync is called with true ShowActiveInactive in the parameter")]
        public async Task GetAListOfActiveAndInactiveSalutationsWhenGetSalutationsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 1,
                Detail = "Mr",
                active = true
            });
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 2,
                Detail = "Mrs",
                active = true
            });
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 3,
                Detail = "Dr",
                active = false
            });
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 4,
                Detail = "Miss",
                active = true
            });

            await context.SaveChangesAsync();

            var salutationComponent = new SalutationComponent(context);

            // Act
            var result = await salutationComponent.GetSalutationsAsync(new SalutationRequestModel
            {
                ShowActiveAndInactive = true
            });

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(4);
        }

        [Fact(DisplayName = "When we call GetSalutationsAsync we get paged results by default")]
        public async Task GetSalutationsAsyncReturnsPagedResults()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var fakeSalutations = FakeSalutationsProvider.FakeData
                .Generate(50)
                .ToList();
            await context.Salutations.AddRangeAsync(fakeSalutations);
            await context.SaveChangesAsync();
            var salutationComponent = new SalutationComponent(context);

            // Act
            var result = await salutationComponent.GetSalutationsAsync(new SalutationRequestModel
            {
                ShowActiveAndInactive = true
            });

            // Assert
            result.PageSize.Should().Be(10);
            result.PageCount.Should().Be(5);
            result.RowCount.Should().Be(50);
            result.CurrentPage.Should().Be(1);
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(10);
        }


        [Fact(DisplayName = "Get a specific salutation")]
        public async Task GetSalutationAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 2,
                Detail = "Mrs",
                active = true
            });

            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 53,
                Detail = "Ms",
                active = true
            });

            await context.SaveChangesAsync();

            var salutationComponent = new SalutationComponent(context);

            // Act
            var result = await salutationComponent.GetSalutationAsync(53);

            // Assert
            result.Should().NotBeNull();
           
            result.Detail.Should().Be("Ms");
            result.SalutationId.Should().Be(53);
            result.Active.Should().BeTrue();


        }

        [Fact(DisplayName = "What happens for the wrong salutation id?")]
        public async Task GetWrongSalutationAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 2,
                Detail = "Mrs",
                active = true
            });

            await context.Salutations.AddAsync(new Salutation
            {
                salutationID = 54,
                Detail = "Ms",
                active = true
            });

            await context.SaveChangesAsync();

            var salutationComponent = new SalutationComponent(context);

            // Act
            var result = await salutationComponent.GetSalutationAsync(53);

            // Assert
            result.Should().NotBeNull();

            result.Detail.Should().Be("Ms");
            result.SalutationId.Should().Be(53);
            result.Active.Should().BeTrue();


        }

    }
}
