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
            string contentDir = appSettings.SettingsConfig.ContentItemDirectory;
            List<InteractionType> interactionTypes;
            List<InteractionFamily> interactionFamilies;


            Task<IEnumerable<FileInfo>> metaDataFilesTask = XmlSerialization.FindMetadataXmlFiles(contentDir);
            Task<IEnumerable<FileInfo>> contentFilesTask = XmlSerialization.FindContentXmlFiles(contentDir);
            Task<XElement> interactionTypeDoc = XmlSerialization.GetXDocumentElementAsync(appSettings.SettingsConfig.InteractionTypesXMLPath, "InteractionTypes");
            Task<XElement> accessibilityDoc = XmlSerialization.GetXDocumentElementAsync(appSettings.SettingsConfig.AccommodationsXMLPath, "Accessibility");
            Task<XDocument> subjectDoc = XmlSerialization.GetXDocumentAsync(appSettings.SettingsConfig.ClaimsXMLPath);

            IList<AccessibilityResourceFamily> accessibilityResourceFamilies = LoadAccessibility(accessibilityDoc.Result, appSettings);
            GetInteractionTypes(interactionTypeDoc.Result, out interactionTypes, out interactionFamilies);
            List<Subject> subjects = subjectDoc.Result.ToSubjects(interactionFamilies);

            List<ItemDigest> itemDigests = await LoadItemDigests(appSettings, accessibilityResourceFamilies, interactionTypes,
                                                                subjects, metaDataFilesTask.Result, contentFilesTask.Result, appSettings.RubricPlaceHolderText);

            List<ItemCardViewModel> itemCards = itemDigests.Select(i => i.ToItemCardViewModel()).ToList();
            SampleItemsContext context = new SampleItemsContext
            {
                AccessibilityResourceFamilies = accessibilityResourceFamilies,
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
            IList<Subject> subjects,
            IEnumerable<FileInfo> metaDataFilesTask,
            IEnumerable<FileInfo> contentFilesTask,
            RubricPlaceHolderText rubricPlaceHolder)
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
                    rubricPlaceHolder)
                .Where(i => i.Grade != GradeLevels.NA).ToList();

            return itemDigests;
        }

        private static IList<AccessibilityResourceFamily> LoadAccessibility(XElement accessibilityXml, AppSettings appSettings)
        {

            List<AccessibilityResource> globalResources = accessibilityXml.Element("MasterResourceFamily")
                                                          .Elements("SingleSelectResource").ToAccessibilityResources(appSettings).ToList();
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
