using SmarterBalanced.SampleItems.Dal.Exceptions;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
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
            IList<AccessibilityResource> globalResources,
            IList<AccessibilityResourceFamily> resourceFamilies)
        {
            if (itemContents.Item.ItemKey != itemMetadata.Metadata.ItemKey)
            {
                throw new SampleItemsContextException("Cannot digest items with different ItemKey values.");
            }

            ItemDigest digest = new ItemDigest(globalResources, resourceFamilies);
            digest.BankKey = itemContents.Item.ItemBank;
            digest.ItemKey = itemContents.Item.ItemKey;
            digest.Grade = GradeLevelsUtils.FromString(itemMetadata.Metadata.Grade);
            digest.Target = itemMetadata.Metadata.Target;
            digest.Subject = itemMetadata.Metadata.Subject;
            digest.InteractionType = itemMetadata.Metadata.InteractionType;
            digest.Claim = itemMetadata.Metadata.Claim;
            digest.AssociatedStimulus = itemMetadata.Metadata.AssociatedStimulus;

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
            IList<AccessibilityResource> globalResources,
            IList<AccessibilityResourceFamily> resourceFamilies)
        {
            BlockingCollection<ItemDigest> digests = new BlockingCollection<ItemDigest>();
            Parallel.ForEach(itemMetadata, metadata =>
            {
                var matchingItems = itemContents.Where(c => c.Item.ItemKey == metadata.Metadata.ItemKey);
                var itemsCount = matchingItems.Count();

                if (itemsCount == 1)
                {
                    digests.Add(ItemToItemDigest(metadata, matchingItems.First(), globalResources, resourceFamilies));
                }
                else if (itemsCount > 1)
                {
                    throw new SampleItemsContextException("Multiple ItemContents wih ItemKey: " + metadata.Metadata.ItemKey + " found.");
                }
                // TODO: log a warning if item count is 0
            });
            return digests;
        }
    }
}
