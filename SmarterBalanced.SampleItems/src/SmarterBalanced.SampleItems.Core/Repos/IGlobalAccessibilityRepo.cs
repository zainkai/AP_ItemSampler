using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface IGlobalAccessibilityRepo
    {
        Task<GlobalAccessibilityViewModel> GetGlobalAccessibilityViewModelAsync(string iSAAPCode);

        Task<string> GetISAAPCodeAsync(GlobalAccessibilityViewModel globalAccessibilityViewModel);

        AppSettings GetSettings();
    }
}