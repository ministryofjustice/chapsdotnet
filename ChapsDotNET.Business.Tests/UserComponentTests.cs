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
        public void UserIsAuthorisedWhenAUserExistsInDatabase()
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
            context.SaveChanges();
            var userComponent = new UserComponent(context);

            //Act
            var result = userComponent.IsUserAuthorised("abc@justice.gov.uk");

            //Assert
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "If a User exists in the database and his role strength is zero then he should not be authorised")]
        public void UserIsNotAuthorisedWhenAUserExistsInDatabaseAndRoleStrengthIsZero()
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
            context.SaveChanges();
            var userComponent = new UserComponent(context);

            //Act
            var result = userComponent.IsUserAuthorised("abc@justice.gov.uk");

            //Assert
            result.Should().BeFalse();
        }
    }
}
