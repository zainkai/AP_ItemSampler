using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Dal.Infrastructure;
using SmarterBalanced.SampleItems.Dal.Models;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Test.DalTests.InfrastructureTests
{
    public class XmlSerializationTests
    {
        [Fact]
        public void TestDeserializeXml()
        {
            string testFile = @"\DalTests\TestContentItems\Items\Item-187-856\metadata.xml";
            string baseTestDirectory = Directory.GetCurrentDirectory();
            FileInfo metadataFile = new FileInfo(baseTestDirectory + testFile);
            ItemMetadata metadata = XmlSerialization.DeserializeXml<ItemMetadata>(metadataFile);
            Assert.NotNull(metadata);
            Assert.NotNull(metadata.metadata);
        }

        [Fact]
        public void TestDeserializeXmlFiles()
        {
            string contentDir = Directory.GetCurrentDirectory() + @"\DalTests\TestContentItems";
            IEnumerable<FileInfo> files = new DirectoryInfo(contentDir).GetFiles("metadata.xml", SearchOption.AllDirectories);
            IEnumerable<ItemMetadata> metadata = XmlSerialization.DeserializeXmlFiles<ItemMetadata>(files);
            Assert.NotEmpty(metadata);
        }
    }
}
