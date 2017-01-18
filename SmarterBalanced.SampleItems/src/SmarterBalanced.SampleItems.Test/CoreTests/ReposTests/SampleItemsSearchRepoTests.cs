using Microsoft.Extensions.Logging;
using Moq;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

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
            
            SampleItemsContext context = SampleItemsContext.Create(
                itemCards: ImmutableArray.Create(
                    ItemCardViewModel.Create(bankKey: goodBankKey, itemKey: goodItemKey, grade: GradeLevels.Grade6),
                    ItemCardViewModel.Create(bankKey: goodBankKey, itemKey: badItemKey, grade: GradeLevels.High),
                    ItemCardViewModel.Create(bankKey: badBankKey, itemKey: goodItemKey, grade: GradeLevels.Grade9),
                    ItemCardViewModel.Create(bankKey: badBankKey, itemKey: badItemKey, grade: GradeLevels.Grade4)));

            var loggerFactory = new Mock<ILoggerFactory>();
            var logger = new Mock<ILogger>();
            loggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            sampleItemsSearchRepo = new SampleItemsSearchRepo(context, loggerFactory.Object);
        }



        #region GetItemCards
        [Fact]
        public void HappyCase()
        {
            Assert.Equal(4, sampleItemsSearchRepo.GetItemCards().Count);
        }

        [Fact(Skip = "TODO")]
        public void TestSearchGradeLevel()
        {

        }

        [Fact(Skip = "TODO")]
        public void TestSearchSubject()
        {

        }

        [Fact(Skip = "TODO")]
        public void TestSearchClaim()
        {

        }

        [Fact(Skip = "TODO")]
        public void TestSearchItemType()
        {

        }

        [Fact(Skip = "TODO")]
        public void TestNullItemId()
        {

        }

        [Fact(Skip = "TODO")]
        public void TestNonIntegerItemId()
        {

        }

        // NOTE: This test should fail. Try to fix the code when "parms" is null
        [Fact(Skip = "TODO")]
        public void TestNullSearchParamObj()
        {

        }

        // TODO: Test following cases:
        //  - Test each ItemSearchParams property when null
        //  - Use Miletone 1 UAT cases as templates


        #endregion

        #region GetItemsSearchViewModel

        [Fact(Skip = "TODO")]
        public void TestGetItemSearchViewModel()
        {

        }

        #endregion

    }
}
