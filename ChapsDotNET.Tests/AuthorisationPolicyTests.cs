using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Policies.Handlers;
using ChapsDotNET.Policies.Requirements;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ChapsDotNET.Tests
{
    public class AuthorisationPolicyTests
    {
        [Fact(DisplayName = "If a User is Not Authorised, Then the Policy Handler Should Return Has Succeeded as false")]
        public async Task IfTheUserIsNotAuthorisedThenThePolicyHandlerShouldReturnHasSucceededFalse()
        {
            // Arrange
            var mockUserComponent = Substitute.For<IUserComponent>();
            mockUserComponent.IsUserAuthorisedAsync(Arg.Any<string>()).Returns(false);

            var requirements = new[] { new IsAuthorisedUserRequirement() };
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[] { new Claim("permission", "100") }));

            // Act
            var context = new AuthorizationHandlerContext(requirements, user, null);
            var handler = new IsAuthorisedUserHandler(mockUserComponent);
            await handler.HandleAsync(context);
            // Assert
            context.HasSucceeded.Should().BeFalse();

        }

        [Fact(DisplayName = "If a User is Authorised, Then the Policy Handler Should Return Has Succeeded as true")]
        public async Task IfTheUserIsAuthorisedThenThePolicyHandlerShouldReturnHasSucceededTrue()
        {
            // Arrange
            var mockUserComponent = Substitute.For<IUserComponent>();
            mockUserComponent.IsUserAuthorisedAsync(Arg.Any<string>()).Returns(true);

            var requirements = new[] { new IsAuthorisedUserRequirement() };
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[] { new Claim("permission", "100") }));

            // Act
            var context = new AuthorizationHandlerContext(requirements, user, null);
            var handler = new IsAuthorisedUserHandler(mockUserComponent);
            await handler.HandleAsync(context);
            // Assert
            context.HasSucceeded.Should().BeTrue();

        }

        private IAuthorizationService BuildAuthorisationService(
            Action<IServiceCollection>? setupServices = null)
        {
            var services = new ServiceCollection();
            services.AddAuthorization();
            services.AddLogging();
            services.AddOptions();
            setupServices?.Invoke(services);
            return services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();
        }
    }
}