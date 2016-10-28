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
        string ISSAP;

        public ItemControllerTests()
        {
            bankKey = 234343;
            itemKey = 485954;
            itemDigest = new ItemDigest
            {
                BankKey = bankKey,
                ItemKey = itemKey,
                Grade = "6"
            };

            ISSAP = "TDS_test;TDS_test2;";

            var itemViewRepoMock = new Mock<IItemViewRepo>();
            itemViewRepoMock.Setup(x => x.GetItemDigest(bankKey, itemKey)).Returns(itemDigest);

            var itemViewModel = new ItemViewModel()
            {
                ItemDigest = itemDigest,
                ItemViewerServiceUrl = $"http://itemviewerservice.cass.oregonstate.edu/item/{bankKey}-{itemKey}"
            };
            itemViewRepoMock.Setup(x => x.GetItemViewModel(bankKey, itemKey)).Returns(itemViewModel);
            itemViewRepoMock.Setup(x => x.GetItemViewModel(bankKey, itemKey, ISSAP)).Returns(itemViewModel);

            controller = new ItemController(itemViewRepoMock.Object);
        }

        /// <summary>
        /// Tests that an ItemViewModel is returned given a vaid id.
        /// </summary>
        [Fact]
        public void TestDetailsSuccess()
        {
            var result = controller.Details(bankKey, itemKey, ISSAP);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ItemViewModel>(viewResult.ViewData.Model);
        }

        /// <summary>
        /// Tests that a BadRequestResult is returned given a null key
        /// </summary>
        [Fact]
        public void TestDetailsNullParam()
        {
            var result = controller.Details(null, itemKey, ISSAP);

            Assert.IsType<BadRequestResult>(result);
        }

        /// <summary>
        /// Tests that a BadRequestResult is returned given a nonexistent key
        /// </summary>
        [Fact]
        public void TestDetailsBadId()
        {
            var result = controller.Details(bankKey + 1, itemKey + 1, ISSAP);

            Assert.IsType<BadRequestResult>(result);
        }

    }
}
