using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface IGlobalAccessibilityRepo
    {
        GlobalAccessibilityViewModel GetGlobalAccessibilityViewModel(string iSSAPCode);

        string GetISSAPCode(GlobalAccessibilityViewModel globalAccessibilityViewModel);

        AppSettings GetSettings();
    }
}