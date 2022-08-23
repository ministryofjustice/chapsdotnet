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
    public class LeadSubjectControllerTests
    {
        [Fact]
        public async Task WhenLeadSubjectIndexPageIsCalledWeShouldBeReturnedWithLeadSubjectsList()
        {
            //Arrange
            var mockLeadSubjectsComponent = Substitute.For<ILeadSubjectComponent>();
            mockLeadSubjectsComponent.GetLeadSubjectsAsync(Arg.Any<LeadSubjectRequestModel>()).Returns(
                new PagedResult<List<LeadSubjectModel>>
                {
                    Results = new List<LeadSubjectModel>()
                    {
                        new LeadSubjectModel
                        {
                            Detail = "Brexit",
                            Active = true,
                            LeadSubjectId = 1
                        },
                        new LeadSubjectModel
                        {
                            Detail = "Brexit",
                            Active = true,
                            LeadSubjectId = 2
                        }
                    }
                });
            var controller = new LeadSubjectsController(mockLeadSubjectsComponent);

            // Act
            var result = await controller.Index() as ViewResult;
            var resultCount = result?.Model as List<LeadSubjectModel>;

            // Assert
            await mockLeadSubjectsComponent.Received().GetLeadSubjectsAsync(Arg.Any<LeadSubjectRequestModel>());
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            resultCount?.Count.Should().Be(2);
        }

        [Fact]
        public void WhenCreateLeadSubjectMethodIsCalledTheCreateViewIsReturned()
        {
            //Arrange
            var mockLeadSubjectsComponent = Substitute.For<ILeadSubjectComponent>();
            var controller = new LeadSubjectsController(mockLeadSubjectsComponent);

            //Act
            var result = controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }



        [Fact]
        public async Task WhenCreateMethodIsCalledTheAddLeadSubjectsAsyncMethodIsCalled()
        {
            //Arrange
            var mockLeadSubjectComponent = Substitute.For<ILeadSubjectComponent>();
            var controller = new LeadSubjectsController(mockLeadSubjectComponent);
            var leadSubjectViewModel = new LeadSubjectViewModel()
            {
                Detail = "Brexit",
                Active = true,
                LeadSubjectId = 1
            };

            //Act
            var result = await controller.Create(leadSubjectViewModel);

            //Assert
            await mockLeadSubjectComponent.Received().AddLeadSubjectAsync(Arg.Any<LeadSubjectModel>());
            result.Should().NotBe(null);

        }



        [Fact]
        public async Task WhenEditLeadSubjectMethodIsCalledTheEditViewIsReturned()
        {
            //Arrange
            var mockLeadSubjectComponent = Substitute.For<ILeadSubjectComponent>();
            LeadSubjectsController controller = new LeadSubjectsController(mockLeadSubjectComponent);
            mockLeadSubjectComponent.GetLeadSubjectAsync(1).Returns(new LeadSubjectModel
            {
                Detail = "Brexit",
                Active = true,
                LeadSubjectId = 1
            });


            //Act
            var result = await controller.Edit(1);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenSaveButtonIsClickedOnTheEditScreenThenUpdateLeadSubjectAsyncIsCalled()
        {
            //Arrange
            var mockLeadSubjectComponent = Substitute.For<ILeadSubjectComponent>();
            var controller = new LeadSubjectsController(mockLeadSubjectComponent);


            //Act
            var result = await controller.Edit(new LeadSubjectViewModel
            {
                Detail = "Brexit",
                Active = true,
                LeadSubjectId = 1
            });

            //Assert
            await mockLeadSubjectComponent.Received().UpdateLeadSubjectAsync(Arg.Any<LeadSubjectModel>());
            result.Should().BeOfType<RedirectToActionResult>();
        }

    }
}

