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
    public class LeadSubjectsComponentTests
    {
        [Fact(DisplayName = "Get a list of salutations when GetSalutationsAsync is called")]
        public async Task GetAListOfLeadSubjectsWhenGetLeadSubjectsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 1,
                Detail = "Brexit",
                active = true
            });
            await context.SaveChangesAsync();

            var leadSubjectComponent = new LeadSubjectComponent(context);

            // Act
            var result = await leadSubjectComponent.GetLeadSubjectsAsync(new LeadSubjectRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            result.Results?.First().Detail.Should().Be("Brexit");
            result.Results?.First().LeadSubjectId.Should().Be(1);
            result.Results?.First().Active.Should().BeTrue();
        }
        //  LeadSubject
        [Fact(DisplayName = "Get a list of only active lead subjects when GetLeadSubjectsAsync is called without true in the parameter")]
        public async Task GetAListOfOnlyActiveLeadSubjectsWhenGetLeadSubjectsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 1,
                Detail = "Courts",
                active = true
            });
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 2,
                Detail = "Brexit",
                active = true
            });
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 3,
                Detail = "Costs",
                active = false
            });
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 4,
                Detail = "Correspondence",
                active = true
            });

            await context.SaveChangesAsync();

            var leadSubjectComponent = new LeadSubjectComponent(context);

            // Act
            var result = await leadSubjectComponent.GetLeadSubjectsAsync(new LeadSubjectRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(3);
            result.Results?.All(x => x.Active).Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of only all lead subjects when GetLeadSubjectsAsync is called with true ShowActiveInactive in the parameter")]
        public async Task GetAListOfActiveAndInactiveLeadSubjectsWhenGetLeadSubjectsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 1,
                Detail = "Courts",
                active = true
            });
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 2,
                Detail = "Brexit",
                active = true
            });
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 3,
                Detail = "Costs",
                active = false
            });
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 4,
                Detail = "Correspondence",
                active = true
            });

            await context.SaveChangesAsync();

            var salutationComponent = new LeadSubjectComponent(context);

            // Act
            var result = await salutationComponent.GetLeadSubjectsAsync(new LeadSubjectRequestModel
            {
                ShowActiveAndInactive = true
            });

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(4);
        }

        [Fact(DisplayName = "When we call GetLeadSubjectsAsync we get paged results by default")]
        public async Task GetLeadSubjectsAsyncReturnsPagedResults()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var fakeLeadSubjects = FakeLeadSubjectsProvider.FakeData
                .Generate(50)
                .ToList();
            await context.LeadSubjects.AddRangeAsync(fakeLeadSubjects);
            await context.SaveChangesAsync();
            var leadSubjectComponent = new LeadSubjectComponent(context);

            // Act
            var result = await leadSubjectComponent.GetLeadSubjectsAsync(new LeadSubjectRequestModel
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


        [Fact(DisplayName = "Get a specific lead subject")]
        public async Task GetLeadSubjectAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 2,
                Detail = "Brexit",
                active = true
            });

            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 53,
                Detail = "Courts",
                active = true
            });

            await context.SaveChangesAsync();

            var leadSubjectComponent = new LeadSubjectComponent(context);

            // Act
            var result = await leadSubjectComponent.GetLeadSubjectAsync(53);

            // Assert
            result.Should().NotBeNull();

            result.Detail.Should().Be("Courts");
            result.LeadSubjectId.Should().Be(53);
            result.Active.Should().BeTrue();


        }

        [Fact(DisplayName = "What happens for the wrong LeadSubjectId?")]
        public async Task GetWrongLeadSubjectAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 2,
                Detail = "Brexit",
                active = true
            });

            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 54,
                Detail = "Courts",
                active = true
            });

            await context.SaveChangesAsync();

            var leadSubjectComponent = new LeadSubjectComponent(context);

            // Act
            var result = await leadSubjectComponent.GetLeadSubjectAsync(53);

            // Assert
            result.Detail.Should().BeNull();
        }


        [Fact(DisplayName = "Add a Lead Subject to the database when calling the AddLeadSubject method returns correct LeadSubjectId")]
        public async Task AddALeadSubjectToTheDatabase()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var leadSubjectComponent = new LeadSubjectComponent(context);
            var newrecord = new Models.LeadSubjectModel
            {
                LeadSubjectId = 1,
                Detail = "Brexit",
                Active = true
            };

            // Act
            var result = await leadSubjectComponent.AddLeadSubjectAsync(newrecord);


            // Assert
            result.Should().NotBe(0);
            context.LeadSubjects.First().Detail.Should().Be("Brexit");
            context.LeadSubjects.First().active.Should().Be(true);

        }

        [Fact(DisplayName = "Adding a LeadSubject with empty detail should throw an ArgumentNullException")]
        public async Task AddingAnEmptyDetailShouldThrowException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            var leadSubjectComponent = new LeadSubjectComponent(context);
            var newrecord = new Models.LeadSubjectModel
            {
                LeadSubjectId = 1,
                Detail = "",
                Active = true
            };

            //Act
            Func<Task> act = async () => { await leadSubjectComponent.AddLeadSubjectAsync(newrecord); };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }


        [Fact(DisplayName = "Updating active status on a Lead Subject from the database when calling the UpdateLeadSubjectActiveStatus")]
        public async Task UpdateActiveStatusOnALeadSubject()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 1,
                Detail = "Brexit",
                active = true
            });

            await context.SaveChangesAsync();

            var leadSubjectComponent = new LeadSubjectComponent(context);

            // Act
            await leadSubjectComponent.UpdateLeadSubjectAsync(new Models.LeadSubjectModel
            {
                LeadSubjectId = 1,
                Active = false,
                Detail = "Courts"
            });

            // Assert

            context.LeadSubjects.First().Detail.Should().Be("Courts");
            context.LeadSubjects.First().active.Should().BeFalse();
        }

        [Fact(DisplayName = "Updating active status on a Lead Subject when calling the UpdateLeadSubjectAsync with no Id")]
        public async Task UpdateActiveStatusOnALeadSubjectWithNoIdShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 1,
                Detail = "Brexit",
                active = true
            });

            await context.SaveChangesAsync();

            var leadSubjectComponent = new LeadSubjectComponent(context);

            // Act

            Func<Task> act = async () => { await leadSubjectComponent.UpdateLeadSubjectAsync(new Models.LeadSubjectModel()); };

            //Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact(DisplayName = "Updating Lead Subject with Empty Detail should throw an ArgumentNullException")]
        public async Task UpdateLeadSubjectWithNoDetailShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.LeadSubjects.AddAsync(new LeadSubject
            {
                LeadSubjectId = 1,
                Detail = "Brexit",
                active = true
            });

            await context.SaveChangesAsync();

            var leadSubjectComponent = new LeadSubjectComponent(context);

            // Act

            Func<Task> act = async () =>
            {
                await leadSubjectComponent.UpdateLeadSubjectAsync(new Models.LeadSubjectModel
                {
                    Detail = string.Empty,
                    LeadSubjectId = 1,
                    Active = true
                });
            };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

    }
}


