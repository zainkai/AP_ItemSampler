using SmarterBalanced.SampleItems.Dal.Models;
using SmarterBalanced.SampleItems.Dal.Infrastructure;
using SmarterBalanced.SampleItems.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Core.Interfaces;

namespace SmarterBalanced.SampleItems.Core.Infrastructure
{
    public class ItemViewRepo : IItemViewRepo
     {
        private ISampleItemsRepo s_repo;
        public ItemViewRepo() : this(new SampleItemsRepo()) { }
        public ItemViewRepo(ISampleItemsRepo repo)
        {
            s_repo = repo;
        }
    }
}
