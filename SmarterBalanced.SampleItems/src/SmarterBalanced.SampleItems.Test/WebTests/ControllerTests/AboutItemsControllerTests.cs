using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
    public class AboutItemsControllerTests : Controller
    {
        AboutItemsController aboutItemsController;
        AboutItemsViewModel aboutItemsViewModel;
        InteractionType ItMath;

        public AboutItemsControllerTests()
        {
            ItMath = new InteractionType("2", "Math Itype", "", 2);
            var interactionTypes = ImmutableArray.Create(ItMath);
            aboutItemsViewModel = new AboutItemsViewModel(interactionTypes, "");
            var appSettings = new AppSettings()
            {
                SettingsConfig = new SettingsConfig()
            };
            var aboutItemsRepoMock = new Mock<IAboutItemsRepo>();
            aboutItemsRepoMock.Setup(x => x.GetAboutItemsViewModel()).Returns(aboutItemsViewModel);
            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            aboutItemsController = new AboutItemsController(aboutItemsRepoMock.Object, appSettings, loggerFactory.Object);
        }

        [Fact]
        public void TestIndex()
        {
            var result = aboutItemsController.Index();
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AboutItemsViewModel>(viewResult.ViewData.Model);

            Assert.Equal(model, aboutItemsViewModel);
        }

        [Fact(Skip = "ToDo")]
        public void TestIndexNull()
        {

        }
    }
}
