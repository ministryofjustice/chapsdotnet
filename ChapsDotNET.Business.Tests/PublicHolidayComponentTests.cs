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

        [Fact(DisplayName = "When we call PublicHolidaysAsync we get paged results by default")]
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

        [Fact(DisplayName = "Get a specific Public Holiday")]
        public async Task GetPublicHolidayAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.PublicHolidays.AddAsync(new PublicHoliday
            {
                PublicHolidayID = 2,
                Description = "Beltane",
                Date = new DateTime(2022, 05, 01)
            });

            await context.PublicHolidays.AddAsync(new PublicHoliday
            {
                PublicHolidayID = 5,
                Description = "Yule",
                Date = new DateTime(2022, 12, 25)
            });

            await context.SaveChangesAsync();

            var publicHolidayComponent = new PublicHolidayComponent(context);

            // Act
            var result = await publicHolidayComponent.GetPublicHolidayAsync(2);

            // Assert
            result.Should().NotBeNull();
            result.Date.Should().HaveDay(01);
            result.Date.Should().HaveMonth(05);
            result.Date.Should().HaveYear(2022);
            result.PublicHolidayID.Should().Be(2);
            result.Description.Should().Be("Beltane");
        }


        [Fact(DisplayName = "Add a Public Holiday to the database when calling the AddPublicHolidayAsync method returns correct PublicHolidayID")]
        public async Task AddAPublicHolidayToTheDatabase()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var publicHolidayComponent = new PublicHolidayComponent(context);
            var newrecord = new Models.PublicHolidayModel
            {
                PublicHolidayID = 1,
                Description = "Samhain",
                Date = new DateTime(2022, 11, 01)
            };

            // Act
            var result = await publicHolidayComponent.AddPublicHolidayAsync(newrecord);


            // Assert
            result.Should().NotBe(0);
            result.Should().Be(1);
            context.PublicHolidays.First().Description.Should().Be("Samhain");
            context.PublicHolidays.First().Date.Should().HaveDay(01);
            context.PublicHolidays.First().Date.Should().HaveMonth(11);
            context.PublicHolidays.First().Date.Should().HaveYear(2022);
            context.PublicHolidays.First().PublicHolidayID.Should().Be(1);
        }


        [Fact(DisplayName = "Adding a Public Holiday with empty detail should throw an ArgumentNullException")]
        public async Task AddingAnEmptyDescriptionShouldThrowException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var publicHolidayComponent = new PublicHolidayComponent(context);
            var newrecord = new Models.PublicHolidayModel
            {
                PublicHolidayID = 1,
                Description = "",
                Date = new DateTime(2023, 01, 01)
            };

            //Act
            Func<Task> act = async () => { await publicHolidayComponent.AddPublicHolidayAsync(newrecord); };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact(DisplayName = "Adding a Public Holiday with a date in the past should throw an ArgumentOutOfRangeException")]
        public async Task AddingAPastDateShouldThrowException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var publicHolidayComponent = new PublicHolidayComponent(context);
            var newrecord = new Models.PublicHolidayModel
            {
                PublicHolidayID = 1,
                Description = "New Year's Day",
                Date = new DateTime(2022, 01, 01)
            };

            //Act
            Func<Task> act = async () => { await publicHolidayComponent.AddPublicHolidayAsync(newrecord); };

            //Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
    }
}
