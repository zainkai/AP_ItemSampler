using System;
using System.Collections.Generic;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface IItemViewRepo
    {
        AppSettings AppSettings { get; }

        ItemViewModel GetItemViewModel(int bankKey, int itemKey);

        ItemViewModel GetItemViewModel(int bankKey, int itemKey, string iSAAPCode);
    }
}
