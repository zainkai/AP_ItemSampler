using Microsoft.Extensions.Logging;
using Moq;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Test.CoreTests.ReposTests
{
    public class SampleItemsSearchRepoTests
    {
        SampleItemsSearchRepo sampleItemsSearchRepo;
        List<ItemDigest> itemDigests;
        ItemDigest goodItemDigest;
        int goodItemKey;
        int goodBankKey;
        int badItemKey;
        int badBankKey;

        public SampleItemsSearchRepoTests()
        {
            goodItemKey = 2343;
            goodBankKey = 8398;
            badItemKey = 9234;
            badBankKey = 1123;
            itemDigests = new List<ItemDigest>() {
                new ItemDigest
                {
                    BankKey = goodBankKey,
                    ItemKey = badItemKey,
                    Grade = GradeLevels.High
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

            goodItemDigest = new ItemDigest
            {
                BankKey = goodBankKey,
                ItemKey = goodItemKey,
                Grade = GradeLevels.Grade6
            };

            itemDigests.Add(goodItemDigest);

            var sampleContextMock = new Mock<SampleItemsContext>();
            sampleContextMock.Setup(x => x.ItemDigests).Returns(itemDigests);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            sampleItemsSearchRepo = new SampleItemsSearchRepo(sampleContextMock.Object, loggerFactory.Object);
        }

    }
}
