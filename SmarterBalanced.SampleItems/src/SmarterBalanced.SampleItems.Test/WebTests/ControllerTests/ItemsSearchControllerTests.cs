using Moq;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Web.Controllers;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
    public class ItemsSearchControllerTests
    {

        ItemsSearchController controller;
        int goodBankKey;
        int badBankKey;
        int goodItemKey;
        int badItemKey;
        List<ItemDigest> itemDigests;

        public ItemsSearchControllerTests()
        {
            itemDigests = new List<ItemDigest>() {
                new ItemDigest
                {
                    BankKey = goodBankKey,
                    ItemKey = goodItemKey,
                    Grade = GradeLevels.Grade6
                },
                new ItemDigest
                {
                    BankKey = goodBankKey,
                    ItemKey = badItemKey,
                    Grade = GradeLevels.High
                },
                new ItemDigest
                {
                    BankKey = badBankKey,
                    ItemKey = goodItemKey,
                    Grade = GradeLevels.Grade9
                },
                new ItemDigest
                {
                    BankKey = badBankKey,
                    ItemKey = badItemKey,
                    Grade = GradeLevels.Grade4
                }
            };

            var sampleItemsSearchRepoMock = new Mock<ISampleItemsSearchRepo>();
            controller = new ItemsSearchController(sampleItemsSearchRepoMock.Object);
        }

        

    }
}
