using ChapsDotNET.Areas.Admin.Controllers;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ChapsDotNET.Tests.Areas
{
    public class ReportsControllerTests
    {

        [Fact]
        public async Task WhenReportsIndexPageIsCalledReportsListShouldBeReturned()
        {
            //Arrange
            var mockReportComponent = Substitute.For<IReportComponent>();
            mockReportComponent.GetReportsAsync().Returns(
                new List<ReportModel>
                {
                    new ReportModel
                    {
                        Name = "Report Autumn 2022",
                        Description = "Report 1",
                        LongDescription = "Long description of Report 2"
                    },
                    new ReportModel
                    {
                        Name = "Report Winter 2022",
                        Description = "Report 2",
                        LongDescription = "Long description of Report 2"
                    }
                });

            var controller = new ReportController(mockReportComponent);

            // Act
            var result = await controller.Index() as ViewResult;
            var resultCount = result?.Model as List<ReportModel>;

            // Assert
            await mockReportComponent.Received().GetReportsAsync();
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            resultCount?.Count.Should().Be(2);
        }



        [Fact]
        public async Task WhenEditReportMethodIsCalledTheEditViewIsReturned()
        {
            //Arrange
            var mockReportComponent = Substitute.For<IReportComponent>();
            ReportController controller = new ReportController(mockReportComponent);

            mockReportComponent.GetReportAsync(1).Returns(new ReportModel
            {
                Name = "Report Autumn 2022",
                Description = "Report 2",
                LongDescription = "Long description of Report 2"
            });

            //Act
            var result = await controller.Edit(1);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }


        [Fact]
        public async Task WhenSaveButtonIsClickedOnTheEditScreenThenUpdateReportAsyncIsCalled()
        {
            //Arrange
            var mockReportComponent = Substitute.For<IReportComponent>();
            var controller = new ReportController(mockReportComponent);


            //Act
            var result = await controller.Edit(new ReportViewModel
            {
                Name = "Report Autumn 2022",
                Description = "Report 1",
                LongDescription = "Long description of Report 1",
                ReportId = 1
            });

            //Assert
            await mockReportComponent.Received().UpdateReportAsync(Arg.Any<ReportModel>());
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}


