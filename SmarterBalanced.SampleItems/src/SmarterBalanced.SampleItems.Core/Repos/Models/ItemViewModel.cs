using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class ItemViewModel
    {
        public string ItemViewerServiceUrl { get; set; }

        public ItemDigest ItemDigest { get; set; }

        public List<AccessibilityResourceViewModel> AccessibilityResourceViewModels { get; set; }

        public string NonApplicableAccessibilityResources { get; set; }
    }
}
