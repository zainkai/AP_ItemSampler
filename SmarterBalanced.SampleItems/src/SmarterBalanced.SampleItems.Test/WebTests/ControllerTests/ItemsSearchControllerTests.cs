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
        ItemsSearchViewModel itemsSearchViewModel;
        int goodBankKey = 99;
        int badBankKey = 1;
        int goodItemKey = 89;
        int badItemKey = 2;
        List<ItemCardViewModel> itemCards;
        string[] mathSubjectList;
        string[] interactionCodeList;
        string[] claimList;

        public ItemsSearchControllerTests()
        {
            string subjectCode = "MATH";
            mathSubjectList = new string[] { subjectCode };
            string interactionTypeCode = "TC2";
            interactionCodeList = new string[] { interactionTypeCode };
            string claimCode = "1";
            claimList = new string[] { claimCode };
            itemsSearchViewModel = new ItemsSearchViewModel();

            itemCards = new List<ItemCardViewModel>() {
                new ItemCardViewModel(bankKey: goodBankKey, itemKey: goodItemKey, title: "", grade: GradeLevels.Grade6, gradeLabel: "",
                subjectCode: "", subjectLabel: "", claimCode: "", claimLabel: "",
                    target: "", interactionTypeCode: "", interactionTypeLabel: ""),
                new ItemCardViewModel(bankKey: goodBankKey, itemKey: badItemKey, title: "", grade: GradeLevels.High, gradeLabel: "",
                subjectCode: subjectCode, subjectLabel: "", claimCode: "", claimLabel: "",
                    target: "", interactionTypeCode: interactionTypeCode, interactionTypeLabel: ""),
                new ItemCardViewModel(bankKey: badBankKey, itemKey: goodItemKey, title: "", grade: GradeLevels.Grade9, gradeLabel: "",
                subjectCode: subjectCode, subjectLabel: "", claimCode: "", claimLabel: "",
                    target: "", interactionTypeCode: interactionTypeCode, interactionTypeLabel: ""),
                new ItemCardViewModel(bankKey: badBankKey, itemKey: badItemKey, title: "", grade: GradeLevels.Grade4, gradeLabel: "",
                subjectCode: subjectCode, subjectLabel: "", claimCode: "", claimLabel: "",
                    target: "", interactionTypeCode: interactionTypeCode, interactionTypeLabel: ""),
            };

            var sampleItemsSearchRepoMock = new Mock<ISampleItemsSearchRepo>();
            sampleItemsSearchRepoMock.Setup(x => x.
                                        GetItemCards(It.IsAny<ItemsSearchParams>()))
                                        .Returns(new List<ItemCardViewModel>());

            sampleItemsSearchRepoMock.Setup(x => x.
                            GetItemCards(new ItemsSearchParams(It.IsAny<string>(), GradeLevels.High, mathSubjectList, interactionCodeList, claimList)))
                            .Returns(new List<ItemCardViewModel> { itemCards[1] });

            sampleItemsSearchRepoMock.Setup(x => x.
                            GetItemCards(new ItemsSearchParams(null, GradeLevels.All, It.IsAny<string[]>(), It.IsAny<string[]>(), It.IsAny<string[]>())))
                            .Returns(new List<ItemCardViewModel>(itemCards));
            sampleItemsSearchRepoMock.Setup(x => x.GetItemsSearchViewModel()).Returns(itemsSearchViewModel);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            controller = new ItemsSearchController(sampleItemsSearchRepoMock.Object, loggerFactory.Object);
        }

        [Fact(Skip ="To Do")]
        public void TestSearchNoParams()
        {
            var result = controller.Search(null, GradeLevels.All, new string[] { }, new string[] { }, new string[] { }) as JsonResult;
            var resultList = result.Value as List<ItemCardViewModel>;

            Assert.Equal(4, resultList.Count);
            Assert.Equal(goodBankKey, resultList[0].BankKey);
            Assert.Equal(badItemKey, resultList[0].ItemKey);
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

        [Fact]
        public void TestIndex()
        {
            var result = controller.Index();
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ItemsSearchViewModel>(viewResult.ViewData.Model);

            Assert.Equal(itemsSearchViewModel, model);
        }
    }

}
