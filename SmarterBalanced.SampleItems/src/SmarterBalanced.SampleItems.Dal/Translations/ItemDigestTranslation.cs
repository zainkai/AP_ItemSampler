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
        public static IEnumerable<ItemDigest> ToItemDigests(
            IEnumerable<ItemMetadata> itemMetadata,
            IEnumerable<ItemContents> itemContents,
            AppSettings settings)
        {
            BlockingCollection<ItemDigest> digests = new BlockingCollection<ItemDigest>();
            Parallel.ForEach(itemMetadata, metadata =>
            {
                var matchingItems = itemContents.Where(c => c.Item.ItemKey == metadata.Metadata.ItemKey);
                var itemsCount = matchingItems.Count();

                if (itemsCount == 1)
                {
                    ItemDigest itemDigest = ToItemDigest(metadata, matchingItems.First(), settings);

                    digests.Add(itemDigest);
                }
                else if (itemsCount > 1)
                {
                    throw new SampleItemsContextException("Multiple ItemContents with ItemKey: " + metadata.Metadata.ItemKey + " found.");
                }




            });

            return digests;
        }


        /// <summary>
        /// Translates metadata, itemcontents and lookups to item digest
        /// </summary>
        public static ItemDigest ToItemDigest(
            ItemMetadata itemMetadata,
            ItemContents itemContents,
            AppSettings settings)
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
                TargetAssessmentType = itemMetadata.Metadata.TargetAssessmentType,
                SufficentEvidenceOfClaim = itemMetadata.Metadata.SufficientEvidenceOfClaim,
                AssociatedStimulus = itemMetadata.Metadata.AssociatedStimulus,
                AslSupported = itemMetadata.Metadata.AccessibilityTagsASLLanguage == "Y",
                AllowCalculator = itemMetadata.Metadata.AllowCalculator == "Y",
                DepthOfKnowledge = itemMetadata.Metadata.DepthOfKnowledge,
                Contents = itemContents.Item.Contents,
                InteractionTypeCode = itemMetadata.Metadata.InteractionType,
                AssociatedPassage = itemContents.Item.AssociatedPassage,
                GradeCode = itemMetadata.Metadata.GradeCode,
                MaximumNumberOfPoints = itemMetadata.Metadata.MaximumNumberOfPoints,
                StandardPublications = itemMetadata.Metadata.StandardPublications,
                SubjectCode = itemMetadata.Metadata.SubjectCode,
                ItemMetadataAttributes = itemContents.Item.ItemMetadataAttributes
            };

            return digest;
        }

    }
}
