using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Business.Tests.Common;
using ChapsDotNET.Data.Entities;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChapsDotNET.Business.Tests
{
    public class TeamComponentTests
    {
        [Fact(DisplayName = "Get a list of teams when GetTeamsAsync is called")]
        public async Task GetAListOfTeamsWhenGetTeamsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Teams.AddAsync(new Team
            {
                TeamID = 1,
                Name = "Team A",
                Acronym = "TeamA",
                active = true
            });
            await context.SaveChangesAsync();

            var teamComponent = new TeamComponent(context);

            // Act
            var result = await teamComponent.GetTeamsAsync(new TeamRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            result.Results?.First().Name.Should().Be("Team A");
            result.Results?.First().TeamId.Should().Be(1);
            result.Results?.First().Active.Should().BeTrue();

        }
    }
}

//        [Fact(DisplayName = "Get a list of only active salutations when GetSalutationsAsync is called without true in the parameter")]
//        public async Task GetAListOfOnlyActiveSalutationsWhenGetSalutationsAsyncIsCalled()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 1,
//                Detail = "Mr",
//                active = true
//            });
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 2,
//                Detail = "Mrs",
//                active = true
//            });
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 3,
//                Detail = "Dr",
//                active = false
//            });
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 4,
//                Detail = "Miss",
//                active = true
//            });

//            await context.SaveChangesAsync();

//            var salutationComponent = new SalutationComponent(context);

//            // Act
//            var result = await salutationComponent.GetSalutationsAsync(new SalutationRequestModel());

//            // Assert
//            result.Results.Should().NotBeNull();
//            result.Results.Should().HaveCount(3);
//            result.Results?.All(x => x.Active).Should().BeTrue();
//        }

//        [Fact(DisplayName = "Get a list of only all salutations when GetSalutationsAsync is called with true ShowActiveInactive in the parameter")]
//        public async Task GetAListOfActiveAndInactiveSalutationsWhenGetSalutationsAsyncIsCalled()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 1,
//                Detail = "Mr",
//                active = true
//            });
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 2,
//                Detail = "Mrs",
//                active = true
//            });
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 3,
//                Detail = "Dr",
//                active = false
//            });
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 4,
//                Detail = "Miss",
//                active = true
//            });

//            await context.SaveChangesAsync();

//            var salutationComponent = new SalutationComponent(context);

//            // Act
//            var result = await salutationComponent.GetSalutationsAsync(new SalutationRequestModel
//            {
//                ShowActiveAndInactive = true
//            });

//            // Assert
//            result.Results.Should().NotBeNull();
//            result.Results.Should().HaveCount(4);
//        }

//        [Fact(DisplayName = "When we call GetSalutationsAsync we get paged results by default")]
//        public async Task GetSalutationsAsyncReturnsPagedResults()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();
//            var fakeSalutations = FakeSalutationsProvider.FakeData
//                .Generate(50)
//                .ToList();
//            await context.Salutations.AddRangeAsync(fakeSalutations);
//            await context.SaveChangesAsync();
//            var salutationComponent = new SalutationComponent(context);

//            // Act
//            var result = await salutationComponent.GetSalutationsAsync(new SalutationRequestModel
//            {
//                ShowActiveAndInactive = true
//            });

//            // Assert
//            result.PageSize.Should().Be(10);
//            result.PageCount.Should().Be(5);
//            result.RowCount.Should().Be(50);
//            result.CurrentPage.Should().Be(1);
//            result.Results.Should().NotBeNull();
//            result.Results.Should().HaveCount(10);
//        }


//        [Fact(DisplayName = "Get a specific salutation")]
//        public async Task GetSalutationAsync()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 2,
//                Detail = "Mrs",
//                active = true
//            });

//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 53,
//                Detail = "Ms",
//                active = true
//            });

//            await context.SaveChangesAsync();

//            var salutationComponent = new SalutationComponent(context);

//            // Act
//            var result = await salutationComponent.GetSalutationAsync(53);

//            // Assert
//            result.Should().NotBeNull();

//            result.Detail.Should().Be("Ms");
//            result.SalutationId.Should().Be(53);
//            result.Active.Should().BeTrue();


