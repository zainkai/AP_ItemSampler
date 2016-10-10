using SmarterBalanced.SampleItems.Dal.Models;
using SmarterBalanced.SampleItems.Dal.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Core.Interfaces;
using SmarterBalanced.SampleItems.Dal.Context;

namespace SmarterBalanced.SampleItems.Core.Infrastructure
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
