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
using SmarterBalanced.SampleItems.Dal.Xml.Models;

namespace SmarterBalanced.SampleItems.Dal.Providers
{
    public static class SampleItemsProvider
    {

        public static SampleItemsContext LoadContext(AppSettings appSettings, ILogger logger)
        {

            CoreStandardsXml standardsXml = LoadCoreStandards(appSettings.SettingsConfig.CoreStandardsXMLPath);
            var accessibilityResourceFamilies = LoadAccessibility(appSettings.SettingsConfig.AccommodationsXMLPath);
            var interactionGroup = LoadInteractionGroup(appSettings.SettingsConfig.InteractionTypesXMLPath);
            ImmutableArray<Subject> subjects = LoadSubjects(appSettings.SettingsConfig.ClaimsXMLPath, interactionGroup.InteractionFamilies);

            var itemDigests = LoadItemDigests(
                appSettings,
                accessibilityResourceFamilies,
                interactionGroup.InteractionTypes,
                subjects,
                standardsXml,
                appSettings).Result;

            var itemCards = itemDigests
                .Select(i => i.ToItemCardViewModel())
                .ToImmutableArray();

            SampleItemsContext context = new SampleItemsContext(
                itemDigests: itemDigests,
                itemCards: itemCards,
                interactionTypes: interactionGroup.InteractionTypes,
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
           CoreStandardsXml coreStandards,
            AppSettings appSettings)
        {
            string contentDir = appSettings.SettingsConfig.ContentItemDirectory;

            var metaDataFiles = XmlSerialization.FindMetadataXmlFiles(contentDir);
            var contentFiles = XmlSerialization.FindContentXmlFiles(contentDir);

            //Parse Xml Files
            IEnumerable<ItemMetadata> itemMetadata = await XmlSerialization.DeserializeXmlFilesAsync<ItemMetadata>(metaDataFiles);
           IEnumerable<ItemContents> itemContents = await XmlSerialization.DeserializeXmlFilesAsync<ItemContents>(contentFiles);

            var itemDigests = ItemDigestTranslation
                .ItemsToItemDigests(
                    itemMetadata,
                    itemContents,
                    accessibilityResourceFamilies,
                    interactionTypes,
                    subjects,
                    coreStandards,
                    appSettings)
                .Where(i => i.Grade != GradeLevels.NA)
                .ToImmutableArray();

            return itemDigests;
        }

        private static ImmutableArray<AccessibilityResourceFamily> LoadAccessibility(string accessibilityPath)
        {
            var accessibilityDoc = XmlSerialization.GetXDocument(accessibilityPath);
            var accessibilityXml = accessibilityDoc.Element("Accessibility");

            ImmutableArray<AccessibilityResource> globalResources = accessibilityXml
                .Element("MasterResourceFamily")
                .Elements("SingleSelectResource")
                .ToAccessibilityResources();

            return accessibilityXml
                .Elements("ResourceFamily")
                .ToAccessibilityResourceFamilies(globalResources);
        }

        private static CoreStandardsXml LoadCoreStandards(string targetFile)
        {
            var coreDoc = XmlSerialization.GetXDocument(targetFile);


            ImmutableArray<CoreStandardsRow> allRows = coreDoc.Element("Levels").Elements()
                                                .Select(t => CoreStandardsRow.Create(t))
                                                .ToImmutableArray();

            var ccss = allRows.Where(r => r.LevelType == "CCSS").ToImmutableArray();
            var targets = allRows.Where(r => r.LevelType == "Target").ToImmutableArray();

            return new CoreStandardsXml(
                targetRows: targets,
                ccssRows: ccss);
        }


        private static ImmutableArray<Subject> LoadSubjects(string subjectFile, ImmutableArray<InteractionFamily> interactionFamilies)
        {
            var subjectDoc = XmlSerialization.GetXDocument(subjectFile);
            var subjects = subjectDoc.Element("Subjects")
                .Elements("Subject")
                .Select(s => Subject.Create(s, interactionFamilies)).ToImmutableArray();
            return subjects;

        }

        private static InteractionGroup LoadInteractionGroup(string interactionTypesFile)
        {
            var interactionTypeDoc = XmlSerialization.GetXDocument(interactionTypesFile);
            var elementRoot = interactionTypeDoc.Element("InteractionTypes");
            var interactionTypes = LoadInteractionTypes(elementRoot);
            var interactionFamilies = LoadInteractionFamilies(elementRoot);

            return new InteractionGroup(interactionFamilies, interactionTypes);
        }

        private static ImmutableArray<InteractionFamily> LoadInteractionFamilies(XElement elementRoot)
        {

           var interactionFamilies = elementRoot
                            .Element("Families")
                            .Elements("Family")
                            .Select(e => InteractionFamily.Create(e))
                            .ToImmutableArray();
            return interactionFamilies;
        }

        private static ImmutableArray<InteractionType> LoadInteractionTypes(XElement elementRoot)
        {
            var interactionTypes = elementRoot
                            .Element("Items")
                            .Elements("Item")
                            .Select(i => InteractionType.Create(i))
                            .ToImmutableArray();

            return interactionTypes;
        }

    }
}
