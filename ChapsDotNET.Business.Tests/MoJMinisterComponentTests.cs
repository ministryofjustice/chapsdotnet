using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Business.Tests.Common;
using ChapsDotNET.Data.Entities;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChapsDotNET.Business.Tests
{
	public class MoJMinisterComponentTests
	{
		[Fact(DisplayName = "Get a list of MoJ Ministers via the GetMoJMinistersAsync method")]
        public async Task GetAListOfMoJMinistersWhenGetMoJMinistersAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 1,
                prefix = "Emperor",
                Name = "Londo Mollari",
                suffix = "Paso Leati",
                active = true
            });
            await context.SaveChangesAsync();

            var mojMinisterComponent = new MoJMinisterComponent(context);

            // Act
            var result = await mojMinisterComponent.GetMoJMinistersAsync(new MoJMinisterRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            result.Results?.First().MoJMinisterId.Should().Be(1);
            result.Results?.First().Prefix.Should().Be("Emperor");
            result.Results?.First().Name.Should().Be("Londo Mollari");
            result.Results?.First().Suffix.Should().Be("Paso Leati");
            result.Results?.First().Active.Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of only active MoJ Ministers when GetMoJMinistersAsync is called without true in the parameter")]
        public async Task GetAListOfOnlyActiveMoJMinistersWhenGetMoJMinistersAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 1,
                prefix = "Emperor",
                Name = "Londo Mollari",
                suffix = "Paso Leati",
                active = true
            });
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 2,
                prefix = "Ambassador",
                Name = "Delenn",
                suffix = "Grey Council",
                active = false
                            
            });
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 3,
                prefix = "Ambassador",
                Name = "G'Kar",
                suffix = "The Red Knight",
                active = true
                
            });
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 4,
                prefix = "Captain",
                Name = "John Sheridan",
                suffix = "",
                active = false
            });

            await context.SaveChangesAsync();

            var mojMinisterComponent = new MoJMinisterComponent(context);

            // Act
            var result = await mojMinisterComponent.GetMoJMinistersAsync(new MoJMinisterRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(2);
            result.Results?.All(x => x.Active).Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of all active and inactive MoJ Ministers")]
        public async Task GetAListOfActiveAndInactiveMoJMinistersWhenGetMoJMinistersAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 1,
                prefix = "Emperor",
                Name = "Londo Mollari",
                suffix = "Paso Leati",
                active = true
            });
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 2,
                prefix = "Ambassador",
                Name = "Delenn",
                suffix = "Grey Council",
                active = false
                            
            });
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 3,
                prefix = "Ambassador",
                Name = "G'Kar",
                suffix = "The Red Knight",
                active = true
                
            });
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 4,
                prefix = "Captain",
                Name = "John Sheridan",
                suffix = "",
                active = false
            });

            await context.SaveChangesAsync();

            var mojMinisterComponent = new MoJMinisterComponent(context);

            // Act
            var result = await mojMinisterComponent.GetMoJMinistersAsync(new MoJMinisterRequestModel
            {
                ShowActiveAndInactive = true
            });

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(4);
        }

        [Fact(DisplayName = "Calling the GetMoJMinistersAsync method returns paged results by default")]
        public async Task GetMoJMinistersAsyncReturnsPagedResults()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var fakeMoJMinisters = FakeMoJMinistersProvider.FakeData
                .Generate(50)
                .ToList();
            await context.MoJMinisters.AddRangeAsync(fakeMoJMinisters);
            await context.SaveChangesAsync();
            var mojMinisterComponent = new MoJMinisterComponent(context);

            // Act
            var result = await mojMinisterComponent.GetMoJMinistersAsync(new MoJMinisterRequestModel
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

        [Fact(DisplayName = "Get a specific MoJ Minister")]
        public async Task GetMoJMinisterAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 2,
                prefix = "Ambassador",
                Name = "Delenn",
                suffix = "Grey Council",
                active = false
            });

            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 53,
                prefix = "Captain",
                Name = "John Sheridan",
                suffix = "",
                active = true
            });

            await context.SaveChangesAsync();

            var mojMinisterComponent = new MoJMinisterComponent(context);

            // Act
            var result = await mojMinisterComponent.GetMoJMinisterAsync(53);

            // Assert
            result.Should().NotBeNull();
            result.MoJMinisterId.Should().Be(53);
            result.Prefix.Should().Be("Captain");
            result.Name.Should().Be("John Sheridan");
            result.Suffix.Should().Be("");
            result.Active.Should().BeTrue();
        }

        [Fact(DisplayName = "What happen when calling wrong MoJ Minister ID?")]
        public async Task GetWrongMoJMinisterAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 2,
                prefix = "Ambassador",
                Name = "Delenn",
                suffix = "Grey Council",
                active = true
            });

            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 54,
                prefix = "Captain",
                Name = "John Sheridan",
                suffix = "",
                active = true
            });

            await context.SaveChangesAsync();

            var mojMinisterComponent = new MoJMinisterComponent(context);

            // Act
            var result = await mojMinisterComponent.GetMoJMinisterAsync(12);

            // Assert
            result.Prefix.Should().BeNull();
            result.Name.Should().BeEmpty();
            result.Suffix.Should().BeNull();
        }

        [Fact(DisplayName = "Calling the AddMoJMinister method returns correct MoJMinisterID")]
        public async Task AddAMoJMinisterToTheDatabase()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var mojMinisterComponent = new MoJMinisterComponent(context);
            var newrecord = new Models.MoJMinisterModel
            {
                MoJMinisterId = 2,
                Prefix = "Ambassador",
                Name = "Delenn",
                Suffix = "Grey Council",
                Active = true
            };

            // Act
            var result = await mojMinisterComponent.AddMoJMinisterAsync(newrecord);

            // Assert
            result.Should().NotBe(0);
            context.MoJMinisters.First().prefix.Should().Be("Ambassador");
            context.MoJMinisters.First().Name.Should().Be("Delenn");
            context.MoJMinisters.First().suffix.Should().Be("Grey Council");
            context.MoJMinisters.First().active.Should().Be(true);
        }

        [Fact(DisplayName = "Adding a MoJ Minister without a name should throw an ArgumentNullException")]
        public async Task AddingAMoJMinisterWithoutANameShouldThrowException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var mojMinisterComponent = new MoJMinisterComponent(context);
            var newrecord = new Models.MoJMinisterModel
            {
                MoJMinisterId = 1,
                Prefix = "Emperor",
                Name = "",
                Suffix = "Paso Leati",
                Active = true
            };

            //Act
            Func<Task> act = async () => { await mojMinisterComponent.AddMoJMinisterAsync(newrecord); };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact(DisplayName = "Update a MoJ Ministers active status via the UpdateSalutationActiveStatus method")]
        public async Task UpdateAMoJMinistersActiveStatus()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 3,
                prefix = "Ambassador",
                Name = "G'Kar",
                suffix = "The Red Knight",
                active = true
            });

            await context.SaveChangesAsync();

            var mojMinisterComponent = new MoJMinisterComponent(context);

            // Act
            await mojMinisterComponent.UpdateMoJMinisterAsync(new Models.MoJMinisterModel
            {
                MoJMinisterId = 3,
                Prefix = "Ambassador",
                Name = "G'Kar",
                Suffix = "The Red Knight",
                Active = false
            });

            // Assert
            context.MoJMinisters.First().prefix.Should().Be("Ambassador");
            context.MoJMinisters.First().Name.Should().Be("G'Kar");
            context.MoJMinisters.First().suffix.Should().Be("The Red Knight");
            context.MoJMinisters.First().active.Should().BeFalse();
        }

        [Fact(DisplayName = "Update a MoJ Ministers active status by calling the UpdateSalutationAsync method with no ID")]
        public async Task UpdateAMoJMinistersActiveStatusWithNoIDShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 1,
                prefix = "Emperor",
                Name = "Londo Mollari",
                suffix = "Paso Leati",
                active = true
            });

            await context.SaveChangesAsync();
            var mojMinisterComponent = new MoJMinisterComponent(context);

            // Act
            Func<Task> act = async () => { await mojMinisterComponent.UpdateMoJMinisterAsync(new Models.MoJMinisterModel()); };

            //Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact(DisplayName = "Updating a MoJ Minister with no name should throw an ArgumentNullException")]
        public async Task UpdateMoJMinisterWithNoNameShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.MoJMinisters.AddAsync(new MoJMinister
            {
                MoJMinisterID = 2,
                prefix = "Ambassador",
                Name = "Delenn",
                suffix = "Grey Council",
                active = false
            });

            await context.SaveChangesAsync();
            var mojMinisterComponent = new MoJMinisterComponent(context);

            // Act
            Func<Task> act = async () =>
            {
                await mojMinisterComponent.UpdateMoJMinisterAsync(new Models.MoJMinisterModel
                {
                    MoJMinisterId = 2,
                    Prefix = "Ambassador",
                    Name = "",
                    Suffix = "Grey Council",
                    Active = false
                });
            };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
