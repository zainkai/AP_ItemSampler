using SmarterBalanced.SampleItems.Dal.Exceptions;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using XmlModels = SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    /// <summary>
    /// Contains methods to digest ItemMetadata and ItemContents objects into ItemDigest objects.
    /// </summary>
    public static class ItemDigestTranslation
    {
        /// <summary>
        /// Translate an ItemMetadata and ItemContents object into an ItemDigest object.
        /// The ItemKey field of the ItemMetadata and ItemContents must be the same.
        /// </summary>
        /// <param name="itemMetadata"></param>
        /// <param name="itemContents"></param>
        /// <returns></returns>
        public static ItemDigest ItemToItemDigest(
            ItemMetadata itemMetadata,
            ItemContents itemContents,
            IList<InteractionType> interactionTypes,
            IList<Subject> subjects
            )
        {
            if(itemMetadata?.Metadata == null)
                throw new ArgumentNullException(nameof(itemMetadata.Metadata));

            if (itemContents?.Item == null)
                throw new ArgumentNullException(nameof(itemContents.Item));

            if (itemContents.Item.ItemKey != itemMetadata.Metadata.ItemKey)
            {
                throw new SampleItemsContextException("Cannot digest items with different ItemKey values.\n"
                    + $"Content Item Key: {itemContents.Item.ItemKey} Metadata Item Key:{itemMetadata.Metadata.ItemKey}");
            }

            XmlModels.StandardIdentifier identifier;
            try
            {
                identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(
                    itemMetadata.Metadata.StandardPublications.First().PrimaryStandard);
            }
            catch (InvalidOperationException ex)
            {
                throw new SampleItemsContextException(
                    $"Publication field for item {itemContents.Item.ItemBank}-{itemContents.Item.ItemKey} is empty.", ex);
            }

            ItemDigest digest = new ItemDigest();
            digest.BankKey = itemContents.Item.ItemBank;
            digest.ItemKey = itemContents.Item.ItemKey;
            digest.ItemType = itemContents.Item.ItemType;
            
            digest.Rubrics = itemContents.Item.Contents.ToRubric();

            digest.TargetAssessmentType = itemMetadata.Metadata.TargetAssessmentType; 
            
            string interactionTypeCode = itemMetadata.Metadata.InteractionType;
            digest.InteractionType = interactionTypes.FirstOrDefault(t => t.Code == interactionTypeCode);

            digest.SufficentEvidenceOfClaim = itemMetadata.Metadata.SufficientEvidenceOfClaim;
            digest.AssociatedStimulus = itemMetadata.Metadata.AssociatedStimulus;

            string subjectId = itemMetadata.Metadata.Subject;
            digest.Subject = subjects.FirstOrDefault(s => s.Code == subjectId);

            digest.TargetId = (string.IsNullOrEmpty(identifier.Target)) ? string.Empty : identifier.Target.Split('-').FirstOrDefault();
            //TODO: do we need claim with identifier? 3-L?
            string claimId = (string.IsNullOrEmpty(identifier.Claim)) ? string.Empty : identifier.Claim.Split('-').FirstOrDefault();
            digest.Claim = digest.Subject?.Claims.FirstOrDefault(t => t.ClaimNumber == claimId);

            digest.CommonCoreStandardsId = identifier.CommonCoreStandard;

            digest.Grade = GradeLevelsUtils.FromString(itemMetadata.Metadata.Grade);

            digest.AslSupported = itemMetadata.Metadata.AccessibilityTagsASLLanguage == "Y" ? true : false;
            digest.AllowCalculator = itemMetadata.Metadata.AllowCalculator == "Y" ? true : false;

            return digest;
        }

        /// <summary>
        /// Converts item XML Contents into Rubrics.
        /// </summary>
        /// <param name="contents"></param>
        /// <returns>List<Rubric>.</returns>
        public static List<Rubric> ToRubric(this List<XmlModels.Content> contents)
        {
            List<Rubric> rubrics = new List<Rubric>();
            if(contents == null)
            {
                return rubrics;
            }

            foreach(var content in contents)
            {
                var rubricEntries = content?.RubricList?.Rubrics?.Where(r => !string.IsNullOrWhiteSpace(r.Value)).ToList();
                var samples = content?.RubricList?.RubricSamples?.Where(r => r.SampleResponses.Count() > 0).ToList();

                if(rubricEntries?.Count() > 0 || samples?.Count() > 0)
                {
                    rubrics.Add(
                        new Rubric
                        {
                            Language = content?.Language,
                            RubricEntries = rubricEntries,
                            Samples = samples
                        });
                }
            }

            return rubrics;
        }

        /// <summary>
        /// Digests a collection of ItemMetadata objects and a collection of ItemContents objects into a collection of ItemDigest objects.
        /// Matches the ItemMetadata and ItemContents objects based on their ItemKey fields.
        /// </summary>
        /// <param name="itemMetadata"></param>
        /// <param name="itemContents"></param>
        /// <returns></returns>
        public static IEnumerable<ItemDigest> ItemsToItemDigests(
            IEnumerable<ItemMetadata> itemMetadata,
            IEnumerable<ItemContents> itemContents,
            IList<AccessibilityResourceFamily> resourceFamilies,
            IList<InteractionType> interactionTypes,
            IList<Subject> subjects)
        {
            BlockingCollection<ItemDigest> digests = new BlockingCollection<ItemDigest>();
            Parallel.ForEach(itemMetadata, metadata =>
            {
                var matchingItems = itemContents.Where(c => c.Item.ItemKey == metadata.Metadata.ItemKey);
                var itemsCount = matchingItems.Count();

                if (itemsCount == 1)
                {
                    ItemDigest itemDigest = ItemToItemDigest(metadata, matchingItems.First(), interactionTypes, subjects);

                    AssignAccessibilityResources(itemDigest, resourceFamilies);

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
        /// Assigns a list of AccessibilityResources to an item digest.
        /// If the item has auxilliary resources disabled, the resources are updated accordingly.
        /// </summary>
        /// <param name="itemDigest"></param>
        /// <param name="resourceFamilies"></param>
        private static void AssignAccessibilityResources(ItemDigest itemDigest, IList<AccessibilityResourceFamily> resourceFamilies)
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

            itemDigest.AccessibilityResources = resources;
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
