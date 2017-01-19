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
            IList<InteractionType> interactionTypes, IList<Subject> subjects, RubricPlaceHolderText rubricPlaceHolder,
            AppSettings settings)
        {
            BlockingCollection<ItemDigest> digests = new BlockingCollection<ItemDigest>();
            Parallel.ForEach(itemMetadata, metadata =>
            {
                var matchingItems = itemContents.Where(c => c.Item.ItemKey == metadata.Metadata.ItemKey);
                var itemsCount = matchingItems.Count();

                if (itemsCount == 1)
                {
                    ItemDigest itemDigest = ItemToItemDigest(metadata, matchingItems.First(), interactionTypes, subjects, rubricPlaceHolder);

                    AssignAccessibilityResourceGroups(itemDigest, resourceFamilies, settings);

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
                                    IList<Subject> subjects, RubricPlaceHolderText rubricPlaceHolder)
        {
            var rubrics = itemContents.Item.Contents.Select(c => c.ToRubric(rubricPlaceHolder)).Where(r => r != null).ToImmutableArray();
            return ItemToItemDigest(itemMetadata, itemContents, interactionTypes, subjects, rubrics);
        }

        /// <summary>
        /// Translate an ItemMetadata and ItemContents object into an ItemDigest object.
        /// The ItemKey field of the ItemMetadata and ItemContents must be the same.
        /// </summary>
        private static ItemDigest ItemToItemDigest(ItemMetadata itemMetadata,
                                    ItemContents itemContents, IList<InteractionType> interactionTypes,
                                    IList<Subject> subjects, ImmutableArray<Rubric> rubrics)
        {

            StandardIdentifier identifier = itemMetadata.ToStandardIdentifier(itemContents);
            string subjectId = itemMetadata.Metadata.Subject;
            var subject = subjects.FirstOrDefault(s => s.Code == subjectId);
            string interactionTypeCode = itemMetadata.Metadata.InteractionType;
            var interactiontype = interactionTypes.FirstOrDefault(t => t.Code == interactionTypeCode);

            return ToItemDigest(itemMetadata, itemContents, identifier, subject, interactiontype, rubrics);
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
                                                InteractionType interactionType, ImmutableArray<Rubric> rubrics)
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
                TargetId = identifier.ToTargetId(),
                Claim = subject?.Claims.FirstOrDefault(t => t.ClaimNumber == identifier.ToClaimId()),
                CommonCoreStandardsId = identifier.CommonCoreStandard,
                Grade = GradeLevelsUtils.FromString(itemMetadata.Metadata.Grade),
                AslSupported = itemMetadata.Metadata.AccessibilityTagsASLLanguage == "Y" ? true : false,
                AllowCalculator = itemMetadata.Metadata.AllowCalculator == "Y" ? true : false
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
        public static Rubric ToRubric(this Content content, RubricPlaceHolderText placeholder)
        {
            if (placeholder == null) { throw new ArgumentNullException(nameof(placeholder)); }

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

            var rubric = new Rubric(content.Language, rubricEntries, samples);
            return rubric;
        }

        /// <summary>
        /// Assigns a list of AccessibilityResources to an item digest.
        /// If the item has auxilliary resources disabled, the resources are updated accordingly.
        /// </summary>
        private static void AssignAccessibilityResourceGroups(
            ItemDigest itemDigest,
            IList<AccessibilityResourceFamily> resourceFamilies,
            AppSettings settings)
        {
            List<AccessibilityResource> resources = resourceFamilies
                .FirstOrDefault(t => t.Subjects.Any(c => c == itemDigest.Subject?.Code)
                    && t.Grades.Contains(itemDigest.Grade)
                )?.Resources;

            if (resources == null)
            {
                return;
            }

            if (!itemDigest.AslSupported || !itemDigest.AllowCalculator)
            {
                resources = resources.Select(t => t.DeepClone()).ToList();

                if (!itemDigest.AslSupported)
                {
                    DisableResource(resources, "AmericanSignLanguage");
                }

                if (!itemDigest.AllowCalculator)
                {
                    DisableResource(resources, "Calculator");
                }
            }

            List<AccessibilityResourceGroup> groups = new List<AccessibilityResourceGroup>();
            foreach(AccessibilityType type in settings.SettingsConfig.AccessibilityTypes)
            {
                var groupResources = resources.Where(r => r.ResourceType == type.Id).OrderBy(r => r.Order);
                groups.Add(new AccessibilityResourceGroup(type.Label, type.Order, groupResources.ToImmutableArray()));
            }

            itemDigest.AccessibilityResourceGroups = groups.OrderBy(g => g.Order).ToImmutableArray();
        }

        /// <summary>
        /// Disables a given resource
        /// </summary>
        /// <param name="resource"></param>
        private static void DisableResource(List<AccessibilityResource> resources, string code)
        {
            var resource = resources.FirstOrDefault(t => t.Code == code);
            if (resource != null)
            {
                resource.Disabled = true;
                resource.Selections.ForEach(s => s.Disabled = true);
            }
        }

    }

}
