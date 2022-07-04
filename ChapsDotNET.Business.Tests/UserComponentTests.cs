using System;
using System.Threading.Tasks;
using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Models;
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

        [Fact(DisplayName = "When a null email address is sent to GetUserByName method, then it should throw Argument Null Exception")]
        public async Task WhenANullEmailAddressIsSentToGetUserByNameMethodThenItThrowsAnArgumentNullException()
        {
            //Arrange
            var context = DataContextFactory.Create();
            var userComponent = new UserComponent(context);

            //Act
            Func<Task> act = async () => { await userComponent.GetUserByNameAsync(null); };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact(DisplayName = "When an empty string as parameter is sent to GetUserByName method, then it should throw Argument Null Exception")]
        public async Task WhenAnEmptyEmailAddressIsSentToGetUserByNameMethodThenItThrowsAnArgumentNullException()
        {
            //Arrange
            var context = DataContextFactory.Create();
            var userComponent = new UserComponent(context);

            //Act
            Func<Task> act = async () => { await userComponent.GetUserByNameAsync(String.Empty); };
            Func<Task> act2 = async () => { await userComponent.GetUserByNameAsync("   "); };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
            await act2.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact(DisplayName = "When an email address is passed to GetUserByName method, then it returns a correctly mapped User Model")]
        public async Task WhenAnEmailAddressIsSentToGetUserByNameMethodThenItReturnACorrectlyMappedUserModel()
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
                Team = new Team
                {
                    Name = "TeamCharlie",
                    Acronym = "TC",
                    active = true
                }
            });
            await context.SaveChangesAsync();
            var userComponent = new UserComponent(context);

            //Act
            var result = await userComponent.GetUserByNameAsync("abc@justice.gov.uk");

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserModel>();
            result.Email.Should().Be("abc@justice.gov.uk");
            result.Name.Should().Be("abc@justice.gov.uk");
            result.DisplayName.Should().Be("Abc");
            result.RoleStrength.Should().Be(100);
            result.TeamAcronym.Should().Be("TC");


        }

        [Fact(DisplayName = "When an email address is passed to GetUserByName method, and the user doesn't exist then it should throw NotFoundException")]
        public async Task WhenAnEmailAddressIsPassedToGetUserByNameMethodAndTheUserDoesntExistThenThrowNotFoundException()
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
            Func<Task> act = async () => { await userComponent.GetUserByNameAsync("abc1@justice.gov.uk"); };

            //Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("User with abc1@justice.gov.uk not found.");
        }

    }
}
