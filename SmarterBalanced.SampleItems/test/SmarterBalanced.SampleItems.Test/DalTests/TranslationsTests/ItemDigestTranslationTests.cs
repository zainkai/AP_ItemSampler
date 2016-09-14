using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SmarterBalanced.SampleItems.Dal.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Infrastructure;
using System.IO;

namespace SmarterBalanced.SampleItems.Test.DalTests.TranslationsTests
{
    public class ItemDigestTranslationTests
    {
        [Fact]
        public void ItemToItemDigestTest()
        {
            int testItemKey = 1;
            int testItemBank = 2;
            int testGrade = 5;

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

        [Fact]
        public void ItemstoItemDigestsTest()
        {
            List<ItemContents> contentsList = new List<ItemContents>();
            List<ItemMetadata> metadataList = new List<ItemMetadata>();
            List<ItemDigest> digests = new List<ItemDigest>();
            int[] itemKeys = Enumerable.Range(50, 10).ToArray();
            int[] banksKeys = Enumerable.Range(100, 10).ToArray();
            int[] grades = Enumerable.Range(1, 10).ToArray();

            int i;
            for (i = 0; i < itemKeys.Length; i++)
            {
                ItemMetadata metadata = new ItemMetadata();
                ItemContents contents = new ItemContents();

                metadata.metadata = new Dal.Models.XMLRepresentations.SmarterAppMetadataXmlRepresentation();
                contents.item = new Dal.Models.XMLRepresentations.ItemXmlFieldRepresentation();

                //Test metadata attributes
                metadata.metadata.ItemKey = itemKeys[i];
                metadata.metadata.Grade = grades[i];
                metadata.metadata.Target = "Test target string";
                metadata.metadata.Claim = "Test claim string";
                metadata.metadata.InteractionType = "EQ";
                metadata.metadata.Subject = "MATH";

                //Test contents attributes
                contents.item.ItemKey = itemKeys[i];
                contents.item.ItemBank = banksKeys[i];

                metadataList.Add(metadata);
                contentsList.Add(contents);
            }
            digests = ItemDigestTranslation.ItemsToItemDigests(metadataList, contentsList).ToList();

            Assert.Equal(itemKeys.Length, digests.Count);
        }
    }
}