//        }

//        [Fact(DisplayName = "What happens for the wrong salutation id?")]
//        public async Task GetWrongSalutationAsync()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();
//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 2,
//                Detail = "Mrs",
//                active = true
//            });

//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 54,
//                Detail = "Ms",
//                active = true
//            });

//            await context.SaveChangesAsync();

//            var salutationComponent = new SalutationComponent(context);

//            // Act
//            var result = await salutationComponent.GetSalutationAsync(53);

//            // Assert
//            result.Detail.Should().BeNull();

//            //result.Detail.Should().Be("Ms");
//            //result.SalutationId.Should().Be(54);
//            //result.Active.Should().BeTrue();


//        }

//        [Fact(DisplayName = "Add a Salutation to the database when calling the AddSalutation method returns Success")]
//        public async Task AddASalutationToTheDatabase()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();

//            var salutationComponent = new SalutationComponent(context);
//            var newrecord = new Models.SalutationModel
//            {
//                SalutationId = 1,
//                Detail = "AAA",
//                Active = true
//            };

//            // Act
//            var result = await salutationComponent.AddSalutationAsync(newrecord);


//            // Assert
//            result.Should().NotBe(0);
//            context.Salutations.First().Detail.Should().Be("AAA");
//            context.Salutations.First().active.Should().Be(true);

//        }

//        [Fact(DisplayName = "Adding a Salutation with empty detail should throw an ArgumentNullException")]
//        public async Task AddingAnEmptyTitleShouldThrowException()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();

//            var salutationComponent = new SalutationComponent(context);
//            var newrecord = new Models.SalutationModel
//            {
//                SalutationId = 1,
//                Detail = "",
//                Active = true
//            };

//            //Act
//            Func<Task> act = async () => { await salutationComponent.AddSalutationAsync(newrecord); };

//            //Assert
//            await act.Should().ThrowAsync<ArgumentNullException>();
//        }


//        [Fact(DisplayName = "Updating active status on a Salutation from the database when calling the UpdateSalutationActiveStatus")]
//        public async Task UpdateActiveStatusOnASalutation()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();

//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 1,
//                Detail = "AAA",
//                active = true
//            });

//            await context.SaveChangesAsync();

//            var salutationComponent = new SalutationComponent(context);

//            // Act
//            await salutationComponent.UpdateSalutationAsync(new Models.SalutationModel
//            {
//                SalutationId = 1,
//                Active = false,
//                Detail = "BBB"
//            });

//            // Assert

//            context.Salutations.First().Detail.Should().Be("BBB");
//            context.Salutations.First().active.Should().BeFalse();
//        }

//        [Fact(DisplayName = "Updating active status on a Salutation when calling the UpdateSalutationAsync with no Id")]
//        public async Task UpdateActiveStatusOnASalutationWithNoIDShouldThrowAnException()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();

//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 1,
//                Detail = "AAA",
//                active = true
//            });

//            await context.SaveChangesAsync();

//            var salutationComponent = new SalutationComponent(context);

//            // Act

//            Func<Task> act = async () => { await salutationComponent.UpdateSalutationAsync(new Models.SalutationModel()); };

//            //Assert
//            await act.Should().ThrowAsync<NotFoundException>();
//        }

//        [Fact(DisplayName = "Updating Salutation with Empty Detail should throw an ArgumentNullException")]
//        public async Task UpdateSalutationWithNoDetailShouldThrowAnException()
//        {
//            // Arrange
//            var context = DataContextFactory.Create();

//            await context.Salutations.AddAsync(new Salutation
//            {
//                salutationID = 1,
//                Detail = "AAA",
//                active = true
//            });

//            await context.SaveChangesAsync();

//            var salutationComponent = new SalutationComponent(context);

//            // Act

//            Func<Task> act = async () =>
//            {
//                await salutationComponent.UpdateSalutationAsync(new Models.SalutationModel
//                {
//                    Detail = string.Empty,
//                    SalutationId = 1,
//                    Active = true
//                });
//            };

//            //Assert
//            await act.Should().ThrowAsync<ArgumentNullException>();
//        }

//    }
//}
