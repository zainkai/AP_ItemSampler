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
        ItemDigest MathDigest, ElaDigest, DuplicateDigest;
        ImmutableArray<ItemDigest> ItemDigests;
        ItemViewRepo ItemViewRepo;
        SampleItemsContext Context;
        int GoodItemKey;
        int BadItemKey;
        int GoodBankKey;
        int BadBankKey;
        int DuplicateItemKey, DuplicateBankKey;
        ItemCardViewModel MathCard, ElaCard, DuplicateCard;

        public ItemViewRepoTests()
        {
            GoodBankKey = 1;
            GoodItemKey = 2;
            BadBankKey = 3;
            BadItemKey = 9;
            GoodItemKey = 4;
            DuplicateBankKey = 5;
            DuplicateItemKey = 6;
            MathCard = ItemCardViewModel.Create(bankKey: GoodBankKey, itemKey: GoodItemKey);
            ElaCard = ItemCardViewModel.Create(bankKey: BadBankKey, itemKey: BadItemKey);
            DuplicateCard = ItemCardViewModel.Create(bankKey: DuplicateBankKey, itemKey: DuplicateItemKey);
            MathDigest = new ItemDigest() { BankKey = GoodBankKey, ItemKey = GoodItemKey };
            ElaDigest = new ItemDigest() { BankKey = BadBankKey, ItemKey = BadItemKey };
            DuplicateDigest = new ItemDigest() { BankKey = DuplicateBankKey, ItemKey = DuplicateItemKey };
            ItemDigests = ImmutableArray.Create(MathDigest, ElaDigest, DuplicateDigest, DuplicateDigest);
            var itemCards = ImmutableArray.Create(MathCard, ElaCard, DuplicateCard, DuplicateCard);

            Context = SampleItemsContext.Create(itemDigests: ItemDigests, itemCards: itemCards);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            ItemViewRepo = new ItemViewRepo(Context, loggerFactory.Object);
        }

        [Fact]
        public void TestGetItemDigest()
        {
            var result = ItemViewRepo.GetItemDigest(GoodBankKey, GoodItemKey);
            var resultCheck = Context.ItemDigests.FirstOrDefault(i => i.ItemKey == GoodItemKey && i.BankKey == GoodBankKey);

            Assert.NotNull(result);
            Assert.Equal(result, resultCheck);
        }

        [Fact]
        public void TestGetItemDigestDuplicate()
        {
            bool thrown = true;
            try
            {
                var result = ItemViewRepo.GetItemDigest(DuplicateBankKey, DuplicateItemKey);
                thrown = false;
            }
            catch { }
            if (!thrown)
                throw new System.Exception("Multiple items exception not thrown.");
        }

        [Fact]
        public void TestGetItemCard()
        {
            var result = ItemViewRepo.GetItemCardViewModel(BadBankKey, BadItemKey);
            var resultCheck=Context.ItemCards.FirstOrDefault(i => i.ItemKey == BadItemKey && i.BankKey == BadBankKey);

            Assert.NotNull(result);
            Assert.Equal(result, resultCheck);
        }

        [Fact]
        public void TestGetItemCardDuplicate()
        {
            bool thrown = true;
            try
            {
                var result = ItemViewRepo.GetItemCardViewModel(DuplicateBankKey, DuplicateItemKey);
                thrown = false;
            }
            catch { }
            if (!thrown)
                throw new System.Exception("Multiple items exception not thrown.");
        }

        //TODO:
        //- add tests for if itemcards and itemdigests have more than one item matching?
        //- add tests for more like this methods
    }
}
