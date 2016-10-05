using SmarterBalanced.SampleItems.Dal.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Dal.Infrastructure;
using SmarterBalanced.SampleItems.Dal.Translations;
using System.IO;
using SmarterBalanced.SampleItems.Dal.Models.Configurations;
using System.Xml.Linq;
using System.Xml;
using Gen = SmarterBalanced.SampleItems.Dal.Models.Generated;

namespace SmarterBalanced.SampleItems.Dal.Context
{
    public class SampleItemsContext : ISampleItemsContext
    {
        public IList<ItemDigest> ItemDigests { get; set; }

        public IList<AccessibilityResource> GlobalAccessibilityResources { get; set; }
        public IList<AccessibilityResourceFamily> AccessibilityResourceFamilies { get; set; }

        public IList<AccessibilityResource> GlobalAccessibilityResources { get; set; }
        public IList<AccessibilityResourceFamily> AccessibilityResourceFamilies { get; set; }

        /// <summary>
        /// TODO: Create itemdigest from xml serialization 
        /// </summary>
        public SampleItemsContext(AppSettings settings)
        {
            string contentDir = settings.SettingsConfig.ContentItemDirectory;

            //Find xml files
            Task<IEnumerable<FileInfo>> fetchMetadataFiles = XmlSerialization.FindMetadataXmlFiles(contentDir);
            Task<IEnumerable<FileInfo>> fetchContentsFiles = XmlSerialization.FindContentXmlFiles(contentDir);
            IEnumerable <FileInfo> metadataFiles = fetchMetadataFiles.Result;
            IEnumerable<FileInfo> contentsFiles = fetchContentsFiles.Result;

            //Parse Xml Files
            Task<IEnumerable<ItemMetadata>> deserializeMetadata = XmlSerialization.DeserializeXmlFilesAsync<ItemMetadata>(metadataFiles);
            Task<IEnumerable<ItemContents>> deserializeContents = XmlSerialization.DeserializeXmlFilesAsync<ItemContents>(contentsFiles);
            IEnumerable<ItemMetadata> itemMetadata = deserializeMetadata.Result;
            IEnumerable<ItemContents> itemContents = deserializeContents.Result;

            ItemDigests = ItemDigestTranslation.ItemsToItemDigests(itemMetadata, itemContents).ToList();

            Gen.Accessibility generatedAccessibility = XmlSerialization.DeserializeXml<Gen.Accessibility>(new FileInfo(settings.SettingsConfig.AccommodationsXMLPath));
            GlobalAccessibilityResources = generatedAccessibility.ToAccessibilityResources();

            AccessibilityResourceFamilies = generatedAccessibility.ResourceFamily
                .Select(f => f.ToAccessibilityResourceFamily(GlobalAccessibilityResources))
                .ToList();
        }
    }
}
