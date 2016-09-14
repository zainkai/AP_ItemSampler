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
            ItemMetadata metadata = new ItemMetadata();
            ItemContents contents = new ItemContents();
            metadata.metadata = new Dal.Models.XMLRepresentations.SmarterAppMetadataXmlRepresentation();
            contents.item = new Dal.Models.XMLRepresentations.ItemXmlFieldRepresentation();
            metadata.metadata.ItemKey = 1;
            metadata.metadata.Grade = 5;
            metadata.metadata.Target = "Test target string";
            metadata.metadata.Claim = "Test claim string";
            metadata.metadata.InteractionType = "EQ";
            metadata.metadata.Subject = "MATH";

            contents.item.ItemKey = 1;
            contents.item.ItemBank = 2;

            ItemDigest digest = ItemDigestTranslation.ItemToItemDigest(metadata, contents);
            Assert.Equal(1, digest.ItemKey);
            Assert.Equal(2, digest.BankKey);
            Assert.Equal(5, digest.Grade);
            Assert.Equal("Test target string", digest.Target);
            Assert.Equal("Test claim string", digest.Claim);
            Assert.Equal("MATH", digest.Subject);
            Assert.Equal("EQ", digest.InteractionType);
        }

        [Fact]
        public void ItemstoItemDigestsTest()
        {

        }


    }
}
