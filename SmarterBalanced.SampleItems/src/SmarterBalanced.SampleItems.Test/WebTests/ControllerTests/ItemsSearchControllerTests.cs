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
            itemsSearchViewModel = new ItemsSearchViewModel();
            itemsSearchViewModelBadReq = null;
            var sampleItemsSearchRepoMock = new Mock<ISampleItemsSearchRepo>();
            var sampleItemsSearchRepoBadRequestMock = new Mock<ISampleItemsSearchRepo>();
            sampleItemsSearchRepoMock.Setup(x => x.
                                        GetItemCards(It.IsAny<ItemsSearchParams>()))
                                        .Returns(new List<ItemCardViewModel>());

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
    }
}
