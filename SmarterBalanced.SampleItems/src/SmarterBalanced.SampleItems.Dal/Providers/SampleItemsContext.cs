using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Dal.Translations;
using System.IO;
using Gen = SmarterBalanced.SampleItems.Dal.Xml.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Aws.Provider;
using SmarterBalanced.SampleItems.Dal.Xml;

namespace SmarterBalanced.SampleItems.Dal.Providers
{
    public class SampleItemsContext
    {
        public virtual IList<ItemDigest> ItemDigests { get; set; }
        public virtual IList<InteractionType> InteractionTypes { get; set; }
        public virtual IList<AccessibilityResource> GlobalAccessibilityResources { get; set; }
        public virtual IList<AccessibilityResourceFamily> AccessibilityResourceFamilies { get; set; }

        private static bool ContentDownloaded = false;

        private static AppSettings settings;
        public static void RegisterSettings(AppSettings settings)
        {
            SampleItemsContext.settings = settings;
        }

        /// <summary>
        /// Downloads content package from Amazon S3 if "UseS3ForContent" is set to true in the appsettings
        /// </summary>
        /// <param name="settings"></param>
        public static void UpdateContent(AppSettings settings)
        {
            ContentProvider contentProvider = new ContentProvider(settings);
            contentProvider.UpdateContent().Wait();
            ContentDownloaded = true;
        }

        private static SampleItemsContext instance; 
        public static SampleItemsContext Default
        {
            get
            {
                if (instance == null)
                {
                    if (settings == null)
                        throw new InvalidOperationException("You need to register app settings before accessing the default instance.");
                    if (ContentDownloaded != true)
                        throw new InvalidOperationException("You need to call UpdateContent() before accessing the default instance.");
                    instance = new SampleItemsContext();
                }

                return instance;
            }
        }

        private SampleItemsContext()
        {
            SetAccessibility();
            SetInteractionTypes();
            SetItemDigests();
        }

        private void SetItemDigests()
        {
            if (AccessibilityResourceFamilies == null || InteractionTypes == null)
            {
                throw new Exception("Please load accessibility resources and interaction types");
            }
            string contentDir = settings.SettingsConfig.ContentItemDirectory;

            //Find xml files
            Task<IEnumerable<FileInfo>> fetchMetadataFiles = XmlSerialization.FindMetadataXmlFiles(contentDir);
            Task<IEnumerable<FileInfo>> fetchContentsFiles = XmlSerialization.FindContentXmlFiles(contentDir);
            IEnumerable<FileInfo> metadataFiles = fetchMetadataFiles.Result;
            IEnumerable<FileInfo> contentsFiles = fetchContentsFiles.Result;

            //Parse Xml Files
            Task<IEnumerable<ItemMetadata>> deserializeMetadata = XmlSerialization.DeserializeXmlFilesAsync<ItemMetadata>(metadataFiles);
            Task<IEnumerable<ItemContents>> deserializeContents = XmlSerialization.DeserializeXmlFilesAsync<ItemContents>(contentsFiles);
            IEnumerable<ItemMetadata> itemMetadata = deserializeMetadata.Result;
            IEnumerable<ItemContents> itemContents = deserializeContents.Result;


            ItemDigests = ItemDigestTranslation
                .ItemsToItemDigests(
                    itemMetadata,
                    itemContents,
                    AccessibilityResourceFamilies,
                    InteractionTypes)
                .ToList();
        }

        private void SetAccessibility()
        {

            Gen.Accessibility generatedAccessibility = XmlSerialization.DeserializeXml<Gen.Accessibility>(new FileInfo(settings.SettingsConfig.AccommodationsXMLPath));
            GlobalAccessibilityResources = generatedAccessibility.ToAccessibilityResources();

            AccessibilityResourceFamilies = generatedAccessibility.ResourceFamily
                .Select(f => f.ToAccessibilityResourceFamily(GlobalAccessibilityResources))
                .ToList();

        }
        // TODO: Get from XML
        private void SetInteractionTypes()
        {
            InteractionTypes = new List<InteractionType>
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

        public AppSettings AppSettings()
        {
            return settings;
        }
    }
}
