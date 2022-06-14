using ChapsDotNET.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace ChapsDotNET.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void WhenHomePageIsCalledWeShouldBeReturnedWithIndexView()
        {
            var mockLogger = Substitute.For<ILogger<HomeController>>();
            var controller = new HomeController(mockLogger);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<ViewResult>();
            result?.ViewName.Should().Be("Index");
        }
    }
}
