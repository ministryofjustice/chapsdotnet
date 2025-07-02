using Azure;
using ChapsDotNET.Areas.Admin.Controllers;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Frontend.Components.Breadcrumbs;
using ChapsDotNET.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Yarp.ReverseProxy.Configuration;
//        PublicHoliday
namespace ChapsDotNET.Tests.Areas
{
    public class PublicHolidaysControllerTests
    {
        [Fact]
        public async Task WhenPublicHolidayIndexPageIsCalledWeShouldBeReturnedWithPublicHolidayList()
        {
            //Arrange
            var mockPublicHolidayComponent = Substitute.For<IPublicHolidayComponent>();
            mockPublicHolidayComponent.GetPublicHolidaysAsync(Arg.Any<PublicHolidayRequestModel>()).Returns(
                new PagedResult<List<PublicHolidayModel>>
                {
                    Results = new List<PublicHolidayModel>()
                    {
                        new PublicHolidayModel
                        {
                            Date = new DateTime(2022,08,31),
                            Description = "August Bank Holiday",
                            PublicHolidayId = 1
                        },
                        new PublicHolidayModel
                        {
                            Date = new DateTime(2022,12,25),
                            Description = "Christmas Day",
                            PublicHolidayId = 2
                        }
                    }
                });
            var controller = new PublicHolidaysController(mockPublicHolidayComponent);


            //Mock Bookmarks Url.Action
            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<UrlActionContext>())
                     .Returns(callInfo =>
                     {
                         var _callInfo = callInfo.Arg<UrlActionContext>();
                         return $"/{_callInfo.Controller}/{_callInfo.Action}";
                     });
            controller.Url = urlHelper;

            //Mock Alerts
            var httpContext = new DefaultHttpContext();
            var mockTempData = Substitute.For<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, mockTempData)
            {
                ["alertContent"] = "YourValue"
            };
            controller.TempData = tempData;

            // Act
            var result = await controller.Index() as ViewResult;
            var resultCount = result?.Model as List<PublicHolidayModel>;

            // Assert
            await mockPublicHolidayComponent.Received().GetPublicHolidaysAsync(Arg.Any<PublicHolidayRequestModel>());
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            resultCount?.Count.Should().Be(2);
        }

        [Fact]
        public void WhenCreatePublicHolidayMethodIsCalledTheCreateViewIsReturned()
        {
            //Arrange
            var mockPublicHolidayComponent = Substitute.For<IPublicHolidayComponent>();
            var controller = new PublicHolidaysController(mockPublicHolidayComponent);

            //Act
            var result = controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }



        [Fact]
        public async Task WhenCreateMethodIsCalledTheAddPublicHolidayAsyncMethodIsCalled()
        {
            //Arrange
            var mockPublicHolidayComponent = Substitute.For<IPublicHolidayComponent>();
            var controller = new PublicHolidaysController(mockPublicHolidayComponent);
            var publicHolidayViewModel = new PublicHolidayViewModel()
            {
                Date = new DateTime(2022, 08, 31),
                Description = "August Bank Holiday",
                PublicHolidayId = 1
            };

            var httpContext = new DefaultHttpContext();
            var mockTempData = Substitute.For<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, mockTempData)
            {
                ["alertContent"] = "YourValue"
            };
            controller.TempData = tempData;

            mockPublicHolidayComponent.AddPublicHolidayAsync(Arg.Any<PublicHolidayModel>()).Returns(1);
            mockPublicHolidayComponent.GetPublicHolidayAsync(1).Returns(publicHolidayViewModel.ToModel());

            //Act
            var result = await controller.Create(publicHolidayViewModel);

            //Assert
            await mockPublicHolidayComponent.Received().AddPublicHolidayAsync(Arg.Any<PublicHolidayModel>());
            result.Should().NotBe(null);

        }



        [Fact]
        public async Task WhenEditPublicHolidayMethodIsCalledTheEditViewIsReturned()
        {
            //Arrange
            var mockPublicHolidayComponent = Substitute.For<IPublicHolidayComponent>();
            PublicHolidaysController controller = new PublicHolidaysController(mockPublicHolidayComponent);
            mockPublicHolidayComponent.GetPublicHolidayAsync(1).Returns(new PublicHolidayModel
            {
                Date = new DateTime(2022, 08, 31),
                Description = "August Bank Holiday",
                PublicHolidayId = 1
            });


            //Act
            var result = await controller.Edit(1);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenSaveButtonIsClickedOnTheEditScreenThenUpdatePublicHolidayAsyncIsCalled()
        {
            //Arrange
            var mockPublicHolidayComponent = Substitute.For<IPublicHolidayComponent>();
            var controller = new PublicHolidaysController(mockPublicHolidayComponent);

            var httpContext = new DefaultHttpContext();
            var mockTempData = Substitute.For<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, mockTempData)
            {
                ["alertContent"] = "YourValue"
            };
            controller.TempData = tempData;

            var publicHolidayViewModel = new PublicHolidayViewModel()
            {
                Date = new DateTime(2022, 08, 31),
                Description = "August Bank Holiday",
                PublicHolidayId = 1
            };

            mockPublicHolidayComponent.UpdatePublicHolidayAsync(Arg.Any<PublicHolidayModel>()).Returns(1);
            mockPublicHolidayComponent.GetPublicHolidayAsync(1).Returns(publicHolidayViewModel.ToModel());

            //Act
            var result = await controller.Edit(new PublicHolidayViewModel
            {
                Date = new DateTime(2022, 08, 31),
                Description = "August Bank Holiday",
                PublicHolidayId = 1
            });

            //Assert
            await mockPublicHolidayComponent.Received().UpdatePublicHolidayAsync(Arg.Any<PublicHolidayModel>());
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}



