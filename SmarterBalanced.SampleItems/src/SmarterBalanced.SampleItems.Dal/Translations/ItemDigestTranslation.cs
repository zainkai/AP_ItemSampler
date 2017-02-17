using SmarterBalanced.SampleItems.Dal.Exceptions;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    /// <summary>
    /// Contains methods to digest ItemMetadata and ItemContents objects into ItemDigest objects.
    /// </summary>
    public static class ItemDigestTranslation
    {
        /// <summary>
        /// Digests a collection of ItemMetadata objects and a collection of ItemContents objects into a collection of ItemDigest objects.
        /// Matches the ItemMetadata and ItemContents objects based on their ItemKey fields.
        /// </summary>
        public static IEnumerable<ItemDigest> ItemsToItemDigests(IEnumerable<ItemMetadata> itemMetadata,
            IEnumerable<ItemContents> itemContents, IList<AccessibilityResourceFamily> resourceFamilies,
            IList<InteractionType> interactionTypes, IList<Subject> subjects,
             CoreStandardsXml standardsXml,
            AppSettings settings)
        {
            BlockingCollection<ItemDigest> digests = new BlockingCollection<ItemDigest>();
            Parallel.ForEach(itemMetadata, metadata =>
            {
                var matchingItems = itemContents.Where(c => c.Item.ItemKey == metadata.Metadata.ItemKey);
                var itemsCount = matchingItems.Count();

                if (itemsCount == 1)
                {
                    ItemDigest itemDigest = ItemToItemDigest(metadata, matchingItems.First(), interactionTypes, subjects, standardsXml, settings);

                    itemDigest.AccessibilityResourceGroups =
                        CreateAccessibilityGroups(itemDigest, resourceFamilies, settings.SettingsConfig.AccessibilityTypes);

                    digests.Add(itemDigest);
                }
                else if (itemsCount > 1)
                {
                    throw new SampleItemsContextException("Multiple ItemContents with ItemKey: " + metadata.Metadata.ItemKey + " found.");
                }
                // TODO: log a warning if item count is 0
            });

            return digests;
        }


        /// <summary>
        /// Translate an ItemMetadata and ItemContents object into an ItemDigest object.
        /// The ItemKey field of the ItemMetadata and ItemContents must be the same.
        /// </summary>
        public static ItemDigest ItemToItemDigest(ItemMetadata itemMetadata,
                                    ItemContents itemContents, IList<InteractionType> interactionTypes,
                                    IList<Subject> subjects, CoreStandardsXml standardsXml, AppSettings appSettings)
        {
            var rubrics = itemContents.Item.Contents.Select(c => c.ToRubric(appSettings)).Where(r => r != null).ToImmutableArray();
            return ItemToItemDigest(itemMetadata, itemContents, interactionTypes, subjects, rubrics, standardsXml);
        }

        /// <summary>
        /// Translate an ItemMetadata and ItemContents object into an ItemDigest object.
        /// The ItemKey field of the ItemMetadata and ItemContents must be the same.
        /// </summary>
        private static ItemDigest ItemToItemDigest(ItemMetadata itemMetadata,
                                    ItemContents itemContents, IList<InteractionType> interactionTypes,
                                    IList<Subject> subjects, ImmutableArray<Rubric> rubrics,
                                    CoreStandardsXml standardsXml)
        {

            string subjectId = itemMetadata.Metadata.Subject;
            var subject = subjects.FirstOrDefault(s => s.Code == subjectId);
            StandardIdentifier identifier = itemMetadata.ToStandardIdentifier(itemContents);
            string interactionTypeCode = itemMetadata.Metadata.InteractionType;
            var interactiontype = interactionTypes.FirstOrDefault(t => t.Code == interactionTypeCode);

            //TODO: fix standards
            CoreStandards coreStandards = CoreStandardFromIdentififer(standardsXml, identifier);
          

            return ToItemDigest(itemMetadata, itemContents, identifier, subject, interactiontype, rubrics, coreStandards);
        }

        public static CoreStandards CoreStandardFromIdentififer(CoreStandardsXml standardsXml, StandardIdentifier itemIdentifier)
        {

            //get target match
            CoreStandardsRow targetRow = null;
            foreach(CoreStandardsRow row in standardsXml.TargetRows)
            {
                var rowIdentifier = row.StandardIdentifier;
                if(StandardIdentifierTargetComparer.Instance.Equals(rowIdentifier, itemIdentifier))
                {
                    targetRow = row;
                }
            }



            //get ccss match

            CoreStandardsRow ccssRow = null;
            foreach (CoreStandardsRow row in standardsXml.CcssRows)
            {
                var rowIdentifier = row.StandardIdentifier;
                if (StandardIdentifierCcssComparer.Instance.Equals(rowIdentifier, itemIdentifier))
                {
                    ccssRow = row;
                }
            }


            return CoreStandards.Create(
                  targetId: itemIdentifier.ToTargetId(),
                  commonCoreStandardsId: itemIdentifier.CommonCoreStandard);

        }

        /// <summary>
        /// Gets the standard identifier from the given item metadata
        /// </summary>
        private static StandardIdentifier ToStandardIdentifier(this ItemMetadata itemMetadata, ItemContents itemContents)
        {
            try
            {
                var identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(
                    itemMetadata.Metadata.StandardPublications.First().PrimaryStandard);
                return identifier;
            }
            catch (InvalidOperationException ex)
            {
                throw new SampleItemsContextException(
                    $"Publication field for item {itemContents.Item.ItemBank}-{itemMetadata.Metadata.ItemKey} is empty.", ex);
            }
        }


        /// <summary>
        /// Translates metadata, itemcontents and lookups to item digest
        /// </summary>
        public static ItemDigest ToItemDigest(ItemMetadata itemMetadata, ItemContents itemContents,
                                                StandardIdentifier identifier, Subject subject,
                                                InteractionType interactionType, ImmutableArray<Rubric> rubrics, CoreStandards coreStandards)
        {
            if (itemMetadata == null) { throw new ArgumentNullException(nameof(itemMetadata)); }
            if (itemMetadata.Metadata == null) { throw new ArgumentNullException(nameof(itemMetadata.Metadata)); }
            if (itemContents == null) { throw new ArgumentNullException(nameof(itemContents)); }
            if (itemContents.Item == null) { throw new ArgumentNullException(nameof(itemContents.Item)); }

            if (itemContents.Item.ItemKey != itemMetadata.Metadata.ItemKey)
            {
                throw new SampleItemsContextException("Cannot digest items with different ItemKey values.\n"
                    + $"Content Item Key: {itemContents.Item.ItemKey} Metadata Item Key:{itemMetadata.Metadata.ItemKey}");
            }

            ItemDigest digest = new ItemDigest()
            {
                ItemType = itemContents.Item.ItemType,
                ItemKey = itemContents.Item.ItemKey,
                BankKey = itemContents.Item.ItemBank,
                Rubrics = rubrics,
                TargetAssessmentType = itemMetadata.Metadata.TargetAssessmentType,
                InteractionType = interactionType,
                SufficentEvidenceOfClaim = itemMetadata.Metadata.SufficientEvidenceOfClaim,
                AssociatedStimulus = itemMetadata.Metadata.AssociatedStimulus,
                Subject = subject,
                Claim = subject?.Claims.FirstOrDefault(t => t.ClaimNumber == identifier.ToClaimId()),
                Grade = GradeLevelsUtils.FromString(itemMetadata.Metadata.Grade),
                AslSupported = itemMetadata.Metadata.AccessibilityTagsASLLanguage == "Y",
                AllowCalculator = itemMetadata.Metadata.AllowCalculator == "Y",
                DepthOfKnowledge = itemMetadata.Metadata.DepthOfKnowledge,
                CoreStandards = coreStandards
            };

            return digest;
        }

        /// <summary>
        /// Translates the standard identifier to claim id 
        /// </summary>
        private static string ToClaimId(this StandardIdentifier identifier)
        {
            return (string.IsNullOrEmpty(identifier.Claim)) ? string.Empty : identifier.Claim.Split('-').FirstOrDefault();
        }

        /// <summary>
        /// Translates the standard identifier to target id
        /// </summary>
        private static string ToTargetId(this StandardIdentifier identifier)
        {
            return (string.IsNullOrEmpty(identifier.Target)) ? string.Empty : identifier.Target.Split('-').FirstOrDefault();
        }

        /// <summary>
        /// Returns a Single Rubric from content and filters out any placeholder text
        /// </summary>
        public static Rubric ToRubric(this Content content, AppSettings appSettings)
        {
            if (appSettings == null || appSettings.RubricPlaceHolderText == null || appSettings.SettingsConfig == null)
                { throw new ArgumentNullException(nameof(appSettings)); }

            var placeholder = appSettings.RubricPlaceHolderText;
            var languageToLabel = appSettings.SettingsConfig.LanguageToLabel;

            if (content == null ||
                content.RubricList == null ||
                content.RubricList.Rubrics == null ||
                content.RubricList.RubricSamples == null)
            {
                return null;
            }


            var rubricEntries = content.RubricList.Rubrics.Where(r => !string.IsNullOrWhiteSpace(r.Value)
                                                                   && !placeholder.RubricPlaceHolderContains.Any(s => r.Value.Contains(s))
                                                                   && !placeholder.RubricPlaceHolderEquals.Any(s => r.Value.Equals(s))).ToImmutableArray();

           Predicate<SampleResponse> pred = (r => string.IsNullOrWhiteSpace(r.SampleContent)
                                                    || placeholder.RubricPlaceHolderContains.Any(s => r.SampleContent.Contains(s))
                                                    || placeholder.RubricPlaceHolderEquals.Any(s => r.SampleContent.Equals(s)));

            content.RubricList.RubricSamples.ForEach(t => t.SampleResponses.RemoveAll(pred));

            var samples = content.RubricList.RubricSamples.Where(t => t.SampleResponses.Count() > 0).ToImmutableArray();
            if (rubricEntries.Length == 0 && samples.Length == 0)
            {
                return null;
            }

            string languangeLabel = (string.IsNullOrEmpty(content.Language)) ? string.Empty: 
                                                languageToLabel[content.Language.ToUpper()];

            var rubric = new Rubric(languangeLabel, rubricEntries, samples);
            return rubric;
        }

        private static AccessibilityResource ApplyFlags(
            this AccessibilityResource resource,
            bool aslSupported,
            bool allowCalculator)
        {
            if (!aslSupported && resource.Code == "AmericanSignLanguage")
            {
                var newResource = resource.ToDisabled();
                return newResource;
            }

            if (!allowCalculator && resource.Code == "Calculator")
            {
                var newResource = resource.ToDisabled();
                return newResource;
            }
            
            return resource;
        } 
        
        private static ImmutableArray<AccessibilityResourceGroup> CreateAccessibilityGroups(
            ItemDigest itemDigest,
            IList<AccessibilityResourceFamily> resourceFamilies,
            IList<AccessibilityType> accessibilityTypes)
        {
            var family = resourceFamilies
                .FirstOrDefault(f =>
                    f.Subjects.Any(c => c == itemDigest.Subject?.Code) &&
                    f.Grades.Contains(itemDigest.Grade));

            if (family == null)
            {
                return ImmutableArray<AccessibilityResourceGroup>.Empty;
            }

            var flaggedResources = family.Resources
                .Select(r => r.ApplyFlags(
                    aslSupported: itemDigest.AslSupported,
                    allowCalculator: itemDigest.AllowCalculator))
                .ToImmutableArray();

            var groups = accessibilityTypes
                .Select(at =>
                {
                    var groupResources = flaggedResources
                    .Where(r => r.ResourceType == at.Id)
                    .ToImmutableArray();

                    var group = new AccessibilityResourceGroup(
                        label: at.Label,
                        order: at.Order,
                        accessibilityResources: groupResources);

                    return group;
                })
                .ToImmutableArray();

            return groups;
        }
    }
}
