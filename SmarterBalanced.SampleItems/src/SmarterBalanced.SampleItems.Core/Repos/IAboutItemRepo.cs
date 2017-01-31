using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public interface IAboutItemRepo : IItemViewRepo
    {
        ImmutableArray<InteractionType> GetAboutItemsViewModel();
        string GetItemViewerUrl(string interactionTypeCode);
    }
}
