using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface ISampleItemsSearchRepo
    {
        Task<IList<ItemDigest>> GetItemDigestsAsync();
        Task<IList<ItemDigest>> GetItemDigestsAsync(string terms, GradeLevels grades, IList<string> subjects, string[] interactionTypes);
        Task<ItemsSearchViewModel> GetItemsSearchViewModelAsync();
    }
}
