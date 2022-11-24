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
    public class SalutationControllerTests
    {
        

        [Fact]
        public async Task WhenSalutationIndexPageIsCalledWeShouldBeReturnedWithSalutationsList()
        {
            //Arrange
            var mockSalutationsComponent = Substitute.For<ISalutationComponent>();
            mockSalutationsComponent.GetSalutationsAsync(Arg.Any<SalutationRequestModel>()).Returns(
                new PagedResult<List<SalutationModel>>
                {
                    Results = new List<SalutationModel>()
                    {
                        new SalutationModel
                        {
                            Detail = "Mr",
                            Active = true,
                            SalutationId = 1
                        },
                        new SalutationModel
                        {
                            Detail = "Mrs",
                            Active = true,
                            SalutationId = 2
                        }
                    }
                });
            var controller = new SalutationsController(mockSalutationsComponent);

            // Act
            var result = await controller.Index() as ViewResult;
            var resultCount = result?.Model as List<SalutationModel>;

            // Assert
            await mockSalutationsComponent.Received().GetSalutationsAsync(Arg.Any<SalutationRequestModel>());
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            resultCount?.Count.Should().Be(2);
        }

        [Fact]
        public void WhenCreateMethodIsCalledTheCreateViewIsReturned()
        {
            //Arrange
            var mockSalutationsComponent = Substitute.For<ISalutationComponent>();
            var controller = new SalutationsController(mockSalutationsComponent);

            //Act
            var result = controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenCreateMethodIsCalledTheAddSalutationsAsyncMethodIsCalled()
        {
            //Arrange
            var mockSalutationsComponent = Substitute.For<ISalutationComponent>();
            var controller = new SalutationsController(mockSalutationsComponent);
            var salutationViewModel = new SalutationViewModel()
            {
                Detail = "aaa",
                Active = true,
                SalutationId = 1
            };

            //Act
            var result = await controller.Create(salutationViewModel);

            //Assert
            await mockSalutationsComponent.Received().AddSalutationAsync(Arg.Any<SalutationModel>());
            result.Should().NotBe(null);
        }

        [Fact]
        public async Task WhenEditMethodIsCalledTheEditViewIsReturned()
        {
            //Arrange
            var mockSalutationsComponent = Substitute.For<ISalutationComponent>();
            SalutationsController controller = new SalutationsController(mockSalutationsComponent);
            mockSalutationsComponent.GetSalutationAsync(1).Returns(new SalutationModel
            {
                Detail = "aaa",
                Active = true,
                SalutationId = 1
            });

            //Act
            var result = await controller.Edit(1);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenSaveButtonIsClickedOnTheEditScreenThenUpdateSalutationAsyncIsCalled()
        {
            //Arrange
            var mockSalutationsComponent = Substitute.For<ISalutationComponent>();
            var controller = new SalutationsController(mockSalutationsComponent);

            //Act
            var result = await controller.Edit(new SalutationViewModel
            {
                Detail = "Mr",
                Active = true,
                SalutationId = 1
            });

            //Assert
            await mockSalutationsComponent.Received().UpdateSalutationAsync(Arg.Any<SalutationModel>());
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}
