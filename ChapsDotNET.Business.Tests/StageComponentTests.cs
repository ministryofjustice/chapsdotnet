
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
            var result = await StageComponent.GetStageAsync(11);

            // Assert
            result.Should().NotBeNull();

            result.Name.Should().Be("Closed");
            result.StageID.Should().Be(11);


        }

        [Fact(DisplayName = "What happens for the wrong Stage id?")]
        public async Task GetWrongStageAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.Stages.AddAsync(new Stage
            {
                StageID = 2,
                Name = "Await"

            });

            await context.Stages.AddAsync(new Stage
            {
                StageID = 54,
                Name = "Closed"
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
