using SmarterBalanced.SampleItems.Core.Repos.Models;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface IGlobalAccessibilityRepo
    {
        GlobalAccessibilityViewModel GetGlobalAccessibilityViewModel();
        GlobalAccessibilityViewModel GetGlobalAccessibilityViewModel(string ISSAPCode);

        string GetISSAPCode(GlobalAccessibilityViewModel globalAccessibilityViewModel);
    }
}