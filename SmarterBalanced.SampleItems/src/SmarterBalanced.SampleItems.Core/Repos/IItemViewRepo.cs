using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface IItemViewRepo
    {
        ItemDigest GetItemDigest(int bankKey, int itemKey);

        ItemViewModel GetItemViewModel(int bankKey, int itemKey);
    }
}
