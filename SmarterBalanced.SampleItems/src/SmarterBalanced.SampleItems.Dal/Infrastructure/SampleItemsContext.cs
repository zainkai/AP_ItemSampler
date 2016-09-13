using SmarterBalanced.SampleItems.Dal.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Dal.Translations;

namespace SmarterBalanced.SampleItems.Dal.Context
{
    public class SampleItemsContext : ISampleItemsContext
    {
        public IEnumerable<ItemDigest> ItemDigests { get; set; }

        /// <summary>
        /// TODO: Create itemdigest from xml serialization 
        /// </summary>
        public SampleItemsContext()
        {
            List<ItemDigest> digests = new List<ItemDigest>();
            for(int i =0; i < 100; i++)
            {
                ItemDigest digest = new ItemDigest
                {
                    BankKey = 103,
                    ItemKey = i
                };
                digests.Add(digest);
            }
            ItemDigests = digests;
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
