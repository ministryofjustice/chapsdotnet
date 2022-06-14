using System.Threading.Tasks;
using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Tests.Common;
using ChapsDotNET.Data.Entities;
using FluentAssertions;
using Xunit;

namespace ChapsDotNET.Business.Tests
{
    public class UserComponentTests
    {
        [Fact(DisplayName = "If a User exists in the database then he is authorised")]
        public async Task UserIsAuthorisedWhenAUserExistsInDatabase()
        {
            //Arrange
            var context = DataContextFactory.Create();
            context.Users.Add(new User
            {
                Name = "abc@justice.gov.uk",
                DisplayName = "Abc",
                RoleStrength = 100,
                email = "abc@justice.gov.uk",
                Changeable = true,
            });
            await context.SaveChangesAsync();
            var userComponent = new UserComponent(context);

            //Act
            var result = await userComponent.IsUserAuthorisedAsync("abc@justice.gov.uk");

            //Assert
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "If a User exists in the database and his role strength is zero then he should not be authorised")]
        public async Task UserIsNotAuthorisedWhenAUserExistsInDatabaseAndRoleStrengthIsZero()
        {
            //Arrange
            var context = DataContextFactory.Create();
            context.Users.Add(new User
            {
                Name = "abc@justice.gov.uk",
                DisplayName = "Abc",
                RoleStrength = 0,
                email = "abc@justice.gov.uk",
                Changeable = true,
            });
            await context.SaveChangesAsync();
            var userComponent = new UserComponent(context);

            //Act
            var result = await userComponent.IsUserAuthorisedAsync("abc@justice.gov.uk");

            //Assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "If a User exists in the database and his name is null then he should not be authorised")]
        public async Task UserIsNotAuthorisedWhenAUserExistsInDatabaseAndNameIsNull()
        {
            //Arrange
            var context = DataContextFactory.Create();
            context.Users.Add(new User
            {
                Name = "",
                DisplayName = "Abc",
                RoleStrength = 100,
                email = "abc@justice.gov.uk",
                Changeable = true,
            });
            await context.SaveChangesAsync();
            var userComponent = new UserComponent(context);

            //Act
            var result = await userComponent.IsUserAuthorisedAsync("abc@justice.gov.uk");

            //Assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "If a User does not exist in the database then he should not be authorised")]
        public async Task UserIsNotAuthorisedWhenAUserDoesNotExistInDatabase()
        {
            //Arrange
            var context = DataContextFactory.Create();
            var userComponent = new UserComponent(context);

            //Act
            var result = await userComponent.IsUserAuthorisedAsync("abc@justice.gov.uk");

            //Assert
            result.Should().BeFalse();
        }

    }
}
