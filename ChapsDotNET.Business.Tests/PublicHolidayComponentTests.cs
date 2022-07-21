using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Business.Tests.Common;
using ChapsDotNET.Data.Entities;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChapsDotNET.Business.Tests
{
    public class PublicHolidayComponentTests
    {
        [Fact(DisplayName = "Get a list of public holidays when GetPublicHolidaysAsync is called")]
        public async Task GetAListOfPublicHolidaysWhenGetPublicHolidaysAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.PublicHolidays.AddAsync(new PublicHoliday{
                PublicHolidayID = 1,
                Date = new DateTime(2022,05,04),
                Description = "May the forth be with you"
            });
            await context.SaveChangesAsync();

            var publicHolidayComponent = new PublicHolidayComponent(context);

            // Act
            var result = await publicHolidayComponent.GetPublicHolidaysAsync(new PublicHolidayRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            result.Results?.First().Description.Should().Be("May the forth be with you");
            result.Results?.First().PublicHolidayID.Should().Be(1);
            result.Results?.First().Date.Should().HaveDay(4);
            result.Results?.First().Date.Should().HaveMonth(5);
            result.Results?.First().Date.Should().HaveYear(2022);
        }

        [Fact(DisplayName = "When we call GetSalutationsAsync we get paged results by default")]
        public async Task GetPublicHolidaysAsyncReturnsPagedResults()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var fakePublicHolidays = FakePublicHolidaysProvider.FakeData
                .Generate(50)
                .ToList();
            await context.PublicHolidays.AddRangeAsync(fakePublicHolidays);
            await context.SaveChangesAsync();
            var publicHolidaysComponent = new PublicHolidayComponent(context);

            // Act
            var result = await publicHolidaysComponent.GetPublicHolidaysAsync(new PublicHolidayRequestModel
            {
                ShowActiveAndInactive = true
            });

            // Assert
            result.PageSize.Should().Be(10);
            result.PageCount.Should().Be(5);
            result.RowCount.Should().Be(50);
            result.CurrentPage.Should().Be(1);
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(10);
        }
    }
}
