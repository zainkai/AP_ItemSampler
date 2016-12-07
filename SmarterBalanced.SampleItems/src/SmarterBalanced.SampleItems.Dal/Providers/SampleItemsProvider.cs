using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SmarterBalanced.SampleItems.Dal.Xml;
using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;

namespace SmarterBalanced.SampleItems.Dal.Providers
{
    public static class SampleItemsProvider
    {
        public static async Task<SampleItemsContext> LoadContext(AppSettings appSettings)
        {
            // TODO:
            //      - Refactor to separate method.
            //      - Don't need Global Acccessibility in context
            XElement accessibilityXml = XDocument
                .Load(appSettings.SettingsConfig.AccommodationsXMLPath)
                .Element("Accessibility");

            List<AccessibilityResource> globalResources = accessibilityXml
                                                          .Element("MasterResourceFamily")
                                                          .Elements("SingleSelectResource")
                                                          .ToAccessibilityResources().ToList();

            IList<AccessibilityResourceFamily> accessibilityResourceFamilies = accessibilityXml
                                                          .Elements("ResourceFamily")
                                                          .ToAccessibilityResourceFamilies(globalResources).ToList();


            // TODO: Refactor to method. is List<InteractionType> needed in the context?
            XElement interactionTypesDoc = XDocument
                .Load(appSettings.SettingsConfig.InteractionTypesXMLPath)
                .Element("InteractionTypes");
            IList<InteractionType> interactionTypes = interactionTypesDoc.Element("Items").Elements("Item").ToInteractionTypes();
            IList<InteractionFamily> interactionFamily = interactionTypesDoc.ToInteractionFamilies(interactionTypes);

            IList<ItemDigest> itemDigests = await LoadItemDigests(appSettings, accessibilityResourceFamilies, interactionTypes);

            // TODO: get actual claims
            IList<Claim> claims = new List<Claim>
            {
                new Claim { Code = "MATH1", Label = "Concepts and Procedures" },
                new Claim { Code = "MATH2", Label = "Problem Solving and Modeling" },
                new Claim { Code = "MATH3", Label = "Communicating Reasoning" },
                new Claim { Code = "MATH4", Label = "Data Analysis" },
                new Claim { Code = "ELA1", Label = "Reading" },
                new Claim { Code = "ELA2", Label = "Writing" },
                new Claim { Code = "ELA3", Label = "Listening" },
                new Claim { Code = "ELA4", Label = "Research/Inquiry" }
            };

            SampleItemsContext context = new SampleItemsContext
            {
                AccessibilityResourceFamilies = accessibilityResourceFamilies,
                GlobalAccessibilityResources = globalResources, // TODO: Remove with global accessibility refactor
                InteractionTypes = interactionTypes,
                ItemDigests = itemDigests,
                AppSettings = appSettings,
                Claims = claims
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

    }
}
