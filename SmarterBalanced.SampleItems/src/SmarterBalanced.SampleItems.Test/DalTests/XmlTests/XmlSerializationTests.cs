using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.DalTests.XmlTests
{
    public class XmlSerializationTests
    {
        // Path to directory containing content items for testing
        static string testContentItemsPath = Directory.GetDirectories(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName,
            "TestContentItems",
            SearchOption.AllDirectories)[0];

        /// <summary>
        /// Test that the xml serializer deserializes the XML document
        /// </summary>
        [Fact]
        public void TestDeserializeXmltoItemMetadata()
        {
            int expectedItemKey = 856;
            string expectedGrade = "7";
            string expectedTarget = "Practice Test Refresh";
            string expectedInteractionType = "HT";
            string expectedSubject = "ELA";
            string expectedClaim = "The student will identify and use the best academic or grade-level or below domain-specific (but not scientific or social studies) construct-relevant word(s)/phrase to convey the precise or intended meaning of a text especially with informational/explanatory writing.";

            string testFile = @"/Items/Item-187-856/metadata.xml";
            FileInfo metadataFile = new FileInfo(testContentItemsPath + testFile);
            ItemMetadata metadata = XmlSerialization.DeserializeXml<ItemMetadata>(metadataFile);

            Assert.NotNull(metadata);
            Assert.NotNull(metadata.Metadata);
            Assert.Equal(expectedItemKey, metadata.Metadata.ItemKey);
            Assert.Equal(expectedGrade, metadata.Metadata.GradeCode);
            Assert.Equal(expectedTarget, metadata.Metadata.TargetAssessmentType);
            Assert.Equal(expectedInteractionType, metadata.Metadata.InteractionType);
            Assert.Equal(expectedSubject, metadata.Metadata.SubjectCode);
            Assert.Equal(expectedClaim, metadata.Metadata.SufficientEvidenceOfClaim);
        }

        /// <summary>
        /// Test that the xml serializer correctly parses item-BANK-KEY.xml
        /// </summary>
        [Fact]
        public void TestDeserializeXmltoItemContents()
        {
            int expectedItemKey = 856;
            int expectedBankKey = 187;

            string testFile = @"/Items/Item-187-856/item-187-856.xml";
            FileInfo metadataFile = new FileInfo(testContentItemsPath + testFile);
            ItemContents contents = XmlSerialization.DeserializeXml<ItemContents>(metadataFile);

            Assert.Equal(expectedItemKey, contents.Item.ItemKey);
            Assert.Equal(expectedBankKey, contents.Item.ItemBank);
        }

        /// <summary>
        /// Test that the xml serializer will deserialize a list of documents
        /// </summary>
        [Fact]
        public async void TestDeserializeXmlFiles()
        {
            IEnumerable<FileInfo> metadataFiles = XmlSerialization.FindMetadataXmlFiles(testContentItemsPath);
            IEnumerable<ItemMetadata> metadata = await XmlSerialization.DeserializeXmlFilesAsync<ItemMetadata>(metadataFiles);
            Assert.Equal(metadataFiles.Count(), metadata.Count());

            IEnumerable<FileInfo> contentFiles = XmlSerialization.FindContentXmlFiles(testContentItemsPath);
            IEnumerable<ItemContents> contents = await XmlSerialization.DeserializeXmlFilesAsync<ItemContents>(contentFiles);
            Assert.Equal(metadataFiles.Count(), contents.Count());
        }

        /// <summary>
        /// Test that the FindMetadataXmlFiles method finds the correct number of files
        /// </summary>
        [Fact]
        public void TestFindMetadataXmlFiles()
        {
            int metadataFilesCount = 6;
            var metadataFiles = XmlSerialization.FindMetadataXmlFiles(testContentItemsPath);
            Assert.Equal(metadataFilesCount, metadataFiles.Count());
        }

        /// <summary>
        /// Test that the FindContentXmlFiles method finds the correct number of files
        /// </summary>
        [Fact]
        public void TestFindContentXmlFiles()
        {
            int contentFilesCount = 6;
            var contentFiles = XmlSerialization.FindContentXmlFiles(testContentItemsPath);
            Assert.Equal(contentFilesCount, contentFiles.Count());
        }
    }
}
