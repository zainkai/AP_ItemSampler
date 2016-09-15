using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using SmarterBalanced.SampleItems.Dal.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using SmarterBalanced.SampleItems.Dal.Infrastructure;

namespace SmarterBalanced.SampleItems.Test.DalTests.InfrastructureTests
{
    public class SampleItemsRepoTests : IDisposable
    {
        private SampleItemsRepo s_repo;
        private List<ItemDigest> itemDigests;
        private int bankKey;
        private int goodItemKey;
        private int badItemKey;


        /// <summary>
        /// Constructor for the test method. Initializes SampleItemsRepo
        /// so that it can be used for each of the test methods
        /// </summary>
        public SampleItemsRepoTests()
        {
            bankKey = 3434;
            goodItemKey = 2343434;
            badItemKey = 859656;

            itemDigests = new List<ItemDigest>();
            itemDigests.Add(new ItemDigest
            {
                BankKey = bankKey,
                ItemKey = badItemKey,
                Grade = "6"
            });

            itemDigests.Add(new ItemDigest
            {
                BankKey = bankKey,
                ItemKey = badItemKey,
                Grade = "7"
            });

            itemDigests.Add(new ItemDigest
            {
                BankKey = bankKey,
                ItemKey = goodItemKey,
                Grade = "7"
            });

            Mock<ISampleItemsContext> sampleItemsContextMock = new Mock<ISampleItemsContext>();
            sampleItemsContextMock.Setup(x => x.ItemDigests).Returns(itemDigests);

            s_repo = new SampleItemsRepo(sampleItemsContextMock.Object);
        }


        /// <summary>
        /// Dispose of the data used in the test
        /// </summary>
        public void Dispose()
        {
            s_repo = null;
            itemDigests= null;
        }

        /// <summary>
        /// Tests that GetItemDigests() returns all of the ItemDigest objects
        /// </summary>
        [Fact]
        public void TestGetItemDigests()
        {
            Assert.Equal(itemDigests.Count(), s_repo.GetItemDigests().Count());
        }

        /// <summary>
        /// Tests that GetItemDigests(pred) returns all of the matching items
        /// </summary>
        [Fact]
        public void TestGetItemDigestsWithPredicate()
        {
            Assert.Equal(itemDigests.Count(), s_repo.GetItemDigests(t => t.BankKey == bankKey).Count());
        }

        /// <summary>
        /// Tests that GetItemDigests(pred) returns a single-item IEnumerable
        /// matching the predicate
        /// </summary>
        [Fact]
        public void TestGetItemDigestsWithPredicateSingle()
        {
            Assert.Equal(1, s_repo.GetItemDigests(t => t.ItemKey == goodItemKey).Count());
        }


        /// <summary>
        /// Tests that GetItemDigest(bankKey, itemKey) returns an item
        /// </summary>
        [Fact]
        public void TestGetItemDigestSingle()
        {
            Assert.NotEqual(null, s_repo.GetItemDigest(bankKey, goodItemKey));
        }

        /// <summary>
        /// Tests that GetItemDigest(bankKey, goodItemKey) returns a default item
        /// when one is not in the list
        /// </summary>
        [Fact]
        public void TestGetItemDigestSingleDefault()
        {
            int nextKey = s_repo.GetItemDigests().Max(t => t.ItemKey) + 1;
            Assert.Equal(null, s_repo.GetItemDigest(bankKey, nextKey));
        }

        /// <summary>
        /// Tests that GetItemDigest(bankKey, goodItemKey) throws an 
        /// InvalidOperationException when there are duplicate items in the list
        /// </summary>
        [Fact]
        public void TestGetItemDigestSingleFailure()
        {
            Assert.Throws<InvalidOperationException>(() => s_repo.GetItemDigest(bankKey, badItemKey));
        }

        /// <summary>
        /// Tests that GetItemDigest(pred) throws an 
        /// InvalidOperationException when there are duplicate items in the list
        /// </summary>
        [Fact]
        public void TestGetItemDigestSingleFailureWithPredicate()
        {
            Assert.Throws<InvalidOperationException>(() => s_repo.GetItemDigest(t => t.BankKey == bankKey));
        }

        /// <summary>
        /// Tests that GetItemDigest(pred) returns an item when it is present 
        /// </summary>
        [Fact]
        public void TestGetItemDigestSingleSuccessWithPredicate()
        {
            Assert.NotEqual(null, s_repo.GetItemDigest(t => t.ItemKey == goodItemKey));
        }

    }
}
