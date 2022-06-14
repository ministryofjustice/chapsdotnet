using ChapsDotNET.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ChapsDotNET.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var mockLogger = Substitute.For<ILogger<HomeController>>();
            var controller = new HomeController(mockLogger);

            // Act
            var result = (ViewResult)controller.Index();

            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("Index");
        }
    }
}