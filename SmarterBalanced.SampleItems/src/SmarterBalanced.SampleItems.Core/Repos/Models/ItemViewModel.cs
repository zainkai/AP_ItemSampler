using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class ItemViewModel
    {
        public string ItemViewerServiceUrl { get; }

        public string AccessibilityCookieName { get; }

        public AboutItemViewModel AboutItemVM { get; }

        public List<AccessibilityResourceViewModel> AccResourceVMs { get; }

        public ItemViewModel(
            string itemViewerServiceUrl,
            string accessibilityCookieName,
            AboutItemViewModel aboutItemVM,
            List<AccessibilityResourceViewModel> accResourceVMs)
        {
            ItemViewerServiceUrl = itemViewerServiceUrl;
            AccessibilityCookieName = accessibilityCookieName;
            AboutItemVM = aboutItemVM;
            AccResourceVMs = accResourceVMs;
        }
    }
}
