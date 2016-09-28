using Moq;
using SmarterBalanced.SampleItems.Core.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using SmarterBalanced.SampleItems.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    Grade = "6"
                },
                new ItemDigest
                {
                    BankKey = goodBankKey,
                    ItemKey = badItemKey,
                    Grade = "High School"
                },
                new ItemDigest
                {
                    BankKey = badBankKey,
                    ItemKey = goodItemKey,
                    Grade = "9"
                },
                new ItemDigest
                {
                    BankKey = badBankKey,
                    ItemKey = badItemKey,
                    Grade = "4"
                }
            };

            var sampleItemsSearchRepoMock = new Mock<ISampleItemsSearchRepo>();
            controller = new ItemsSearchController(sampleItemsSearchRepoMock.Object);
        }

        

    }
}
