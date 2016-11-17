using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface IGlobalAccessibilityRepo
    {
        GlobalAccessibilityViewModel GetGlobalAccessibilityViewModel(string iSAAPCode);

        string GetISAAPCode(GlobalAccessibilityViewModel globalAccessibilityViewModel);

        AppSettings GetSettings();
    }
}