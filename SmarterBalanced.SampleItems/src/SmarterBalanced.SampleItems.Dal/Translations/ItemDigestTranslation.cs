using SmarterBalanced.SampleItems.Dal.Models;
using System;
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
        public static ItemDigest ItemToItemDigest(ItemMetadata itemMetadata, ItemContents itemContents)
        {
            if (itemContents.item.ItemKey != itemMetadata.metadata.ItemKey)
            {
                throw new Exception("Cannot digest items with different ItemKey values.");
            }
            ItemDigest digest = new ItemDigest();
            digest.BankKey = itemContents.item.ItemBank;
            digest.ItemKey = itemContents.item.ItemKey;
            digest.Grade = itemMetadata.metadata.Grade;
            digest.Target = itemMetadata.metadata.Target;
            digest.Subject = itemMetadata.metadata.Subject;
            digest.InteractionType = itemMetadata.metadata.InteractionType;
            digest.Claim = itemMetadata.metadata.Claim;
            return digest;
        }

        /// <summary>
        /// Digests a collection of ItemMetadata objects and a collection of ItemContents objects into a collection of ItemDigest objects.
        /// Matches the ItemMetadata and ItemContents objects based on their ItemKey fields.
        /// </summary>
        /// <param name="itemMetadata"></param>
        /// <param name="itemContents"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<ItemDigest>> ItemsToItemDigestsAsync(IEnumerable<ItemMetadata> itemMetadata, IEnumerable<ItemContents> itemContents)
        {
            List<ItemDigest> digests = new List<ItemDigest>();
            if (itemMetadata.Count() != itemContents.Count())
            {
                throw new Exception("Item metadata and contents counts differ.");
            }
            await Task.Run(() =>
            {
                foreach (ItemMetadata metadata in itemMetadata)
                {
                    var countitems = itemContents.Where(c => c.item.ItemKey == metadata.metadata.ItemKey);

                    if (countitems.Count() == 1)
                    {
                        digests.Add(ItemToItemDigest(metadata, countitems.First()));
                    }

                    else if (countitems.Count() == 0)
                    {
                        throw new Exception("Could not match ItemMetadata object with ItemKey: " + metadata.metadata.ItemKey +
                            " with an ItemContents object");
                    }
                    else
                    {
                        throw new Exception("Multiple ItemContents wih ItemKey: " + metadata.metadata.ItemKey + " found.");
                    }
                }
            });
            return digests;
        }
    }
}
