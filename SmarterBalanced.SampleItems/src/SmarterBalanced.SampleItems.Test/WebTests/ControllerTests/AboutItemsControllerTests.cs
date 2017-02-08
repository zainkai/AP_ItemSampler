using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
        public class AboutItemsControllerTests : Controller
    {

        AboutItemsController aboutItemsController;

        public AboutItemsControllerTests()
        {
            var appSettings = new AppSettings()
            {
                SettingsConfig = new SettingsConfig()
            };
            var aboutItemsRepo = new Mock<IAboutItemsRepo>();

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            //aboutItemsController = new AboutItemsController(appSettings, loggerFactory.Object);
        }
}
