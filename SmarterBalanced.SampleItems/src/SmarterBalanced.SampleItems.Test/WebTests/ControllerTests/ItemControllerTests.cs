using Xunit;
using Moq;
using SmarterBalanced.SampleItems.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
    public class ItemControllerTests
    {
        ItemController controller;
        ItemViewModel itemViewModel;
        ItemViewModel itemViewModelCookie;
        int bankKey;
        int itemKey;
        string iSAAP;

        public ItemControllerTests()
        {
            bankKey = 234343;
            itemKey = 485954;

            var itemDigest = new ItemDigest
            {
                BankKey = bankKey,
                ItemKey = itemKey,
                Grade = GradeLevels.Grade6
            };

            iSAAP = "TDS_test;TDS_test2;";

            string accCookieName = "accessibilitycookie";

            var accessibilityResourceViewModels = new List<AccessibilityResourceViewModel>();

            var appSettings = new AppSettings()
            {
                SettingsConfig = new SettingsConfig()
                {
                    AccessibilityCookie = accCookieName
                }
            };

            itemViewModel = new ItemViewModel()
            {
                ItemDigest = itemDigest,
                ItemViewerServiceUrl = $"http://itemviewerservice.cass.oregonstate.edu/item/{bankKey}-{itemKey}",
                AccResourceVMs = accessibilityResourceViewModels
            };

            itemViewModelCookie = new ItemViewModel()
            {
                AccResourceVMs = accessibilityResourceViewModels
            };
            var itemViewRepoMock = new Mock<IItemViewRepo>();
          
            itemViewRepoMock.Setup(repo => repo.GetItemViewModel(bankKey, itemKey, string.Empty, null)).Returns(itemViewModel);
            itemViewRepoMock.Setup(repo => repo.GetItemViewModel(bankKey, itemKey, iSAAP, null)).Returns(itemViewModel);
            itemViewRepoMock.Setup(repo => repo.GetItemViewModel(bankKey, itemKey, null, null)).Returns(itemViewModelCookie);
            itemViewRepoMock.Setup(repo => repo.AppSettings).Returns(appSettings);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            controller = new ItemController(itemViewRepoMock.Object, loggerFactory.Object);
        }

        /// <summary>
        /// Tests that an ItemViewModel is returned given a vaid id.
        /// </summary>
        [Fact]
        public void TestDetailsSuccess()
        {
            var result = controller.Details(bankKey, itemKey, iSAAP);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ItemViewModel>(viewResult.ViewData.Model);

            Assert.Equal(itemViewModel, model);

        }

        /// <summary>
        /// Tests that a BadRequestResult is returned given a null key
        /// </summary>
        [Fact]
        public void TestDetailsNullParam()
        {
            var result = controller.Details(null, itemKey, iSAAP);

            Assert.IsType<BadRequestResult>(result);
        }

        /// <summary>
        /// Tests that a BadRequestResult is returned given a nonexistent key
        /// </summary>
        [Fact]
        public void TestDetailsBadId()
        {
            var result = controller.Details(bankKey + 1, itemKey + 1, iSAAP);

            Assert.IsType<BadRequestResult>(result);
        }

        /// <summary>
        /// Tests that a cookie ISSAP is returned instead of param
        /// </summary>
        [Fact]
        public void TestDetailsNoISAAP()
        {
            var result = controller.Details(bankKey, itemKey, string.Empty);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ItemViewModel>(viewResult.ViewData.Model);

            Assert.Equal(itemViewModel, model);
        }
    }
}
