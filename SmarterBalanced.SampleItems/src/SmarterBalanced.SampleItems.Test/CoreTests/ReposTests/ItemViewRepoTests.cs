using Microsoft.Extensions.Logging;
using Moq;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;
namespace SmarterBalanced.SampleItems.Test.CoreTests.ReposTests
{
    public class ItemViewRepoTests
    {
        ItemDigest MathDigest, ElaDigest, DuplicateDigest;
        Subject Math, Ela;
        Claim Claim1, Claim2;
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

            Math = new Subject("Math", "", "", new ImmutableArray<Claim>() { }, new ImmutableArray<string>() { });
            Ela = new Subject("Ela", "", "", new ImmutableArray<Claim>() { }, new ImmutableArray<string>() { });
            Claim1 = new Claim("1", "", "");
            Claim2 = new Claim("2", "", "");

            //generated item cards for more like this tests
            itemCards = itemCards.AddRange(MoreItemCards());
            var settings = new AppSettings() { SettingsConfig = new SettingsConfig() { NumMoreLikeThisItems = 3 } };

            Context = SampleItemsContext.Create(itemDigests: ItemDigests, itemCards: itemCards, appSettings: settings);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
            ItemViewRepo = new ItemViewRepo(Context, loggerFactory.Object);
        }

        private ImmutableArray<ItemCardViewModel> MoreItemCards()
        {
            var subjectCodes = new string[] { "Math", "Ela", "Science" };
            var claimCodes = new string[] { "1", "2", "3" };
            var gradeValues = GradeLevelsUtils.singleGrades.ToList();
            var moreCards = new List<ItemCardViewModel>();
            for (int i = 10; i < 60; i++)
            {
                moreCards.Add(ItemCardViewModel.Create(
                    bankKey: 10, 
                    itemKey: i, 
                    grade: gradeValues[i % gradeValues.Count],
                    subjectCode: subjectCodes[i%subjectCodes.Length],
                    claimCode: claimCodes[((i+60)/7)%claimCodes.Length]));
            }
            return moreCards.ToImmutableArray();
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
                throw new Exception("Multiple items exception not thrown.");
        }

        //TODO:
        //- add tests for if itemcards and itemdigests have more than one item matching?
        //- add tests for more like this methods

        [Fact]
        public void TestMoreLikeThisHappyCase()
        {
            var itemDigest = new ItemDigest() { Subject = Math, Claim = Claim1, Grade=GradeLevels.Grade6};
            var more = ItemViewRepo.GetMoreLikeThis(itemDigest);

            Assert.Equal(3, more.GradeAboveItems.ItemCards.Count());
            Assert.Equal(3, more.GradeBelowItems.ItemCards.Count());
            Assert.Equal(3, more.SameGradeItems.ItemCards.Count());

            foreach (var card in more.GradeAboveItems.ItemCards)
            {
                Assert.Equal(GradeLevels.Grade7, card.Grade);
            }
            foreach (var card in more.SameGradeItems.ItemCards)
            {
                Assert.Equal(GradeLevels.Grade6, card.Grade);
            }
            foreach (var card in more.GradeBelowItems.ItemCards)
            {
                Assert.Equal(GradeLevels.Grade5, card.Grade);
            }

        }

        [Fact]
        public void TestMoreLikeThisComparer()
        {
            var comparer = new MoreLikeThisComparer("Math", "1");
            var card1 = ItemCardViewModel.Create(subjectCode: "Math", claimCode: "1");
            var card2 = ItemCardViewModel.Create(subjectCode: "Math", claimCode: "2");
            var card3 = ItemCardViewModel.Create(subjectCode: "Ela", claimCode: "2");
            var cards = new List<ItemCardViewModel>() { card2, card1, card3 };
            var ordered = cards.OrderBy(c => c, comparer).ToList();
            
            Assert.Equal(ordered[0], card1);
            Assert.Equal(ordered[1], card2);
            Assert.Equal(ordered[2], card3);
        }
        
    }
}
