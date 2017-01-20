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

        public static SampleItemsContext LoadContext(AppSettings appSettings, ILogger logger)
        {
            string contentDir = appSettings.SettingsConfig.ContentItemDirectory;
            
            var metaDataFiles = XmlSerialization.FindMetadataXmlFiles(contentDir);
            var contentFiles = XmlSerialization.FindContentXmlFiles(contentDir);

            var interactionTypeDoc = XmlSerialization.GetXDocumentElement(appSettings.SettingsConfig.InteractionTypesXMLPath, "InteractionTypes");
            var accessibilityDoc = XmlSerialization.GetXDocumentElement(appSettings.SettingsConfig.AccommodationsXMLPath, "Accessibility");
            var subjectDoc = XmlSerialization.GetXDocument(appSettings.SettingsConfig.ClaimsXMLPath);
            
            var accessibilityResourceFamilies = LoadAccessibility(accessibilityDoc);

            ImmutableArray<InteractionType> interactionTypes = interactionTypeDoc
                .Element("Items")
                .Elements("Item")
                .Select(i => i.ToInteractionType())
                .ToImmutableArray();

            ImmutableArray<InteractionFamily> interactionFamilies = interactionTypeDoc
                .Element("Families")
                .Elements("Family")
                .Select(e => e.ToInteractionFamily())
                .ToImmutableArray();

            // TODO: consider using static Subject.Create or additional constructors instead of extension methods on XElement for organization's sake
            var subjects = subjectDoc.ToSubjects(interactionFamilies);

            // TODO: ItemDigests need everything in the universe. any way to separate the parts?
            var itemDigests = LoadItemDigests(
                appSettings,
                accessibilityResourceFamilies,
                interactionTypes,
                subjects,
                metaDataFiles,
                contentFiles,
                appSettings).Result;

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
    }
}
