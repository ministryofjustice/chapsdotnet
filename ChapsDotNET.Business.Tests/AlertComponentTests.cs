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
    public class AlertsComponentTests
    {
        [Fact(DisplayName = "Get a list of Alerts when GetAlertsAsync is called")]
        public async Task GetAListOfAlertsWhenGetAlertsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 1,
                Message = "Brexit",
                active = true
            });
            await context.SaveChangesAsync();

            var AlertComponent = new AlertComponent(context);

            // Act
            var result = await AlertComponent.GetAlertsAsync(new AlertRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            result.Results?.First().Message.Should().Be("Brexit");
            result.Results?.First().AlertId.Should().Be(1);
            result.Results?.First().Live.Should().BeTrue();
        }
        //  Alert
        [Fact(DisplayName = "Get a list of only active Alerts when GetAlertsAsync is called without true in the parameter")]
        public async Task GetAListOfOnlyActiveAlertsWhenGetAlertsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 1,
                Message = "Courts",
                live = true
            });
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 2,
                Message = "Brexit",
                live = true
            });
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 3,
                Message = "Costs",
                live = false
            });
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 4,
                Message = "Correspondence",
                live = true
            });

            await context.SaveChangesAsync();

            var AlertComponent = new AlertComponent(context);

            // Act
            var result = await AlertComponent.GetAlertsAsync(new AlertRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(3);
            result.Results?.All(x => x.Live).Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of only all Alerts when GetAlertsAsync is called with true ShowActiveInactive in the parameter")]
        public async Task GetAListOfActiveAndInactiveAlertsWhenGetAlertsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 1,
                Message = "Courts",
                live = true
            });
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 2,
                Message = "Brexit",
                live = true
            });
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 3,
                Message = "Costs",
                live = false
            });
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 4,
                Message = "Correspondence",
                live = true
            });

            await context.SaveChangesAsync();

            var AlertComponent = new AlertComponent(context);

            // Act
            var result = await AlertComponent.GetAlertsAsync(new AlertRequestModel
            {
                ShowActiveAndInactive = true
            });

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(4);
        }

        [Fact(DisplayName = "When we call GetAlertsAsync we get paged results by default")]
        public async Task GetAlertsAsyncReturnsPagedResults()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var fakeAlerts = FakeAlertsProvider.FakeData
                .Generate(50)
                .ToList();
            await context.Alerts.AddRangeAsync(fakeAlerts);
            await context.SaveChangesAsync();
            var AlertComponent = new AlertComponent(context);

            // Act
            var result = await AlertComponent.GetAlertsAsync(new AlertRequestModel
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


        [Fact(DisplayName = "Get a specific Alert")]
        public async Task GetAlertAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 2,
                Message = "Brexit",
                live = true
            });

            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 53,
                Message = "Courts",
                live = true
            });

            await context.SaveChangesAsync();

            var AlertComponent = new AlertComponent(context);

            // Act
            var result = await AlertComponent.GetAlertAsync(53);

            // Assert
            result.Should().NotBeNull();

            result.Message.Should().Be("Courts");
            result.AlertId.Should().Be(53);
            result.Live.Should().BeTrue();


        }

        [Fact(DisplayName = "What happens for the wrong AlertId?")]
        public async Task GetWrongAlertAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 2,
                Message = "Brexit",
                live = true
            });

            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 54,
                Message = "Courts",
                live = true
            });

            await context.SaveChangesAsync();

            var AlertComponent = new AlertComponent(context);

            // Act
            var result = await AlertComponent.GetAlertAsync(53);

            // Assert
            result.Message.Should().BeNull();
        }


        [Fact(DisplayName = "Add a Alert to the database when calling the AddAlert method returns correct AlertId")]
        public async Task AddAAlertToTheDatabase()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var AlertComponent = new AlertComponent(context);
            var newrecord = new Models.AlertModel
            {
                AlertId = 1,
                Message = "Brexit",
                Live = true
            };

            // Act
            var result = await AlertComponent.AddAlertAsync(newrecord);


            // Assert
            result.Should().NotBe(0);
            context.Alerts.First().Message.Should().Be("Brexit");
            context.Alerts.First().live.Should().Be(true);

        }

        [Fact(DisplayName = "Adding a Alert with empty message should throw an ArgumentNullException")]
        public async Task AddingAnEmptyMessageShouldThrowException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var AlertComponent = new AlertComponent(context);
            var newrecord = new Models.AlertModel
            {
                AlertId = 1,
                Message = "",
                Live = true
            };

            //Act
            Func<Task> act = async () => { await AlertComponent.AddAlertAsync(newrecord); };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }


        [Fact(DisplayName = "Updating active status on a Alert from the database when calling the UpdateAlertActiveStatus")]
        public async Task UpdateActiveStatusOnAAlert()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 1,
                Message = "Brexit",
                live = true
            });

            await context.SaveChangesAsync();

            var AlertComponent = new AlertComponent(context);

            // Act
            await AlertComponent.UpdateAlertAsync(new Models.AlertModel
            {
                AlertId = 1,
                Live = false,
                Message = "Courts"
            });

            // Assert

            context.Alerts.First().Message.Should().Be("Courts");
            context.Alerts.First().live.Should().BeFalse();
        }

        [Fact(DisplayName = "Updating active status on a Alert when calling the UpdateAlertAsync with no Id")]
        public async Task UpdateActiveStatusOnAAlertWithNoIdShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 1,
                Message = "Brexit",
                live = true
            });

            await context.SaveChangesAsync();

            var AlertComponent = new AlertComponent(context);

            // Act

            Func<Task> act = async () => { await AlertComponent.UpdateAlertAsync(new Models.AlertModel()); };

            //Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact(DisplayName = "Updating Alert with Empty Detail should throw an ArgumentNullException")]
        public async Task UpdateAlertWithNoDetailShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.Alerts.AddAsync(new Alert
            {
                AlertId = 1,
                Message = "Brexit",
                live = true
            });

            await context.SaveChangesAsync();

            var AlertComponent = new AlertComponent(context);

            // Act

            Func<Task> act = async () =>
            {
                await AlertComponent.UpdateAlertAsync(new Models.AlertModel
                {
                    Message = string.Empty,
                    AlertId = 1,
                    Live = true
                });
            };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

    }
}


