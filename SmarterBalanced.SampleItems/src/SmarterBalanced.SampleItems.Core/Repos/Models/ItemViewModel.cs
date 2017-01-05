using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class ItemViewModel
    {
        public string ItemViewerServiceUrl { get; set; }

        public string AccessibilityCookieName { get; set; }

        public ItemDigest ItemDigest { get; set; }

        public List<AccessibilityResourceViewModel> AccResourceVMs { get; set; }
    }
}
