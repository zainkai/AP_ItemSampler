using SmarterBalanced.SampleItems.Dal.Providers.Models;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public interface IAboutItemsRepo: IItemViewRepo
    {
        AboutItemsViewModel GetAboutItemsViewModel(string interactionTypeCode = "");
    }
}
