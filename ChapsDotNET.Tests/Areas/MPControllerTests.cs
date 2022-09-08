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
	public class MPControllerTests
	{
        [Fact]
        public async Task WhenMPIndexPageIsCalledAListOfMPsShouldBeReturned()
        {
            //Arrange
            var mockMPComponent = Substitute.For<IMPComponent>();
            mockMPComponent.GetMPsAsync(Arg.Any<MPRequestModel>()).Returns(
                new PagedResult<List<MPModel>>
                {
                    Results = new List<MPModel>()
                    {
                        new MPModel
                        {
                            MPId = 1,
                            RtHon = true,
                            SalutationId = 3,
                            Surname = "Mollari",
                            FirstNames = "Londo",
                            Email = "Londo.Mollari@CentauriPrime.com",
                            AddressLine1 = "8 Acacia Avenue", 
                            AddressLine2 = "",
                            AddressLine3 = "",
                            Town = "Hobbiton",
                            County = "Shire",
                            Postcode = "XX33 Q45",
                            Suffix = "Paso Leati",
                            Active = true
                        },
                        new MPModel
                        {
                            MPId = 2,
                            RtHon = true,
                            SalutationId = 3,
                            Surname = "Mollari",
                            FirstNames = "Londo",
                            Email = "Londo.Mollari@CentauriPrime.com",
                            AddressLine1 = "8 Acacia Avenue", 
                            AddressLine2 = "",
                            AddressLine3 = "",
                            Town = "Hobbiton",
                            County = "Shire",
                            Postcode = "XX33 Q45",
                            Suffix = "Paso Leati",
                            Active = true
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

