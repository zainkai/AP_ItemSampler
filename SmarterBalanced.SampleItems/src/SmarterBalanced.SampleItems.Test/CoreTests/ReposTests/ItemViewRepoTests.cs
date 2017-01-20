using Microsoft.Extensions.Logging;
using Moq;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;
namespace SmarterBalanced.SampleItems.Test.CoreTests.ReposTests
{
    public class ItemViewRepoTests
    {
        ItemDigest mathDigest, elaDigest, duplicateDigest;
        ImmutableArray<ItemDigest> itemDigests;
        ItemViewRepo itemViewRepo;
        SampleItemsContext context;
        int goodItemKey;
        int goodBankKey;
        int badItemKey;
        int badBankKey;
        int duplicateItemKey, duplicateBankKey;
        ItemCardViewModel mathCard, elaCard, duplicateCard;

        public ItemViewRepoTests()
        {
            goodBankKey = 1;
            goodItemKey = 2;
            badBankKey = 3;
            goodItemKey = 4;
            duplicateBankKey = 5;
            duplicateItemKey = 6;
            mathCard = ItemCardViewModel.Create(bankKey: goodBankKey, itemKey: goodItemKey);
            elaCard = ItemCardViewModel.Create(bankKey: badBankKey, itemKey: badItemKey);
            duplicateCard = ItemCardViewModel.Create(bankKey: duplicateBankKey, itemKey: duplicateItemKey);
            mathDigest = new ItemDigest() { BankKey = goodBankKey, ItemKey = goodItemKey };
            elaDigest = new ItemDigest() { BankKey = badBankKey, ItemKey = badItemKey };
            duplicateDigest = new ItemDigest() { BankKey = duplicateBankKey, ItemKey = duplicateItemKey };
            itemDigests = ImmutableArray.Create(mathDigest, elaDigest, duplicateDigest, duplicateDigest);
            var itemCards = ImmutableArray.Create(mathCard, elaCard, duplicateCard, duplicateCard);

            context = SampleItemsContext.Create(itemDigests: itemDigests, itemCards: itemCards);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            itemViewRepo = new ItemViewRepo(context, loggerFactory.Object);
        }

        [Fact]
        public void TestGetItemDigest()
        {
            var result = itemViewRepo.GetItemDigest(goodBankKey, goodItemKey);
            var resultCheck = context.ItemDigests.FirstOrDefault(i => i.ItemKey == goodItemKey && i.BankKey == goodBankKey);

            Assert.NotNull(result);
            Assert.Equal(result, resultCheck);
        }

        [Fact]
        public void TestGetItemDigestDuplicate()
        {
            bool thrown = true;
            try
            {
                var result = itemViewRepo.GetItemDigest(duplicateBankKey, duplicateItemKey);
                thrown = false;
            }
            catch { }
            if (!thrown)
                throw new System.Exception("Multiple items exception not thrown.");
        }

        [Fact]
        public void TestGetItemCard()
        {
            var result = itemViewRepo.GetItemCardViewModel(badBankKey, badItemKey);
            var resultCheck=context.ItemCards.FirstOrDefault(i => i.ItemKey == badItemKey && i.BankKey == badBankKey);

            Assert.NotNull(result);
            Assert.Equal(result, resultCheck);
        }

        [Fact]
        public void TestGetItemCardDuplicate()
        {
            bool thrown = true;
            try
            {
                var result = itemViewRepo.GetItemCardViewModel(duplicateBankKey, duplicateItemKey);
                thrown = false;
            }
            catch { }
            if (!thrown)
                throw new System.Exception("Multiple items exception not thrown.");
        }

        //TODO:
        //- add tests for if itemcards and itemdigests have more than one item matching?
        //-
    }
}
