using Xunit;
using Moq;
using SmarterBalanced.SampleItems.Web.Controllers;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
    public class HomeControllerTests
    {

        HomeController HomeController;

        public HomeControllerTests()
        {
            var appSettings = new AppSettings()
            {
                SettingsConfig = new SettingsConfig()
            };

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            HomeController = new HomeController(appSettings, loggerFactory.Object);
        }

        [Fact (Skip ="TODO: Test each of the status codes")]
        public void StatusCodeTest()
        {

        }


    }
}
