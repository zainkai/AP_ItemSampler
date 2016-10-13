using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public class SampleItemsSearchRepo : ISampleItemsSearchRepo
     {
        private SampleItemsContext context;
        public SampleItemsSearchRepo(SampleItemsContext context)
        {
            this.context = context;
        }

        public IList<ItemDigest> GetItemDigests()
        {
            return context.ItemDigests.Where(t => !string.IsNullOrEmpty(t.Grade) && t.Grade != "NA").ToList();
        }

    }
}
