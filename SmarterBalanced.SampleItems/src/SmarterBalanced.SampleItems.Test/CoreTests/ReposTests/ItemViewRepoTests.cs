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
        ItemDigest mathDigest, elaDigest;
        ImmutableArray<ItemDigest> itemDigests;
        ItemViewRepo itemViewRepo;
        SampleItemsContext context;
        int goodItemKey;
        int goodBankKey;
        int badItemKey;
        int badBankKey;
        ItemCardViewModel mathCard, elaCard;

        public ItemViewRepoTests()
        {
            goodBankKey = 1;
            goodItemKey = 2;
            badBankKey = 3;
            goodItemKey = 4;
            mathCard = ItemCardViewModel.Create(bankKey: goodBankKey, itemKey: goodItemKey);
            elaCard = ItemCardViewModel.Create(bankKey: badBankKey, itemKey: badItemKey);
            mathDigest = new ItemDigest() { BankKey = goodBankKey, ItemKey = goodItemKey };
            elaDigest = new ItemDigest() { BankKey = badBankKey, ItemKey = badItemKey };
            itemDigests = ImmutableArray.Create(mathDigest, elaDigest);
            var itemCards = ImmutableArray.Create(mathCard, elaCard);

            context = SampleItemsContext.Create(itemDigests: itemDigests, itemCards: itemCards);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            itemViewRepo = new ItemViewRepo(context, loggerFactory.Object);

            
        }

        [Fact]
        private void TestGetItem()
        {
            var result = itemViewRepo.GetItemDigest(goodBankKey, goodItemKey);
            var resultCheck = context.ItemDigests.FirstOrDefault(i => i.ItemKey == goodItemKey && i.BankKey == goodBankKey);

            Assert.NotNull(result);
            Assert.Equal(result, resultCheck);
        }

        [Fact]
        private void TestGetItemCard()
        {
            var result = itemViewRepo.GetItemCardViewModel(badBankKey, badItemKey);
            var resultCheck=context.ItemCards.FirstOrDefault(i => i.ItemKey == goodItemKey && i.BankKey == goodBankKey);

            Assert.NotNull(result);
            Assert.Equal(result, resultCheck);
        }

        //TODO:
        //- add tests for if itemcards and itemdigests have more than one item matching?
        //-
    }
}
