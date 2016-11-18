using Xunit;
using Moq;
using SmarterBalanced.SampleItems.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
    public class ItemControllerTests
    {
        ItemController controller;
        ItemDigest itemDigest;
        int bankKey;
        int itemKey;
        string iSAAP;

        public ItemControllerTests()
        {
            bankKey = 234343;
            itemKey = 485954;
            itemDigest = new ItemDigest
            {
                BankKey = bankKey,
                ItemKey = itemKey,
                Grade = GradeLevels.Grade6
            };

            iSAAP = "TDS_test;TDS_test2;";

            var itemViewRepoMock = new Mock<IItemViewRepo>();

            var itemViewModel = new ItemViewModel()
            {
                ItemDigest = itemDigest,
                ItemViewerServiceUrl = $"http://itemviewerservice.cass.oregonstate.edu/item/{bankKey}-{itemKey}"
            };
            itemViewRepoMock.Setup(x => x.GetItemViewModelAsync(bankKey, itemKey)).ReturnsAsync(itemViewModel);
            itemViewRepoMock.Setup(x => x.GetItemViewModelAsync(bankKey, itemKey, iSAAP)).ReturnsAsync(itemViewModel);

            controller = new ItemController(itemViewRepoMock.Object);
        }

        /// <summary>
        /// Tests that an ItemViewModel is returned given a vaid id.
        /// </summary>
        [Fact]
        public async void TestDetailsSuccess()
        {
            var result = await controller.Details(bankKey, itemKey, iSAAP);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ItemViewModel>(viewResult.ViewData.Model);
        }

        /// <summary>
        /// Tests that a BadRequestResult is returned given a null key
        /// </summary>
        [Fact]
        public async void TestDetailsNullParam()
        {
            var result = await controller.Details(null, itemKey, iSAAP);

            Assert.IsType<BadRequestResult>(result);
        }

        /// <summary>
        /// Tests that a BadRequestResult is returned given a nonexistent key
        /// </summary>
        [Fact]
        public async void TestDetailsBadId()
        {
            var result = await controller.Details(bankKey + 1, itemKey + 1, iSAAP);

            Assert.IsType<BadRequestResult>(result);
        }

    }
}
