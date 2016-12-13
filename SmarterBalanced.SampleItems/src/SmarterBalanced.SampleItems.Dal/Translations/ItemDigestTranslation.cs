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
            IList<AccessibilityResourceFamily> resourceFamilies,
            IList<InteractionType> interactionTypes)
        {
            if(itemMetadata?.Metadata == null)
            {
                throw new ArgumentNullException(nameof(itemMetadata.Metadata));
            }

            if (itemContents?.Item == null)
            {
                throw new ArgumentNullException(nameof(itemContents.Item));
            }

            if (itemContents.Item.ItemKey != itemMetadata.Metadata.ItemKey)
            {
                throw new SampleItemsContextException("Cannot digest items with different ItemKey values.\n"
                    + $"Content Item Key: {itemContents.Item.ItemKey} Metadata Item Key:{itemMetadata.Metadata.ItemKey}");
            }
            XmlModels.StandardIdentifier identifier = null;
            try
            {
                identifier =
                    StandardIdentifierTranslation.StandardStringtoStandardIdentifier
                        (itemMetadata.Metadata.StandardPublications.First().PrimaryStandard);
            } catch(InvalidOperationException)
            {
                throw new SampleItemsContextException($"Publication field for item {itemContents.Item.ItemBank}-{itemContents.Item.ItemKey} is empty.");
            }


            ItemDigest digest = new ItemDigest();
            digest.BankKey = itemContents.Item.ItemBank;
            digest.ItemKey = itemContents.Item.ItemKey;
            digest.Grade = GradeLevelsUtils.FromString(itemMetadata.Metadata.Grade);
            digest.ItemType = itemContents.Item.ItemType;
            digest.TargetAssessmentType = itemMetadata.Metadata.TargetAssessmentType; 
            digest.Subject = itemMetadata.Metadata.Subject;
            digest.InteractionTypeCode = itemMetadata.Metadata.InteractionType;
            digest.SufficentEvidenceOfClaim = itemMetadata.Metadata.SufficientEvidenceOfClaim;
            digest.AssociatedStimulus = itemMetadata.Metadata.AssociatedStimulus;
            digest.AccessibilityResources = resourceFamilies.FirstOrDefault(t => t.Subjects.Any(c => c == digest.Subject) && t.Grades.Contains(digest.Grade))?.Resources;
            digest.InteractionTypeLabel = interactionTypes.FirstOrDefault(t => t.Code == digest.InteractionTypeCode)?.Label;
            digest.Name = $"{digest.Subject} {digest.Grade.ToString()} {digest.InteractionTypeCode}";
            digest.ClaimId = identifier.Claim;
            digest.TargetId = identifier.Target;
            digest.CommonCoreStandardsId = identifier.CommonCoreStandard;

            return digest;
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
            IList<InteractionType> interactionTypes)
        {
            BlockingCollection<ItemDigest> digests = new BlockingCollection<ItemDigest>();
            Parallel.ForEach(itemMetadata, metadata =>
            {
                var matchingItems = itemContents.Where(c => c.Item.ItemKey == metadata.Metadata.ItemKey);
                var itemsCount = matchingItems.Count();

                if (itemsCount == 1)
                {
                    digests.Add(ItemToItemDigest(metadata, matchingItems.First(), resourceFamilies, interactionTypes));
                }
                else if (itemsCount > 1)
                {
                    throw new SampleItemsContextException("Multiple ItemContents with ItemKey: " + metadata.Metadata.ItemKey + " found.");
                }
                // TODO: log a warning if item count is 0
            });
            return digests;
        }
    }
}
