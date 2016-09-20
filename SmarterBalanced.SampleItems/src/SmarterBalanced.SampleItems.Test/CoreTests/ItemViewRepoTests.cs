using Moq;
using SmarterBalanced.SampleItems.Core.Infrastructure;
using SmarterBalanced.SampleItems.Dal.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.CoreTests
{

    public class ItemViewRepoTests
    {
        ItemViewRepo itemViewRepo;
        ItemDigest itemDigest;
        int goodItemKey;
        int goodBankKey;
        int badItemKey;
        int badBankKey;

        public ItemViewRepoTests() {
            goodItemKey = 2343;
            goodBankKey = 8398;
            badItemKey = 9234;
            badBankKey = 1123;
            itemDigest = new ItemDigest {
                    BankKey = goodBankKey,
                    ItemKey = goodItemKey,
                    Grade = "6"
                };

            var sampleItemsRepoMock = new Mock<ISampleItemsRepo>();
            sampleItemsRepoMock.Setup(x => x.GetItemDigest(goodBankKey, goodItemKey)).Returns(itemDigest);
            itemViewRepo = new ItemViewRepo(sampleItemsRepoMock.Object);
        }

        /// <summary>
        /// Tests that the correct item is returned from GetItemDigest()
        /// given the correct itemKey and bankKey
        /// </summary>
        [Fact]
        public void TestGetItemDigestValidKeys()
        {
            var item = itemViewRepo.GetItemDigest(goodBankKey, goodItemKey);
            Assert.Equal(itemDigest, item);
        }

        /// <summary>
        /// Tests that GetItemDigest() returns null given a bad item key
        /// </summary>
        [Fact]
        public void TestGetItemDigestBadItemKey()
        {
            var item = itemViewRepo.GetItemDigest(goodBankKey, badItemKey);
            Assert.Null(item);
        }

        /// <summary>
        /// Tests that GetItemDigest() returns null given a bad bank key
        /// </summary>
        [Fact]
        public void TestGetItemDigestBadBankKey()
        {
            var item = itemViewRepo.GetItemDigest(badBankKey, goodItemKey);
            Assert.Null(item);
        }
        
        /// <summary>
        /// Tests that GetItemDigest() returns null given a bad bank key and bad item key
        /// </summary>
        [Fact]
        public void TestGetItemDigestBothBadKeys()
        {
            var item = itemViewRepo.GetItemDigest(badBankKey, badItemKey);
            Assert.Null(item);
        }


    }
}
