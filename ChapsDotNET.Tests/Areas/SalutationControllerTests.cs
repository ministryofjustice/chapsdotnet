﻿using ChapsDotNET.Areas.Admin.Controllers;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
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
    }
}