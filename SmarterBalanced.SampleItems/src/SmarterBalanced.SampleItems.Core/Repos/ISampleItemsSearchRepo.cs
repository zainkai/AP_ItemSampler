using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface ISampleItemsSearchRepo
    {
        IList<ItemCardViewModel> GetItemCards();
        IList<ItemCardViewModel> GetItemCards(GradeLevels grades, IList<string> subjects, string[] interactionTypes, string[] claimIds);
        ItemsSearchViewModel GetItemsSearchViewModel();
    }
}
