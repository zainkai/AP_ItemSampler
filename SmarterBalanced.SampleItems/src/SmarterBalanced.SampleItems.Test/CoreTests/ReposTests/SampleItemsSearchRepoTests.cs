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
    public class SampleItemsSearchRepoTests
    {
        SampleItemsSearchRepo sampleItemsSearchRepo;
        ImmutableArray<ItemDigest> itemDigests;
        ItemDigest goodItemDigest;
        int goodItemKey;
        int goodBankKey;
        int badItemKey;
        int badBankKey;
        SampleItemsContext context;
        Claim claim1, claim2;
        Subject math, ela;
        InteractionType itEla, itMath;
        public SampleItemsSearchRepoTests()
        {
            goodItemKey = 2343;
            goodBankKey = 8398;
            badItemKey = 9234;
            badBankKey = 1123;
            claim1 = new Claim("claim1", "1", "ELA Claim 1");
            claim2 = new Claim("claim2", "2", "Math Claim 1");
            itMath = new InteractionType("2", "Math Itype", 2);
            itEla = new InteractionType("1", "Ela Itype", 1);
            math = Subject.Create("Math", "Mathematics", "Math", ImmutableArray.Create(claim2), ImmutableArray.Create(itMath.Code));
            ela = Subject.Create("ELA", "English", "ELA", ImmutableArray.Create(claim1), ImmutableArray.Create(itEla.Code));
            var interactionTypes = ImmutableArray.Create(itEla, itMath);
            var subjects = ImmutableArray.Create(ela, math);
            var itemCards = ImmutableArray.Create(
                        ItemCardViewModel.Create(bankKey: goodBankKey, itemKey: goodItemKey, grade: GradeLevels.Grade6,
                                                 subjectCode: math.Code, interactionTypeCode: itMath.Code, claimCode: claim1.Code),
                        ItemCardViewModel.Create(bankKey: goodBankKey, itemKey: badItemKey, grade: GradeLevels.High,
                                                 subjectCode: math.Code, interactionTypeCode: itMath.Code, claimCode: claim2.Code),
                        ItemCardViewModel.Create(bankKey: badBankKey, itemKey: goodItemKey, grade: GradeLevels.Grade9,
                                                 subjectCode: ela.Code, interactionTypeCode: itEla.Code, claimCode: claim1.Code),
                        ItemCardViewModel.Create(bankKey: badBankKey, itemKey: badItemKey, grade: GradeLevels.Grade4,
                                                 subjectCode: ela.Code, interactionTypeCode: itEla.Code, claimCode: claim2.Code),
                        ItemCardViewModel.Create(bankKey: 1, itemKey: 2, grade: GradeLevels.Grade12)
                );
            context = SampleItemsContext.Create(itemCards: itemCards, subjects: subjects, interactionTypes: interactionTypes);

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            sampleItemsSearchRepo = new SampleItemsSearchRepo(context, loggerFactory.Object);
        }

        /// <summary>
        /// Asserts whether or not the card lists have exactly the same cards
        /// </summary>
        /// <param name="cards1">First list to compare</param>
        /// <param name="cards2">Second list to compare</param>
        /// <returns></returns>
        private void AssertItemCardListsEqual(IList<ItemCardViewModel> cards1, IList<ItemCardViewModel> cards2)
        {
            foreach (var card in cards1)
            {
                try { cards2.Remove(card); }
                catch { }
            }
            if (cards2.Any())
            {
                throw new System.Exception("Lists not equal");
            }
        }


        /// <summary>
        /// Orders cards by bank key, then by item key
        /// </summary>
        /// <param name="cardsList">List of cards to order</param>
        /// <returns></returns>
        private List<ItemCardViewModel> Sort(IList<ItemCardViewModel> cardsList)
        {
            return cardsList.OrderBy(c => c.BankKey).ThenBy(c => c.ItemKey).ToList();
        }



        #region GetItemCards
        [Fact]
        public void HappyCase()
        {
            Assert.Equal(context.ItemCards.Count(), sampleItemsSearchRepo.GetItemCards().Count);
        }

        [Fact]
        public void TestSearchGradeLevel()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.High, null, new string[] { }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => GradeLevelsUtils.Contains(GradeLevels.High, c.Grade)).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count);
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestSearchSubject()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, new List<string>() { math.Code }, new string[] { }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.SubjectCode == math.Code).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count);
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestSearchClaim()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, new List<string>() { math.Code, ela.Code }, new string[] { }, new string[] { claim1.Code });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.ClaimCode == claim1.Code).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count);
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestSearchItemType()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, new List<string>() { math.Code, ela.Code }, new string[] { itEla.Code }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.InteractionTypeCode == itEla.Code).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count);
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestNullItemId()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, null, new string[] { }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards;

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count());
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestNonIntegerItemId()
        {
            var parameters = new ItemsSearchParams("NOT A NUMBER!", GradeLevels.All, null, new string[] { }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards;

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count());
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        // NOTE: This test should fail. Try to fix the code when "parms" is null
        [Fact]
        public void TestNullSearchParamObj()
        {
            var cards = sampleItemsSearchRepo.GetItemCards(null);

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, context.ItemCards.Count());
            Assert.Equal(Sort(cards), Sort(context.ItemCards));
        }

        // TODO: Test following cases:
        //  - Test each ItemSearchParams property when null

        [Fact]
        public void TestGetItemCardsNull()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, null, null, null);
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards;

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count());
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        //  - Use Miletone 1 UAT cases as templates
        [Fact]
        public void TestSearchTwoClaims()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, new List<string>() { math.Code }, new string[] { }, new string[] { claim1.Code, claim2.Code });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.SubjectCode == math.Code && (c.ClaimCode == claim1.Code
                                                     || c.ClaimCode == claim2.Code)).ToList();
            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count);
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestSearchTwoITypes()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, new List<string>() { math.Code }, new string[] { itEla.Code, itMath.Code }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.SubjectCode == math.Code && (c.ClaimCode == claim1.Code
                                                     || c.ClaimCode == claim2.Code)).ToList();
            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count);
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestSearchTwoSubjects()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, new List<string>() { math.Code, ela.Code }, new string[] { }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.SubjectCode == math.Code || c.SubjectCode == ela.Code).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count);
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestSearchAllCategories()
        {
            var parameters = new ItemsSearchParams(goodItemKey.ToString(), GradeLevels.Grade6, new List<string>() { math.Code }, new string[] { itMath.Code }, new string[] { claim1.Code });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.SubjectCode == math.Code && c.ItemKey == goodItemKey && c.Grade == GradeLevels.Grade6
                                                     && c.InteractionTypeCode == itMath.Code && c.ClaimCode == claim1.Code).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, cardsCheck.Count);
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestGetItemCardsAll()
        {
            var cards = sampleItemsSearchRepo.GetItemCards();
            Assert.NotNull(cards);
            Assert.Equal(cards.Count, context.ItemCards.Count());
        }

        [Fact]
        public void TestGetItemCardsMultipleGrade()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.Grade9 | GradeLevels.Grade4, null, new string[] { }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.Grade == GradeLevels.Grade9 || c.Grade == GradeLevels.Grade4 
                                                     || c.Grade == GradeLevels.Elementary || c.Grade == GradeLevels.High).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cards.Count, 3);
            Assert.Equal(Sort(cards), Sort(cardsCheck));
        }

        [Fact]
        public void TestGetItemCardsEmptyList()
        {
            var parameters = new ItemsSearchParams("-1", GradeLevels.High, null, new string[] { }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            Assert.NotNull(cards);
            Assert.Empty(cards);
        }

        [Fact]
        public void TestGetItemCardsItemKey()
        {
            var parameters = new ItemsSearchParams(badItemKey.ToString(), GradeLevels.All, null, new string[] { }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.ItemKey.ToString().StartsWith(badItemKey.ToString())).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cardsCheck.Count, cards.Count);
            Assert.Equal(Sort(cardsCheck), Sort(cards));
        }

        [Fact]
        public void TestGetItemCardsInteractionNoSubject()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, null, new string[] { itMath.Code }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            Assert.NotNull(cards);
            Assert.Equal(context.ItemCards.Count(), cards.Count);

            //Shouldn't filter cards because no subject
            Assert.Equal(Sort(context.ItemCards), Sort(cards));
        }

        [Fact]
        public void TestGetItemCardsClaimNoSubject()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, null, new string[] { }, new string[] { claim2.Code });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            Assert.NotNull(cards);
            Assert.Equal(context.ItemCards.Count(), cards.Count);

            //Shouldn't filter cards because no subject
            Assert.Equal(Sort(context.ItemCards), Sort(cards));
        }

        [Fact]
        public void TestGetItemCardsSubjectAndInteraction()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, new List<string>() { math.Code }, new string[] { itMath.Code }, new string[] { });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.SubjectCode == math.Code && c.InteractionTypeCode == itMath.Code).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cardsCheck.Count, cards.Count);
            Assert.Equal(Sort(cardsCheck), Sort(cards));
        }

        [Fact]
        public void TestGetItemCardsSubjectAndClaim()
        {
            var parameters = new ItemsSearchParams(null, GradeLevels.All, new List<string>() { math.Code }, new string[] { }, new string[] { claim2.Code });
            var cards = sampleItemsSearchRepo.GetItemCards(parameters);
            var cardsCheck = context.ItemCards.Where(c => c.SubjectCode == math.Code && c.ClaimCode == claim2.Code).ToList();

            Assert.NotNull(cards);
            Assert.Equal(cardsCheck.Count, cards.Count);
            Assert.Equal(Sort(cardsCheck), Sort(cards));
        }

        #endregion

        #region GetItemsSearchViewModel

        [Fact]
        public void TestGetItemSearchViewModel()
        {
            var model = sampleItemsSearchRepo.GetItemsSearchViewModel();

            Assert.NotNull(model);
            Assert.Equal(model.InteractionTypes, context.InteractionTypes);
            Assert.Equal(model.Subjects, context.Subjects);
        }

        #endregion

    }
}
