using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    interface IAboutItemRepo : IItemViewRepo
    {
        AboutItemsViewModel GetAboutItemsViewModel();
        ItemCardViewModel GetItemCardViewModel(InteractionType interactionType);
    }
}
