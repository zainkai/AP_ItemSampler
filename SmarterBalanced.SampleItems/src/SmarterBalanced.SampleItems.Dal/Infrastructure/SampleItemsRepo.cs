using SmarterBalanced.SampleItems.Dal.Context;
using SmarterBalanced.SampleItems.Dal.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Infrastructure
{
    public class SampleItemsRepo : ISampleItemsRepo
    {
        private ISampleItemsContext s_context;
        private bool s_disposed;


        public SampleItemsRepo() : this(new SampleItemsContext()) { }

        public SampleItemsRepo(ISampleItemsContext context)
        {
            s_context = context;
        }

        public IEnumerable<ItemDigest> GetItemDigests()
        {
            return s_context.ItemDigests;
        }

        public IEnumerable<ItemDigest> GetItemDigests(Func<ItemDigest, bool> predicate)
        {
            return GetItemDigests().Where(predicate);
        }

        /// <summary>
        /// Retreives the single specified ItemDigest
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>ItemDigest</returns>
        public ItemDigest GetItemDigest(Func<ItemDigest, bool> predicate)
        {
            return GetItemDigests().SingleOrDefault(predicate);
        }

        public ItemDigest GetItemDigest(int bankKey, int itemKey)
        {
            return GetItemDigest(t => t.BankKey == bankKey && t.ItemKey == itemKey);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            throw new NotImplementedException();
        }


    }
}
