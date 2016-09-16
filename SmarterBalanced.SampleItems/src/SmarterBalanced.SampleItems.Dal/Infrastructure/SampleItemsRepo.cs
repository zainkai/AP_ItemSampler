using SmarterBalanced.SampleItems.Dal.Context;
using SmarterBalanced.SampleItems.Dal.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Infrastructure
{
    public class SampleItemsRepo: ISampleItemsRepo
    {
        private ISampleItemsContext s_context;

        private static SampleItemsRepo s_singletonInstance;

        private SampleItemsRepo()
        {
            s_context = new SampleItemsContext();
        }
        
        public static SampleItemsRepo Default
        {
            get
            {
                if(s_singletonInstance == null)
                {
                    s_singletonInstance = new SampleItemsRepo();
                }

                return s_singletonInstance;
            }
        }

        /// <summary>
        /// Get all ItemDigests with default order (BankKey, then ItemKey).
        /// </summary>
        /// <returns>
        /// An IEnumerable of ItemDigests
        /// </returns>
        public IEnumerable<ItemDigest> GetItemDigests()
        {
            return s_context.ItemDigests.OrderBy(t => t.BankKey)
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
        /// Get ItemDigest matching the specified identifier keys
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>ItemDigest</returns>
        public ItemDigest GetItemDigest(int bankKey, int itemKey)
        {
            return GetItemDigest(t => t.BankKey == bankKey && t.ItemKey == itemKey);
        }


    }
}
