using System;
using System.Collections.Generic;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface IItemViewRepo
    {
        AppSettings GetSettings();

        ItemDigest GetItemDigest(Func<ItemDigest, bool> predicate);

        ItemDigest GetItemDigest(int bankKey, int itemKey);

        IEnumerable<ItemDigest> GetItemDigests();

        ItemViewModel GetItemViewModel(int bankKey, int itemKey);

        ItemViewModel GetItemViewModel(int bankKey, int itemKey, string iSSAPCode);

        LocalAccessibilityViewModel UpdateAccessibility(LocalAccessibilityViewModel localAccViewModel, string iSSAP);
    }
}
