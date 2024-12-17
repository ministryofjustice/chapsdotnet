using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Tests.Common;
using ChapsDotNET.Data.Entities;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChapsDotNET.Business.Tests
{
    public class ReportsComponentTests
    {
        [Fact(DisplayName = "Get a list of Reports when GetReportsAsync is called")]
        public async Task GetAListOfReportsWhenGetReportsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Reports.AddAsync(new Report
            {
                ReportId = 1,
                Name = "Report 1",
                Description = "Description 1",
                LongDescription = "Long Description 1"
            });
            await context.SaveChangesAsync();

            var ReportComponent = new ReportComponent(context);

            // Act
            var result = await ReportComponent.GetReportsAsync();

            // Assert
            result.Should().NotBeNull();
            result.First().Name.Should().Be("Report 1");
            result.First().Description.Should().Be("Description 1");
            result.First().LongDescription.Should().Be("Long Description 1");

        }

        [Fact(DisplayName = "Get a specific Report")]
        public async Task GetReportAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Reports.AddAsync(new Report
            {
                ReportId = 2,
                Name = "Report 2",
                Description = "Description 2",
                LongDescription = "Long Description 2"
                
            });

            await context.Reports.AddAsync(new Report
            {
                ReportId = 53,
                Name = "Report 53",
                Description = "Description 53",
                LongDescription = "Long Description 53"
            });

            await context.SaveChangesAsync();

            var ReportComponent = new ReportComponent(context);

            // Act
            var result = await ReportComponent.GetReportAsync(53);

            // Assert
            result.Should().NotBeNull();
            result.ReportId.Should().Be(53);
            result.Name.Should().Be("Report 53");
            result.Description.Should().Be("Description 53");
            result.LongDescription.Should().Be("Long Description 53");
        }

        [Fact(DisplayName = "What happens for the wrong ReportId?")]
        public async Task GetWrongReportAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Reports.AddAsync(new Report
            {
                ReportId = 2,
                Name = "Report 2",
                Description = "Description 2",
                LongDescription = "Long Description 2"
            });

            await context.Reports.AddAsync(new Report
            {
                ReportId = 53,
                Name = "Report 53",
                Description = "Description 53",
                LongDescription = "Long Description 53"
            });

            await context.SaveChangesAsync();

            var ReportComponent = new ReportComponent(context);

            // Act
            var result = await ReportComponent.GetReportAsync(54);

            // Assert
            result.Description.Should().BeNull();
            result.LongDescription.Should().BeNull();

        }



    }
}


