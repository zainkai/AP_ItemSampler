﻿using Microsoft.AspNetCore.Mvc;
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
            aboutItemsViewModel = new AboutItemsViewModel(interactionTypes, "", "", null);
            var appSettings = new AppSettings()
            {
                SettingsConfig = new SettingsConfig()
            };
            var aboutItemsRepoMock = new Mock<IAboutItemsRepo>();
            aboutItemsRepoMock.Setup(x => x.GetAboutItemsViewModel("")).Returns(aboutItemsViewModel);
            aboutItemsRepoMock.Setup(x => x.GetAboutItemsViewModel("bad"));
            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            aboutItemsController = new AboutItemsController(aboutItemsRepoMock.Object, appSettings, loggerFactory.Object);
        }

        // TODO: Add more tests

        [Fact]
        public void TestIndex()
        {
            var result = aboutItemsController.Index();
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AboutItemsViewModel>(viewResult.ViewData.Model);

            Assert.Equal(model, aboutItemsViewModel);
        }

        [Fact]
        public void TestGetItemViewerUrl()
        {
            var result = aboutItemsController.GetItemUrl("bad");
            JsonResult resJson = Assert.IsType<JsonResult>(result);

            Assert.Equal(null, resJson.Value);
        }
    }
}