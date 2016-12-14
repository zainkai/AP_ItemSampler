using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Web.Controllers;
using System.Collections.Generic;
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
        List<ItemDigest> itemDigests;
        string[] mathSubjectList;
        string[] interactionCodeList;
        string[] claimList;

        public ItemsSearchControllerTests()
        {
            string subject = "MATH";
            mathSubjectList = new string[] { subject };
            string interactionTypeCode = "TC2";
            interactionCodeList = new string[] { interactionTypeCode };
            string claimCode = "1";
            claimList = new string[] { claimCode };

            itemDigests = new List<ItemDigest>() {
                new ItemDigest
                {
                    BankKey = goodBankKey,
                    ItemKey = goodItemKey,
                    Grade = GradeLevels.Grade6
                },
                new ItemDigest
                {
                    BankKey = goodBankKey,
                    ItemKey = badItemKey,
                    Grade = GradeLevels.High,
                    Subject = subject,
                    InteractionTypeCode = interactionTypeCode
                },
                new ItemDigest
                {
                    BankKey = badBankKey,
                    ItemKey = goodItemKey,
                    Grade = GradeLevels.Grade9
                },
                new ItemDigest
                {
                    BankKey = badBankKey,
                    ItemKey = badItemKey,
                    Grade = GradeLevels.Grade4
                }
            };

            var sampleItemsSearchRepoMock = new Mock<ISampleItemsSearchRepo>();

            sampleItemsSearchRepoMock.Setup(x => x.GetItemDigests()).Returns(itemDigests);
            sampleItemsSearchRepoMock.Setup(x => x.
                                        GetItemDigests(GradeLevels.High, mathSubjectList, interactionCodeList, claimList))
                                        .Returns(new List<ItemDigest> { itemDigests[1] });

            sampleItemsSearchRepoMock.Setup(x => x.
                                        GetItemDigests(GradeLevels.High, new string[] { "ELA" }, interactionCodeList, claimList))
                                        .Returns(new List<ItemDigest> { });

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            controller = new ItemsSearchController(sampleItemsSearchRepoMock.Object, loggerFactory.Object);
        }


        [Fact]
        public void TestSearchHappyCase()
        {
            var result = controller.Search(GradeLevels.High, mathSubjectList, interactionCodeList, claimList) as JsonResult;
            List<ItemDigest> resultList = result.Value as List<ItemDigest>;

            Assert.Equal(1, resultList.Count);
            Assert.Equal(goodBankKey, resultList[0].BankKey);
            Assert.Equal(badItemKey, resultList[0].ItemKey);
        }


        [Fact]
        public void TestSearchNoResult()
        {
            var result = controller.Search(GradeLevels.High, new string[] { "ELA" }, interactionCodeList, claimList) as JsonResult;
            List<ItemDigest> resultList = result.Value as List<ItemDigest>;

            Assert.Equal(0, resultList.Count);
        }

    }
    
}
