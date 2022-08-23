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
            var mockMoJMinisterComponent = Substitute.For<MoJMinisterComponent>();
            mockMoJMinisterComponent.GetMoJMinistersAsync(Arg.Any<MoJMinisterRequestModel>()).Returns(
                new PagedResult<List<MoJMinisterModel>>
                {
                    Results = new List<MoJMinisterModel>()
                    {
                        new MoJMinisterModel
                        {
                            Prefix = "Emperor",
                            Name = "Londo Mollari",
                            Suffix = "Paso Leati",
                            Active = true,
                            MoJMinisterId = 1
                        },
                        new MoJMinisterModel
                        {
                            Prefix = "Ambassador",
                            Name = "Delenn",
                            Suffix = "Grey Council",
                            Active = true,
                            MoJMinisterId = 2
                        }
                    }
                });
            var controller = new MoJMinistersController(mockMoJMinisterComponent);

            // Act
            var result = await controller.Index() as ViewResult;
            var resultCount = result?.Model as List<MoJMinisterModel>;

            // Assert
            await mockMoJMinisterComponent.Received().GetMoJMinistersAsync(Arg.Any<MoJMinisterRequestModel>());
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            resultCount?.Count.Should().Be(2);
        }
	}
}
