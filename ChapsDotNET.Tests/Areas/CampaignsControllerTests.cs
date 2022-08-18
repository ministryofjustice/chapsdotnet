﻿using ChapsDotNET.Areas.Admin.Controllers;
using ChapsDotNET.Business.Components;
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
    public class CampaignsControllerTests
    {
        [Fact]
        public async Task WhenCampaignIndexPageIsCalledWeShouldBeReturnedWithCampaignsList()
        {
            //Arrange
            var mockCampaignComponent = Substitute.For<ICampaignComponent>();
            mockCampaignComponent.GetCampaignsAsync(Arg.Any<CampaignRequestModel>()).Returns(
                new PagedResult<List<CampaignModel>>
                {
                    Results = new List<CampaignModel>()
                    {
                        new CampaignModel
                        {
                            Detail = "Summer 2022",
                            Active = true,
                            CampaignId = 1
                        },
                        new CampaignModel
                        {
                            Detail = "Winter 2022",
                            Active = true,
                            CampaignId = 2
                        }
                    }
                });
            var controller = new CampaignsController(mockCampaignComponent);

            // Act
            var result = await controller.Index() as ViewResult;
            var resultCount = result?.Model as List<CampaignModel>;

            // Assert
            await mockCampaignComponent.Received().GetCampaignsAsync(Arg.Any<CampaignRequestModel>());
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            resultCount?.Count.Should().Be(2);
        }

        [Fact]
        public void WhenCreateCampaignMethodIsCalledTheCreateViewIsReturned()
        {
            //Arrange
            var mockCampaignComponent = Substitute.For<ICampaignComponent>();
            var controller = new CampaignsController(mockCampaignComponent);

            //Act
            var result = controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }



        [Fact]
        public async Task WhenCreateMethodIsCalledTheAddCampaignAsyncMethodIsCalled()
        {
            //Arrange
            var mockCampaignComponent = Substitute.For<ICampaignComponent>();
            var controller = new CampaignsController(mockCampaignComponent);
            var campaignViewModel = new CampaignViewModel()
            {
                Detail = "Summer 2022",
                Active = true,
                CampaignId = 1
            };

            //Act
            var result = await controller.Create(campaignViewModel);

            //Assert
            await mockCampaignComponent.Received().AddCampaignAsync(Arg.Any<CampaignModel>());
            result.Should().NotBe(null);

        }



        [Fact]
        public async Task WhenEditCampaignMethodIsCalledTheEditViewIsReturned()
        {
            //Arrange
            var mockCampaignComponent = Substitute.For<ICampaignComponent>();
            CampaignsController controller = new CampaignsController(mockCampaignComponent);
            mockCampaignComponent.GetCampaignAsync(1).Returns(new CampaignModel
            {
                Detail = "Summer 2022",
                Active = true,
                CampaignId = 1
            });


            //Act
            var result = await controller.Edit(1);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenSaveButtonIsClickedOnTheEditScreenThenUpdateCampaignAsyncIsCalled()
        {
            //Arrange
            var mockCampaignComponent = Substitute.For<ICampaignComponent>();
            var controller = new CampaignsController(mockCampaignComponent);


            //Act
            var result = await controller.Edit(new CampaignViewModel
            {
                Detail = "Summer 2022",
                Active = true,
                CampaignId = 1
            });

            //Assert
            await mockCampaignComponent.Received().UpdateCampaignAsync(Arg.Any<CampaignModel>());
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}



