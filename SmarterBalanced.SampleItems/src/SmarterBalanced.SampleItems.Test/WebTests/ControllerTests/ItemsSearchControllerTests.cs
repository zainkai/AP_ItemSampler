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

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
    public class ItemsSearchControllerTests
    {
        ItemsSearchController controller;
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

            sampleItemsSearchRepoMock.Setup(x => x.GetItemCards()).Returns(itemCards);
            sampleItemsSearchRepoMock.Setup(x => x.
                                        GetItemCards(new ItemsSearchParams(It.IsAny<string>(), GradeLevels.High, mathSubjectList, interactionCodeList, claimList)))
                                        .Returns(new List<ItemCardViewModel> { itemCards[1] });

            sampleItemsSearchRepoMock.Setup(x => x.
                                        GetItemCards(new ItemsSearchParams(It.IsAny<string>(), GradeLevels.High, new string[] { "ELA" }, interactionCodeList, claimList)))
                                        .Returns(new List<ItemCardViewModel> { });

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            controller = new ItemsSearchController(sampleItemsSearchRepoMock.Object, loggerFactory.Object);
        }


        [Fact (Skip ="TODO: Fixme")]
        public void TestSearchHappyCase()
        {
            var result = controller.Search("", GradeLevels.High, mathSubjectList, interactionCodeList, claimList) as JsonResult;
            var resultList = result.Value as List<ItemCardViewModel>;

            Assert.Equal(1, resultList.Count);
            Assert.Equal(goodBankKey, resultList[0].BankKey);
            Assert.Equal(badItemKey, resultList[0].ItemKey);
        }


        [Fact(Skip = "TODO: Fixme")]
        public void TestSearchNoResult()
        {
            var result = controller.Search("", GradeLevels.High, new string[] { "ELA" }, interactionCodeList, claimList) as JsonResult;
            var resultList = result.Value as List<ItemCardViewModel>;

            Assert.Equal(0, resultList.Count);
        }

    }

}
