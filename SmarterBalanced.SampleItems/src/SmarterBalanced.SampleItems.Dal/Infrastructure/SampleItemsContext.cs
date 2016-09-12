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
            throw new NotImplementedException();
            ItemContents itemcontents = null;
            ItemMetadata itemMetadata = null;
            var stuff = ItemDigestTranslation.ItemToItemDigest(itemMetadata, itemcontents);
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
