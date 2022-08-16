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
    public class CampaignComponentTests
    {
        [Fact(DisplayName = "Get a list of Campaigns when GetCampaignsAsync is called")]
        public async Task GetAListOfCampaignsWhenGetCampaignsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 1,
                Detail = "Mr",
                active = true
            });
            await context.SaveChangesAsync();

            var CampaignComponent = new CampaignComponent(context);

            // Act
            var result = await CampaignComponent.GetCampaignsAsync(new CampaignRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            result.Results?.First().Detail.Should().Be("Mr");
            result.Results?.First().CampaignId.Should().Be(1);
            result.Results?.First().Active.Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of only active Campaigns when GetCampaignsAsync is called without true in the parameter")]
        public async Task GetAListOfOnlyActiveCampaignsWhenGetCampaignsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 1,
                Detail = "Mr",
                active = true
            });
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 2,
                Detail = "Mrs",
                active = true
            });
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 3,
                Detail = "Dr",
                active = false
            });
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 4,
                Detail = "Miss",
                active = true
            });

            await context.SaveChangesAsync();

            var CampaignComponent = new CampaignComponent(context);

            // Act
            var result = await CampaignComponent.GetCampaignsAsync(new CampaignRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(3);
            result.Results?.All(x => x.Active).Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of only all Campaigns when GetCampaignsAsync is called with true ShowActiveInactive in the parameter")]
        public async Task GetAListOfActiveAndInactiveCampaignsWhenGetCampaignsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 1,
                Detail = "Mr",
                active = true
            });
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 2,
                Detail = "Mrs",
                active = true
            });
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 3,
                Detail = "Dr",
                active = false
            });
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 4,
                Detail = "Miss",
                active = true
            });

            await context.SaveChangesAsync();

            var CampaignComponent = new CampaignComponent(context);

            // Act
            var result = await CampaignComponent.GetCampaignsAsync(new CampaignRequestModel
            {
                ShowActiveAndInactive = true
            });

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(4);
        }

        [Fact(DisplayName = "When we call GetCampaignsAsync we get paged results by default")]
        public async Task GetCampaignsAsyncReturnsPagedResults()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var fakeCampaigns = FakeCampaignsProvider.FakeData
                .Generate(50)
                .ToList();
            await context.Campaigns.AddRangeAsync(fakeCampaigns);
            await context.SaveChangesAsync();
            var CampaignComponent = new CampaignComponent(context);

            // Act
            var result = await CampaignComponent.GetCampaignsAsync(new CampaignRequestModel
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


        [Fact(DisplayName = "Get a specific Campaign")]
        public async Task GetCampaignAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 2,
                Detail = "Summer 2022",
                active = true
            });

            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 53,
                Detail = "Winter 2022",
                active = true
            });

            await context.SaveChangesAsync();

            var CampaignComponent = new CampaignComponent(context);

            // Act
            var result = await CampaignComponent.GetCampaignAsync(53);

            // Assert
            result.Should().NotBeNull();

            result.Detail.Should().Be("Winter 2022");
            result.CampaignId.Should().Be(53);
            result.Active.Should().BeTrue();


        }

        [Fact(DisplayName = "What happens for the wrong Campaign id?")]
        public async Task GetWrongCampaignAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 2,
                Detail = "Summer 2022",
                active = true
            });

            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 54,
                Detail = "Winter 2022",
                active = true
            });

            await context.SaveChangesAsync();

            var CampaignComponent = new CampaignComponent(context);

            // Act
            var result = await CampaignComponent.GetCampaignAsync(53);

            // Assert
            result.Detail.Should().BeEmpty();
        }


        [Fact(DisplayName = "Add a Campaign to the database when calling the AddCampaign method returns correct CampaignID")]
        public async Task AddACampaignToTheDatabase()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var CampaignComponent = new CampaignComponent(context);
            var newrecord = new Models.CampaignModel
            {
                CampaignId = 1,
                Detail = "AAA",
                Active = true
            };

            // Act
            var result = await CampaignComponent.AddCampaignAsync(newrecord);


            // Assert
            result.Should().NotBe(0);
            context.Campaigns.First().Detail.Should().Be("AAA");
            context.Campaigns.First().active.Should().Be(true);

        }

        [Fact(DisplayName = "Adding a Campaign with empty detail should throw an ArgumentNullException")]
        public async Task AddingAnEmptyTitleShouldThrowException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var CampaignComponent = new CampaignComponent(context);
            var newrecord = new Models.CampaignModel
            {
                CampaignId = 1,
                Detail = "",
                Active = true
            };

            //Act
            Func<Task> act = async () => { await CampaignComponent.AddCampaignAsync(newrecord); };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }


        [Fact(DisplayName = "Updating active status on a Campaign from the database when calling the UpdateCampaignActiveStatus")]
        public async Task UpdateActiveStatusOnACampaign()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 1,
                Detail = "AAA",
                active = true
            });

            await context.SaveChangesAsync();

            var CampaignComponent = new CampaignComponent(context);

            // Act
            await CampaignComponent.UpdateCampaignAsync(new Models.CampaignModel
            {
                CampaignId = 1,
                Active = false,
                Detail = "BBB"
            });

            // Assert

            context.Campaigns.First().Detail.Should().Be("BBB");
            context.Campaigns.First().active.Should().BeFalse();
        }

        [Fact(DisplayName = "Updating active status on a Campaign when calling the UpdateCampaignAsync with no Id")]
        public async Task UpdateActiveStatusOnACampaignWithNoIDShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 1,
                Detail = "AAA",
                active = true
            });

            await context.SaveChangesAsync();

            var CampaignComponent = new CampaignComponent(context);

            // Act

            Func<Task> act = async () => { await CampaignComponent.UpdateCampaignAsync(new Models.CampaignModel()); };

            //Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact(DisplayName = "Updating Campaign with Empty Detail should throw an ArgumentNullException")]
        public async Task UpdateCampaignWithNoDetailShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.Campaigns.AddAsync(new Campaign
            {
                CampaignID = 1,
                Detail = "AAA",
                active = true
            });

            await context.SaveChangesAsync();

            var CampaignComponent = new CampaignComponent(context);

            // Act

            Func<Task> act = async () =>
            {
                await CampaignComponent.UpdateCampaignAsync(new Models.CampaignModel
                {
                    Detail = string.Empty,
                    CampaignId = 1,
                    Active = true
                });
            };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

    }
}
