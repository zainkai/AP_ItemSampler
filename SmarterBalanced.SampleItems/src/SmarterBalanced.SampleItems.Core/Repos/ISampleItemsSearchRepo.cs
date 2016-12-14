using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface ISampleItemsSearchRepo
    {
        IList<ItemDigest> GetItemDigests();
        IList<ItemDigest> GetItemDigests(GradeLevels grades, IList<string> subjects, string[] interactionTypes, string[] claimIds);
        ItemsSearchViewModel GetItemsSearchViewModel();
    }
}
