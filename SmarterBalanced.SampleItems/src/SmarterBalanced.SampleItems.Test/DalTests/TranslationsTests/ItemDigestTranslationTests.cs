using SmarterBalanced.SampleItems.Dal.Exceptions;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.DalTests.TranslationsTests
{
    public class ItemDigestTranslationTests
    {
        /// <summary>
        /// Test translating a single ItemMetadata object and a single ItemContents object into an ItemDigest object
        /// </summary>
        [Fact]
        public void TestItemToItemDigest()
        {
            int testItemKey = 1;
            int testItemBank = 2;
            string testGrade = "5";

            ItemMetadata metadata = new ItemMetadata();
            ItemContents contents = new ItemContents();
            metadata.Metadata = new SmarterAppMetadataXmlRepresentation();
            contents.Item = new ItemXmlFieldRepresentation();
            metadata.Metadata.ItemKey = testItemKey;
            metadata.Metadata.Grade = testGrade;
            metadata.Metadata.Target = "Test target string";
            metadata.Metadata.Claim = "Test claim string";
            metadata.Metadata.InteractionType = "EQ";
            metadata.Metadata.Subject = "MATH";

            contents.Item.ItemKey = testItemKey;
            contents.Item.ItemBank = testItemBank;

            ItemDigest digest = ItemDigestTranslation.ItemToItemDigest(metadata, contents);
            Assert.Equal(testItemKey, digest.ItemKey);
            Assert.Equal(testItemBank, digest.BankKey);
            Assert.Equal(testGrade, digest.Grade);
            Assert.Equal("Test target string", digest.Target);
            Assert.Equal("Test claim string", digest.Claim);
            Assert.Equal("MATH", digest.Subject);
            Assert.Equal("EQ", digest.InteractionType);
        }

        /// <summary>
        /// Tests that an exception is thrown if the ItemKey fields do not match for the 
        /// ItemMetadata and ItemContents objects passed to the ItemToItemDigest method.
        /// </summary>
        [Fact]
        public void TestItemToItemDigestInvalid()
        {
            ItemMetadata metadata = new ItemMetadata();
            ItemContents contents = new ItemContents();
            metadata.Metadata = new SmarterAppMetadataXmlRepresentation();
            contents.Item = new ItemXmlFieldRepresentation();
            metadata.Metadata.ItemKey = 1;
            metadata.Metadata.Grade = "7";
            metadata.Metadata.Target = "Test target string";
            metadata.Metadata.Claim = "Test claim string";
            metadata.Metadata.InteractionType = "EQ";
            metadata.Metadata.Subject = "MATH";

            contents.Item.ItemKey = 2;
            contents.Item.ItemBank = 3;
            var exception = Assert.Throws(typeof(SampleItemsContextException), () => ItemDigestTranslation.ItemToItemDigest(metadata, contents));
            Assert.Equal("Cannot digest items with different ItemKey values.", exception.Message);
        }


        /// <summary>
        /// Test translating a collection of ItemMetadata objects and a collection of ItenContents objects
        /// into a collection of ItemDigest objects.
        /// </summary>
        [Fact]
        public void TestItemstoItemDigests()
        {
            int testItemCount = 3;
            List<ItemContents> contentsList = new List<ItemContents>();
            List<ItemMetadata> metadataList = new List<ItemMetadata>();
            IEnumerable<ItemDigest> digests;

            // Get a range of numbers from 50 to the number of items being tested.
            // Use the same numer for an item's key and bank to make it easy to validate that
            // ItemMetadata and ItemContents objects are being paired correctly.
            int[] itemKeys = Enumerable.Range(50, testItemCount).ToArray();
            int[] banksKeys = itemKeys;
            string testTarget = "Test target string";
            string testClaim = "Test claim string";
            string testInteractionType = "EQ";
            string testSubject = "MATH";

            int i;
            for (i = 0; i < testItemCount; i++)
            {
                ItemMetadata metadata = new ItemMetadata();
                ItemContents contents = new ItemContents();

                metadata.Metadata = new SmarterAppMetadataXmlRepresentation();
                contents.Item = new ItemXmlFieldRepresentation();

                //Test metadata attributes
                metadata.Metadata.ItemKey = itemKeys[i];
                metadata.Metadata.Grade = itemKeys[i].ToString();
                metadata.Metadata.Target = testTarget + itemKeys[i];
                metadata.Metadata.Claim = testClaim + itemKeys[i];
                metadata.Metadata.InteractionType = testInteractionType + itemKeys[i];
                metadata.Metadata.Subject = testSubject + itemKeys[i];

                //Test contents attributes
                contents.Item.ItemKey = itemKeys[i];
                contents.Item.ItemBank = banksKeys[i];

                metadataList.Add(metadata);
                contentsList.Add(contents);
            }
            digests = ItemDigestTranslation.ItemsToItemDigests(metadataList, contentsList);

            Assert.Equal(itemKeys.Length, digests.Count());

            foreach(ItemDigest digest in digests)
            {
                int id = digest.ItemKey;
                Assert.Equal(digest.ItemKey, digest.BankKey);
                Assert.Equal(digest.ItemKey.ToString(), digest.Grade);
                Assert.Equal(testTarget + id, digest.Target);
                Assert.Equal(testClaim + id, digest.Claim);
                Assert.Equal(testInteractionType + id, digest.InteractionType);
                Assert.Equal(testSubject + id, digest.Subject);
            }
        }
    }
}
