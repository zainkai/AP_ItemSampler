using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class ItemViewModel
    {
        public string ItemViewerServiceUrl { get; set; }

        public ItemDigest ItemDigest { get; set; }

        public LocalAccessibilityViewModel LocalAccessibilityViewModel { get; set; }
    }
}
