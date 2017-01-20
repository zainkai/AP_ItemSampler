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
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Dal.Providers
{
    public static class SampleItemsProvider
    {

        public static async Task<SampleItemsContext> LoadContext(AppSettings appSettings, ILogger logger)
        {
            string contentDir = appSettings.SettingsConfig.ContentItemDirectory;
            ImmutableArray<InteractionType> interactionTypes;
            ImmutableArray<InteractionFamily> interactionFamilies;


            Task<IEnumerable<FileInfo>> metaDataFilesTask = XmlSerialization.FindMetadataXmlFiles(contentDir);
            Task<IEnumerable<FileInfo>> contentFilesTask = XmlSerialization.FindContentXmlFiles(contentDir);
            Task<XElement> interactionTypeDoc = XmlSerialization.GetXDocumentElementAsync(appSettings.SettingsConfig.InteractionTypesXMLPath, "InteractionTypes");
            Task<XElement> accessibilityDoc = XmlSerialization.GetXDocumentElementAsync(appSettings.SettingsConfig.AccommodationsXMLPath, "Accessibility");
            Task<XDocument> subjectDoc = XmlSerialization.GetXDocumentAsync(appSettings.SettingsConfig.ClaimsXMLPath);

            var accessibilityResourceFamilies = LoadAccessibility(accessibilityDoc.Result);

            GetInteractionTypes(interactionTypeDoc.Result, out interactionTypes, out interactionFamilies);
            var subjects = subjectDoc.Result.ToSubjects(interactionFamilies);

            // TODO: ItemDigests need everything in the universe. any way to separate the parts?
            var itemDigests = await LoadItemDigests(
                appSettings,
                accessibilityResourceFamilies,
                interactionTypes,
                subjects,
                metaDataFilesTask.Result,
                contentFilesTask.Result,
                appSettings);

            var itemCards = itemDigests
                .Select(i => i.ToItemCardViewModel())
                .ToImmutableArray();

            SampleItemsContext context = new SampleItemsContext(
                itemDigests: itemDigests,
                itemCards: itemCards,
                interactionTypes: interactionTypes,
                subjects: subjects,
                accessibilityResourceFamilies: accessibilityResourceFamilies,
                appSettings: appSettings);

            logger.LogInformation($"Loaded {itemDigests.Length} item digests");
            logger.LogInformation($"Context loaded successfully");

            return context;
        }

        private static async Task<ImmutableArray<ItemDigest>> LoadItemDigests(
            AppSettings settings,
            IList<AccessibilityResourceFamily> accessibilityResourceFamilies,
            IList<InteractionType> interactionTypes,
            IList<Subject> subjects,
            IEnumerable<FileInfo> metaDataFilesTask,
            IEnumerable<FileInfo> contentFilesTask,
            AppSettings appSettings)
        {
    
            //Parse Xml Files
            Task<IEnumerable<ItemMetadata>> itemMetadataTask = XmlSerialization.DeserializeXmlFilesAsync<ItemMetadata>(metaDataFilesTask);
            Task<IEnumerable<ItemContents>> itemContentsTask = XmlSerialization.DeserializeXmlFilesAsync<ItemContents>(contentFilesTask);

            var itemDigests = ItemDigestTranslation
                .ItemsToItemDigests(
                    await itemMetadataTask,
                    await itemContentsTask,
                    accessibilityResourceFamilies,
                    interactionTypes,
                    subjects,
                    appSettings)
                .Where(i => i.Grade != GradeLevels.NA)
                .ToImmutableArray();

            return itemDigests;
        }

        private static ImmutableArray<AccessibilityResourceFamily> LoadAccessibility(XElement accessibilityXml)
        {
            ImmutableArray<AccessibilityResource> globalResources = accessibilityXml
                .Element("MasterResourceFamily")
                .Elements("SingleSelectResource")
                .ToAccessibilityResources()
                .ToImmutableArray();

            return accessibilityXml
                .Elements("ResourceFamily")
                .ToAccessibilityResourceFamilies(globalResources)
                .ToImmutableArray();
        }

        private static void GetInteractionTypes(XElement interactionTypesDoc,
            out ImmutableArray<InteractionType> interactionTypes,
            out ImmutableArray<InteractionFamily> interactionFamily)
        {
            interactionTypes = interactionTypesDoc
                .Element("Items")
                .Elements("Item")
                .Select(i => i.ToInteractionType())
                .ToImmutableArray();

            interactionFamily = interactionTypesDoc
                .Element("Families")
                .Elements("Family")
                .Select(e => e.ToInteractionFamily())
                .ToImmutableArray();
        }
    }
}
