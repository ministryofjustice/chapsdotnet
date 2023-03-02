using ChapsDotNET.Areas.Admin.Controllers;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ChapsDotNET.Tests.Areas
{
    public class AlertControllerTests
    {
        [Fact]
        public async Task WhenAlertIndexPageIsCalledWeShouldBeReturnedWithAlertsList()
        {
            //Arrange
            var mockAlertsComponent = Substitute.For<IAlertComponent>();
            mockAlertsComponent.GetAlertsAsync(Arg.Any<AlertRequestModel>()).Returns(
                new PagedResult<List<AlertModel>>
                {
                    Results = new List<AlertModel>()
                    {
                        new AlertModel
                        {
                            Message = "Brexit",
                            Live = true,
                            AlertId = 1
                        },
                        new AlertModel
                        {
                            Message = "Brexit",
                            Live = true,
                            AlertId = 2
                        }
                    }
                });
            var controller = new AlertsController(mockAlertsComponent);

            // Act
            var result = await controller.Index() as ViewResult;
            var resultCount = result?.Model as List<AlertModel>;

            // Assert
            await mockAlertsComponent.Received().GetAlertsAsync(Arg.Any<AlertRequestModel>());
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            resultCount?.Count.Should().Be(2);
        }

        [Fact]
        public void WhenCreateAlertMethodIsCalledTheCreateViewIsReturned()
        {
            //Arrange
            var mockAlertsComponent = Substitute.For<IAlertComponent>();
            var controller = new AlertsController(mockAlertsComponent);

            //Act
            var result = controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }



        [Fact]
        public async Task WhenCreateMethodIsCalledTheAddAlertsAsyncMethodIsCalled()
        {
            //Arrange
            var mockAlertComponent = Substitute.For<IAlertComponent>();
            var controller = new AlertsController(mockAlertComponent);
            var AlertViewModel = new AlertViewModel()
            {
                Message = "Brexit",
                live = true,
                AlertId = 1
            };

            //Act
            var result = await controller.Create(AlertViewModel);

            //Assert
            await mockAlertComponent.Received().AddAlertAsync(Arg.Any<AlertModel>());
            result.Should().NotBe(null);

        }



        [Fact]
        public async Task WhenEditAlertMethodIsCalledTheEditViewIsReturned()
        {
            //Arrange
            var mockAlertComponent = Substitute.For<IAlertComponent>();
            AlertsController controller = new AlertsController(mockAlertComponent);
            mockAlertComponent.GetAlertAsync(1).Returns(new AlertModel
            {
                Message = "Brexit",
                Live = true,
                AlertId = 1
            });


            //Act
            var result = await controller.Edit(1);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenSaveButtonIsClickedOnTheEditScreenThenUpdateAlertAsyncIsCalled()
        {
            //Arrange
            var mockAlertComponent = Substitute.For<IAlertComponent>();
            var controller = new AlertsController(mockAlertComponent);


            //Act
            var result = await controller.Edit(new AlertViewModel
            {
                Message = "Brexit",
                live = true,
                AlertId = 1
            });

            //Assert
            await mockAlertComponent.Received().UpdateAlertAsync(Arg.Any<AlertModel>());
            result.Should().BeOfType<RedirectToActionResult>();
        }

    }
}

