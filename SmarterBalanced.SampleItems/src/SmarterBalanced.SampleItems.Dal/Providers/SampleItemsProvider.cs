using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SmarterBalanced.SampleItems.Dal.Xml;
using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using Microsoft.Extensions.Logging;

namespace SmarterBalanced.SampleItems.Dal.Providers
{
    public static class SampleItemsProvider
    {

        public static async Task<SampleItemsContext> LoadContext(AppSettings appSettings, ILogger logger)
        {
            List<InteractionType> interactionTypes;
            List<InteractionFamily> interactionFamily;

            Task<XElement> interactionTypeDoc = XmlSerialization.GetXDocumentElementAsync(appSettings.SettingsConfig.InteractionTypesXMLPath, "InteractionTypes");
            Task<XElement> accessibilityDoc = XmlSerialization.GetXDocumentElementAsync(appSettings.SettingsConfig.AccommodationsXMLPath, "Accessibility");
            Task<XDocument> subjectDoc = XmlSerialization.GetXDocumentAsync(appSettings.SettingsConfig.ClaimsXMLPath);

            IList<AccessibilityResourceFamily> accessibilityResourceFamily = LoadAccessibility(accessibilityDoc.Result);
            GetInteractionTypes(interactionTypeDoc.Result, out interactionTypes, out interactionFamily);
            List<Subject> subjects = subjectDoc.Result.ToSubjects(interactionFamily);

            List<ItemDigest> itemDigests = await LoadItemDigests(appSettings, accessibilityResourceFamily, interactionTypes, subjects);
            List<ItemCardViewModel> itemCards = itemDigests.Select(i => i.ToItemCardViewModel()).ToList();
            SampleItemsContext context = new SampleItemsContext
            {
                AccessibilityResourceFamilies = accessibilityResourceFamily,
                InteractionTypes = interactionTypes,
                ItemDigests = itemDigests,
                ItemCards = itemCards,
                AppSettings = appSettings,
                Subjects = subjects
            };

            logger.LogInformation($"Loaded {itemDigests.Count()} item digests");
            logger.LogInformation($"Context loaded successfully");

            return context;
        }

        private static async Task<List<ItemDigest>> LoadItemDigests(
            AppSettings settings,
            IList<AccessibilityResourceFamily> accessibilityResourceFamilies,
            IList<InteractionType> interactionTypes,
            IList<Subject> subjects)
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

            var itemDigests = ItemDigestTranslation
                .ItemsToItemDigests(
                    itemMetadata,
                    itemContents,
                    accessibilityResourceFamilies,
                    interactionTypes,
                    subjects)
                .Where(i => i.Grade != GradeLevels.NA).ToList();

            return itemDigests;
        }

        private static IList<AccessibilityResourceFamily> LoadAccessibility(XElement accessibilityXml)
        {

            List<AccessibilityResource> globalResources = accessibilityXml.Element("MasterResourceFamily")
                                                          .Elements("SingleSelectResource").ToAccessibilityResources().ToList();
            return accessibilityXml.Elements("ResourceFamily")
                     .ToAccessibilityResourceFamilies(globalResources).ToList();
        }

        private static void GetInteractionTypes(XElement interactionTypesDoc, out List<InteractionType> interactionTypes, out List<InteractionFamily> interactionFamily)
        {
            interactionTypes = interactionTypesDoc.Element("Items").Elements("Item").ToInteractionTypes();
            interactionFamily = interactionTypesDoc.ToInteractionFamilies();
        }
    }
}
