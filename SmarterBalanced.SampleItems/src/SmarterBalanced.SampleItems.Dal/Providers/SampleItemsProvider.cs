using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using Gen = SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SmarterBalanced.SampleItems.Dal.Xml;
using SmarterBalanced.SampleItems.Dal.Translations;

namespace SmarterBalanced.SampleItems.Dal.Providers
{
    public static class SampleItemsProvider
    {
        public static async Task<SampleItemsContext> LoadContext(AppSettings appSettings)
        {
            Gen.Accessibility generatedAccessibility = XmlSerialization.DeserializeXml<Gen.Accessibility>(
                new FileInfo(appSettings.SettingsConfig.AccommodationsXMLPath));

            IList<AccessibilityResource> globalAccessibilityResources = generatedAccessibility.ToAccessibilityResources();
            IList<AccessibilityResourceFamily> accessibilityResourceFamilies = generatedAccessibility.ResourceFamily
                .Select(f => f.ToAccessibilityResourceFamily(globalAccessibilityResources))
                .ToList();
            
            IList<InteractionType> interactionTypes = LoadInteractionTypes();
            IList<ItemDigest> itemDigests = await LoadItemDigests(appSettings, accessibilityResourceFamilies, interactionTypes);

            SampleItemsContext context = new SampleItemsContext
            {
                AccessibilityResourceFamilies = accessibilityResourceFamilies,
                GlobalAccessibilityResources = globalAccessibilityResources,
                InteractionTypes = interactionTypes,
                ItemDigests = itemDigests,
                AppSettings = appSettings
            };

            return context;
        }

        private static async Task<IList<ItemDigest>> LoadItemDigests(
            AppSettings settings,
            IList<AccessibilityResourceFamily> accessibilityResourceFamilies,
            IList<InteractionType> interactionTypes)
        {
            string contentDir = settings.SettingsConfig.ContentItemDirectory;

            //Find xml files
            Task<IEnumerable<FileInfo>> fetchMetadataFiles = XmlSerialization.FindMetadataXmlFiles(contentDir);
            Task<IEnumerable<FileInfo>> fetchContentsFiles = XmlSerialization.FindContentXmlFiles(contentDir);
            IEnumerable<FileInfo> metadataFiles = await fetchMetadataFiles;
            IEnumerable<FileInfo> contentsFiles = await fetchContentsFiles;

            //Parse Xml Files
            Task<IEnumerable<ItemMetadata>> deserializeMetadata = XmlSerialization.DeserializeXmlFilesAsync<ItemMetadata>(metadataFiles);
            Task<IEnumerable<ItemContents>> deserializeContents = XmlSerialization.DeserializeXmlFilesAsync<ItemContents>(contentsFiles);
            IEnumerable<ItemMetadata> itemMetadata = await deserializeMetadata;
            IEnumerable<ItemContents> itemContents = await deserializeContents;


            IList<ItemDigest> itemDigests = ItemDigestTranslation
                .ItemsToItemDigests(
                    itemMetadata,
                    itemContents,
                    accessibilityResourceFamilies,
                    interactionTypes)
                .ToList();

            return itemDigests;
        }

        // TODO: Get from XML
        private static IList<InteractionType> LoadInteractionTypes()
        {
            return new List<InteractionType>
            {
                new InteractionType
                {
                    Code = "GI",
                    Label = "Grid Item"
                },
                new InteractionType
                {
                    Code = "MI",
                    Label = "Match Interaction"
                },
                new InteractionType
                {
                    Code = "HTQ",
                    Label = "Hot Text"
                },
                new InteractionType
                {
                    Code = "MC",
                    Label = "Multiple Choice"
                },
                new InteractionType
                {
                    Code = "WER",
                    Label = "Writing Extended Response"
                },
                new InteractionType
                {
                    Code = "SA",
                    Label = "Short Answer"
                },
                new InteractionType
                {
                    Code = "MS",
                    Label = "Multi-Select"
                },
                new InteractionType
                {
                    Code = "EBSR",
                    Label = "Evidence Based Selected Response"
                },
                new InteractionType
                {
                    Code = "TI",
                    Label = "Table Interaction"
                },
                new InteractionType
                {
                    Code = "ER",
                    Label = "Extended Response"
                },
                new InteractionType
                {
                    Code = "EQ",
                    Label = "Equation"
                }
            };
        }
    }
}
