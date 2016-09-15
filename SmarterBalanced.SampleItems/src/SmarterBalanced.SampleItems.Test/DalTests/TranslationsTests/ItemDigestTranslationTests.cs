using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SmarterBalanced.SampleItems.Dal.Exceptions;
using SmarterBalanced.SampleItems.Dal.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Infrastructure;
using System.IO;

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
            metadata.metadata = new Dal.Models.XMLRepresentations.SmarterAppMetadataXmlRepresentation();
            contents.item = new Dal.Models.XMLRepresentations.ItemXmlFieldRepresentation();
            metadata.metadata.ItemKey = testItemKey;
            metadata.metadata.Grade = testGrade;
            metadata.metadata.Target = "Test target string";
            metadata.metadata.Claim = "Test claim string";
            metadata.metadata.InteractionType = "EQ";
            metadata.metadata.Subject = "MATH";

            contents.item.ItemKey = testItemKey;
            contents.item.ItemBank = testItemBank;

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
            metadata.metadata = new Dal.Models.XMLRepresentations.SmarterAppMetadataXmlRepresentation();
            contents.item = new Dal.Models.XMLRepresentations.ItemXmlFieldRepresentation();
            metadata.metadata.ItemKey = 1;
            metadata.metadata.Grade = "7";
            metadata.metadata.Target = "Test target string";
            metadata.metadata.Claim = "Test claim string";
            metadata.metadata.InteractionType = "EQ";
            metadata.metadata.Subject = "MATH";

            contents.item.ItemKey = 2;
            contents.item.ItemBank = 3;
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

                metadata.metadata = new Dal.Models.XMLRepresentations.SmarterAppMetadataXmlRepresentation();
                contents.item = new Dal.Models.XMLRepresentations.ItemXmlFieldRepresentation();

                //Test metadata attributes
                metadata.metadata.ItemKey = itemKeys[i];
                metadata.metadata.Grade = itemKeys[i].ToString();
                metadata.metadata.Target = testTarget + itemKeys[i];
                metadata.metadata.Claim = testClaim + itemKeys[i];
                metadata.metadata.InteractionType = testInteractionType + itemKeys[i];
                metadata.metadata.Subject = testSubject + itemKeys[i];

                //Test contents attributes
                contents.item.ItemKey = itemKeys[i];
                contents.item.ItemBank = banksKeys[i];

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
