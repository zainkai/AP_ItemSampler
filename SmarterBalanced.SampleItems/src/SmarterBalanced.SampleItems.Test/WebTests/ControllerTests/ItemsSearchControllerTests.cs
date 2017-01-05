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

        public ItemsSearchControllerTests()
        {     
            var sampleItemsSearchRepoMock = new Mock<ISampleItemsSearchRepo>();
            sampleItemsSearchRepoMock.Setup(x => x.
                                        GetItemCards(It.IsAny<ItemsSearchParams>()))
                                        .Returns(new List<ItemCardViewModel>());
            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            controller = new ItemsSearchController(sampleItemsSearchRepoMock.Object, loggerFactory.Object);
        }


        [Fact]
        public void TestSearchHappyCase()
        {
            var result = controller.Search(null, GradeLevels.High, new string[] { "MATH" }, null, null);            
            //var resultList = result.Value as List<ItemCardViewModel>;
            //Assert.Equal(1, resultList.Count);
            //Assert.Equal(goodBankKey, resultList[0].BankKey);
            //Assert.Equal(badItemKey, resultList[0].ItemKey);
        }

        [Fact]
        public void TestSearchNoResult()
        {
            var result = controller.Search(null, GradeLevels.High, null, null, null) as JsonResult;
            var resultList = result.Value as List<ItemCardViewModel>;

            Assert.IsType<List<ItemCardViewModel>>(result.Value);
            Assert.IsType<JsonResult>(result);
            Assert.Equal(0, resultList.Count);
        }

        [Fact(Skip = "To Do")]
        public void TestIndex()
        {
            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<ItemsSearchViewModel>(viewResult);
        }
    }

}
