using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Web.Controllers;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;
using System.Linq;

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
    public class ItemsSearchControllerTests
    {
        ItemsSearchController controller;
        ItemsSearchController controllerBadReq;
        ItemsSearchViewModel itemsSearchViewModel;
        ItemsSearchViewModel itemsSearchViewModelBadReq;

        public ItemsSearchControllerTests()
        {
            var sampleItemsSearchRepoMock = new Mock<ISampleItemsSearchRepo>();
            var sampleItemsSearchRepoBadRequestMock = new Mock<ISampleItemsSearchRepo>();
            sampleItemsSearchRepoMock.Setup(x => x
                .GetItemCards(It.Is<ItemsSearchParams>(p => p.ItemId == "42")))
                .Returns(new List<ItemCardViewModel> { ItemCardViewModel.Create(itemKey: 42) });

            sampleItemsSearchRepoMock.Setup(x => x.GetItemsSearchViewModel()).Returns(itemsSearchViewModel);
            sampleItemsSearchRepoBadRequestMock.Setup(x => x.GetItemsSearchViewModel()).Returns(itemsSearchViewModelBadReq);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            controller = new ItemsSearchController(sampleItemsSearchRepoMock.Object, loggerFactory.Object);
            controllerBadReq = new ItemsSearchController(sampleItemsSearchRepoBadRequestMock.Object, loggerFactory.Object);
        }

        /// <summary>
        /// Tests that an empty ItemCardViewModel List is returned, given no parms.
        /// </summary>
        [Fact]
        public void TestSearchNoResult()
        {
            var result = controller.Search(null, GradeLevels.High, null, null, null) as JsonResult;
            var resultList = result.Value as List<ItemCardViewModel>;

            Assert.IsType<List<ItemCardViewModel>>(result.Value);
            Assert.IsType<JsonResult>(result);
            Assert.Equal(0, resultList.Count);
        }

        /// <summary>
        /// Tests that an ItemSearchViewModel is returned.
        /// </summary>
        [Fact]
        public void TestIndex()
        {
            var result = controller.Index();
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ItemsSearchViewModel>(viewResult.ViewData.Model);

            Assert.Equal(itemsSearchViewModel, model);
        }

        /// <summary>
        /// Tests that a BadRequestResult is returned by Index
        /// </summary>
        [Fact]
        public void TestIndexNull()
        {
            var result = controllerBadReq.Index();

            Assert.IsType<BadRequestResult>(result);
        }

        /// <summary>
        /// Tests that a BadRequestResult is returned by Index
        /// </summary>
        [Fact]
        public void TestMatchingKey()
        {
            var result = controller.Search("42", GradeLevels.High, null, null, null) as JsonResult;
            var cards = result.Value as List<ItemCardViewModel>;
            Assert.Equal(cards.Count, 1);
            Assert.Equal(cards[0].ItemKey, 42);
        }
    }
}
