using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
namespace SmarterBalanced.SampleItems.Test.ModelTests
{
    public class ItemCardViewModelTests
    {
        [Fact]
        public void TestMoreLikeThisComparer()
        {
            var comparer = new MoreLikeThisComparer("Math", "1");
            var card1 = ItemCardViewModel.Create(subjectCode: "Math", claimCode: "1");
            var card2 = ItemCardViewModel.Create(subjectCode: "Math", claimCode: "2");
            var card3 = ItemCardViewModel.Create(subjectCode: "Ela", claimCode: "2");
            var cards = new List<ItemCardViewModel>() { card2, card1, card3 };
            var ordered = cards.OrderBy(c => c, comparer).ToList();

            Assert.NotNull(ordered);
            Assert.Equal(cards.Count, ordered.Count);

            Assert.Equal(ordered[0], card1);
            Assert.Equal(ordered[1], card2);
            Assert.Equal(ordered[2], card3);
        }

        [Fact]
        public void TestComparerEmptyList()
        {
            var comparer = new MoreLikeThisComparer("Math", "1");
            var cards = new List<ItemCardViewModel>();
            var ordered = cards.OrderBy(c => c, comparer).ToList();

            Assert.NotNull(ordered);
            Assert.Empty(ordered);
        }

        [Fact]
        public void TestComparerNullSubject()
        {
            var comparer = new MoreLikeThisComparer(null, "1");
            var card1 = ItemCardViewModel.Create(subjectCode: "Math", claimCode: "1");
            var card2 = ItemCardViewModel.Create(subjectCode: "Math", claimCode: "2");
            var card3 = ItemCardViewModel.Create(subjectCode: "Ela", claimCode: "2");
            var cards = new List<ItemCardViewModel>() { card2, card1, card3 };
            var ordered = cards.OrderBy(c => c, comparer).ToList();

            Assert.NotNull(ordered);
            Assert.Equal(cards.Count, ordered.Count);
        }

        [Fact]
        public void TestComparerNullClaim()
        {
            var comparer = new MoreLikeThisComparer("Ela", null);
            var card1 = ItemCardViewModel.Create(subjectCode: "Math", claimCode: "1");
            var card2 = ItemCardViewModel.Create(subjectCode: "Math", claimCode: "2");
            var card3 = ItemCardViewModel.Create(subjectCode: "Ela", claimCode: "2");
            var cards = new List<ItemCardViewModel>() { card2, card1, card3 };
            var ordered = cards.OrderBy(c => c, comparer).ToList();

            Assert.NotNull(ordered);
            Assert.Equal(cards.Count, ordered.Count);
            Assert.Equal(ordered[0], card3);
        }
    }
}
