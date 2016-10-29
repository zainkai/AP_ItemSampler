using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface ISampleItemsSearchRepo
    {
        IList<ItemDigest> GetItemDigests();

    }
}
