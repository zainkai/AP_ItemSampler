using Moq;
using SmarterBalanced.SampleItems.Core.Infrastructure;
using SmarterBalanced.SampleItems.Dal.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Test.CoreTests
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
                    Grade = "High School"
                },
                new ItemDigest
                {
                    BankKey = badBankKey,
                    ItemKey = goodItemKey,
                    Grade = "9"
                },
                new ItemDigest
                {
                    BankKey = badBankKey,
                    ItemKey = badItemKey,
                    Grade = "4"
                }
            };

            goodItemDigest = new ItemDigest
            {
                BankKey = goodBankKey,
                ItemKey = goodItemKey,
                Grade = "6"
            };

            itemDigests.Add(goodItemDigest);

            var sampleItemsRepoMock = new Mock<ISampleItemsRepo>();
            sampleItemsRepoMock.Setup(x => x.GetItemDigest(goodBankKey, goodItemKey)).Returns(goodItemDigest);
            sampleItemsSearchRepo = new SampleItemsSearchRepo(sampleItemsRepoMock.Object);
        }

    }
}
