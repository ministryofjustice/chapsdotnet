using System;
using ChapsDotNET.Controllers;
using ChapsDotNET.Policies.Requirements;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ChapsDotNET.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var authorizationService = BuildAuthorisationService(services =>
            {
                services.AddAuthorization(options =>
                {
                    // By default, all incoming requests will be authorized according to the default policy.
                    options.FallbackPolicy = options.DefaultPolicy;
                    options.AddPolicy("IsAuthorisedUser", isAuthorizedUserPolicy =>
                    {
                        isAuthorizedUserPolicy.Requirements.Add(new IsAuthorisedUserRequirement());
                    });
                });
            });

            var mockLogger = Substitute.For<ILogger<HomeController>>();
            var controller = new HomeController(mockLogger);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            result?.ViewName.Should().Be("Index");
        }

        private IAuthorizationService BuildAuthorisationService(
            Action<IServiceCollection> setupServices = null)
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