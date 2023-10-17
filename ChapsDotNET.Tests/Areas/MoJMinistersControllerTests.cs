using ChapsDotNET.Areas.Admin.Controllers;
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
	public class MoJMinistersControllerTests
	{
        [Fact]
        public async Task WhenMoJMinistersIndexPageIsCalledAMoJMinistersListShouldBeReturned()
        {
            //Arrange
            var mockMoJMinisterComponent = Substitute.For<IMoJMinisterComponent>();
            var mockUserComponent = Substitute.For<IUserComponent>();

            mockMoJMinisterComponent.GetMoJMinistersAsync(Arg.Any<MoJMinisterRequestModel>()).Returns(
                new PagedResult<List<MoJMinisterModel>>
                {
                    Results = new List<MoJMinisterModel>()
                    {
                        new MoJMinisterModel
                        {
                            MoJMinisterId = 1,
                            Prefix = "Emperor",
                            Name = "Londo Mollari",
                            Suffix = "Paso Leati",
                            Active = true
                        },
                        new MoJMinisterModel
                        {
                            MoJMinisterId = 2,
                            Prefix = "Ambassador",
                            Name = "Delenn",
                            Suffix = "Grey Council",
                            Active = true
                        }
                    }
                });
            var controller = new MoJMinistersController(mockMoJMinisterComponent, mockUserComponent);

            // Act
            var result = await controller.Index() as ViewResult;
            var resultCount = result?.Model as List<MoJMinisterModel>;

            // Assert
            await mockMoJMinisterComponent.Received().GetMoJMinistersAsync(Arg.Any<MoJMinisterRequestModel>());
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            resultCount?.Count.Should().Be(2);
        }

        [Fact]
        public void WhenCreateMethodIsCalledTheCreateViewIsReturned()
        {
            //Arrange
            var mockMoJMinisterComponent = Substitute.For<IMoJMinisterComponent>();
            var mockUserComponent = Substitute.For<IUserComponent>();

            var controller = new MoJMinistersController(mockMoJMinisterComponent, mockUserComponent);

            //Act
            var result = controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenCreateMethodIsCalledTheAddMoJMinistersAsyncMethodIsCalled()
        {
            //Arrange
            var mockMoJMinisterComponent = Substitute.For<IMoJMinisterComponent>();
            var mockUserComponent = Substitute.For<IUserComponent>();

            var controller = new MoJMinistersController(mockMoJMinisterComponent, mockUserComponent);
            var mojMinisterViewModel = new MoJMinisterViewModel()
            {
                MoJMinisterId = 2,
                Prefix = "Ambassador",
                Name = "Delenn",
                Suffix = "Grey Council",
                Active = true
            };

            //Act
            var result = await controller.Create(mojMinisterViewModel);

            //Assert
            await mockMoJMinisterComponent.Received().AddMoJMinisterAsync(Arg.Any<MoJMinisterModel>());
            result.Should().NotBe(null);
        }

        [Fact]
        public async Task WhenEditMethodIsCalledTheEditViewIsReturned()
        {
            //Arrange
            var mockMoJMinisterComponent = Substitute.For<IMoJMinisterComponent>();
            var mockUserComponent = Substitute.For<IUserComponent>();

            MoJMinistersController controller = new MoJMinistersController(mockMoJMinisterComponent, mockUserComponent);
            mockMoJMinisterComponent.GetMoJMinisterAsync(1).Returns(new MoJMinisterModel
            {
                MoJMinisterId = 3,
                Prefix = "Captain",
                Name = "John Sheridan",
                Suffix = "",
                Active = false
            }); 
            
            //Act
            var result = await controller.Edit(1);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenSaveButtonIsClickedOnTheEditScreenThenUpdateMoJMinisterAsyncIsCalled()
        {
            //Arrange
            var mockMoJMinisterComponent = Substitute.For<IMoJMinisterComponent>();
            var mockUserComponent = Substitute.For<IUserComponent>();

            var controller = new MoJMinistersController(mockMoJMinisterComponent, mockUserComponent);

            //Act
            var result = await controller.Edit(new MoJMinisterViewModel
            {
                MoJMinisterId = 4,
                Prefix = "Ambassador",
                Name = "G'Kar",
                Suffix = "The Red Knight",
                Active = true
            });

            //Assert
            await mockMoJMinisterComponent.Received().UpdateMoJMinisterAsync(Arg.Any<MoJMinisterModel>());
            result.Should().BeOfType<RedirectToActionResult>();
        }
	}
}
