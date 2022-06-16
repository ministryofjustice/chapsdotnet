using System.Linq;
using System.Threading.Tasks;
using ChapsDotNET.Business.Components;
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
            var result = await salutationComponent.GetSalutationsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Detail.Should().Be("Mr");
            result.First().SalutationId.Should().Be(1);
            result.First().Active.Should().BeTrue();
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
            var result = await salutationComponent.GetSalutationsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.All(x => x.Active).Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of all active & inactive salutations when GetSalutationsAsync is called wit true in the parameter")]
        public async Task GetAListOfAllSalutationsWhenGetSalutationsAsyncIsCalledWithTrue()
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
            var result = await salutationComponent.GetSalutationsAsync(true);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(4);
        }
    }
}
