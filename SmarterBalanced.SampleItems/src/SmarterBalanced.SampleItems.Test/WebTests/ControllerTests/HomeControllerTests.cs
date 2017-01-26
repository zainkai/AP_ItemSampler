using Xunit;
using Moq;
using SmarterBalanced.SampleItems.Web.Controllers;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
    public class HomeControllerTests
    {

        HomeController HomeController;
        string badReq;
        string notFound;
        string forbidden;
        string defaultResponse;

        public HomeControllerTests()
        {
            badReq = "400 Bad Request";
            notFound = "404 Not Found";
            forbidden = "403 Forbidden";
            defaultResponse = "Something went wrong";

            var appSettings = new AppSettings()
            {
                SettingsConfig = new SettingsConfig()
            };
            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            HomeController = new HomeController(appSettings, loggerFactory.Object);
        }

        [Fact]
        public void StatusCodeBadReqTest()
        {
            var result = HomeController.StatusCodeError(400);
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(badReq, viewResult.ViewData["Error"]);
        }

        [Fact]
        public void StatusCodeNotFoundTest()
        {
            var result = HomeController.StatusCodeError(404);
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(notFound, viewResult.ViewData["Error"]);
        }

        [Fact]
        public void StatusCodeForbiddenTest()
        {
            var result = HomeController.StatusCodeError(403);
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(forbidden, viewResult.ViewData["Error"]);
        }

        [Fact]
        public void StatusCodeTest()
        {
            var result = HomeController.StatusCodeError(1);
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(defaultResponse, viewResult.ViewData["Error"]);
        }

        [Fact]
        public void ErrorTest()
        {
            var result = HomeController.Error();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
