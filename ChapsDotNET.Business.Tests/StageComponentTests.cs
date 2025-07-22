
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
    public class StageComponentTests

    {
        [Fact(DisplayName = "Get a list of Stages when GetStagesAsync is called")]
        public async Task GetAListOfStagesWhenGetStagesAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Stages.AddAsync(new Stage
            {
                StageID = 1,
                Name = "Awaiting Allocation"
            });
            await context.SaveChangesAsync();

            var StageComponent = new StageComponent(context);

            // Act
            var result = await StageComponent.GetStagesAsync(new StageRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            result.Results?.First().Name.Should().Be("Awating Allocation");
            result.Results?.First().StageId.Should().Be(1);
            
        }





        [Fact(DisplayName = "Get a specific Stage")]
        public async Task GetStageAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Stages.AddAsync(new Stage
            {
                StageID = 2,
                Name = "Awaiting Allocation"
            });

            await context.Stages.AddAsync(new Stage
            {
                StageID = 11,
                Name = "Closed"
            });

            await context.SaveChangesAsync();

            var StageComponent = new StageComponent(context);

            // Act
            var result = await StageComponent.GetStageAsync(53);

            // Assert
            result.Should().NotBeNull();

            result.Name.Should().Be("Closed");
            result.StageId.Should().Be(11);
            result.Active.Should().BeTrue();


        }

        [Fact(DisplayName = "What happens for the wrong Stage id?")]
        public async Task GetWrongStageAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Stages.AddAsync(new Stage
            {
                StageID = 2,
                Detail = "Mrs",
                active = true
            });

            await context.Stages.AddAsync(new Stage
            {
                StageID = 54,
                Detail = "Ms",
                active = true
            });

            await context.SaveChangesAsync();

            var StageComponent = new StageComponent(context);

            // Act
            Func<Task> act = async () => { await StageComponent.GetStageAsync(53); };

            //Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }


    }
}
