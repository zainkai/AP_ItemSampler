using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class ItemDigestTranslation
    {
        public static ItemDigest ItemToItemDigest(ItemMetadata itemMetadata, ItemContents itemContents)
        {
            if(itemContents.item.ItemKey != itemMetadata.metadata.ItemKey)
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

        public static List<ItemDigest> ItemsToItemDigests(IEnumerable<ItemMetadata> itemMetadata, IEnumerable<ItemContents> itemContents)
        {
            List<ItemDigest> digests = (from content in itemContents
                          join metadata in itemMetadata 
                          on  content.item.ItemKey equals metadata.metadata.ItemKey
                          select ItemToItemDigest(metadata, content)).ToList();

            return digests;
        }

    }
}
