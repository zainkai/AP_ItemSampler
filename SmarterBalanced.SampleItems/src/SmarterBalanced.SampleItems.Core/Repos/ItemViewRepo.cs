using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public class ItemViewRepo : IItemViewRepo
     {
        private SampleItemsContext context;
        public ItemViewRepo(SampleItemsContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get all ItemDigests with default order (BankKey, then ItemKey).
        /// </summary>
        /// <returns>
        /// An IEnumerable of ItemDigests
        /// </returns>
        public IEnumerable<ItemDigest> GetItemDigests()
        {
            return context.ItemDigests.OrderBy(t => t.BankKey)
                                        .ThenBy(t => t.ItemKey);
        }

        /// <summary>
        /// Get all ItemDigests matching the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>An IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigests(Func<ItemDigest, bool> predicate)
        {
            return GetItemDigests().Where(predicate);
        }

        /// <summary>
        /// Retreives the single specified ItemDigest.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>ItemDigest</returns>
        public ItemDigest GetItemDigest(Func<ItemDigest, bool> predicate)
        {
            return GetItemDigests().SingleOrDefault(predicate);
        }

        /// <summary>
        /// Retrieves an ItemDigest matching the given bankKey and itemKey.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>an ItemDigest object.</returns>
        public ItemDigest GetItemDigest(int bankKey, int itemKey)
        {
            return GetItemDigest(item => item.BankKey == bankKey && item.ItemKey == itemKey);
        }

        /// <summary>
        /// Constructs an itemviewerservice URL to access the 
        /// item corresponding to the given ItemDigest.
        /// </summary>
        /// <param name="digest"></param>
        /// <returns>a string URL.</returns>
        private string GetItemViewerUrl(ItemDigest digest)
        {
            if(digest == null)
            {
                return string.Empty;
            }

            string baseUrl = context.AppSettings().SettingsConfig.ItemViewerServiceURL;
            return $"{baseUrl}/item/{digest.BankKey}-{digest.ItemKey}";
        }

        /// <summary>
        /// Constructs an ItemViewModel with an ItemViewerService URL.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>an ItemViewModel.</returns>
        public ItemViewModel GetItemViewModel(int bankKey, int itemKey)
        {
            ItemViewModel itemView = null;
            ItemDigest itemDigest = GetItemDigest(bankKey, itemKey);
            if(itemDigest != null)
            {
                itemView = new ItemViewModel();
                itemView.ItemDigest = itemDigest;
                itemView.ItemViewerServiceUrl = GetItemViewerUrl(itemView.ItemDigest);
            }
            return itemView;
        }
    }
}
