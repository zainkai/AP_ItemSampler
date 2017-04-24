using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class ItemViewModel
    {
        public string ItemViewerServiceUrl { get; }

        public string ItemNames { get;}
        public string BrailleItemNames { get; }

        public string AccessibilityCookieName { get; }

        public bool IsPerformanceItem { get; }

        public AboutThisItemViewModel AboutThisItemVM { get; }

        public ImmutableArray<AccessibilityResourceGroup> AccResourceGroups { get; }

        public MoreLikeThisViewModel MoreLikeThisVM { get; }

        public string PerformanceItemDescription { get; }

        public string Subject { get; }

        public ImmutableArray<string> BrailleItemCodes { get; }
        public ImmutableArray<string> BraillePassageCodes { get; }

        public ItemViewModel(
            string itemViewerServiceUrl,
            string accessibilityCookieName,
            bool isPerformanceItem,
            ImmutableArray<AccessibilityResourceGroup> accResourceGroups,
            MoreLikeThisViewModel moreLikeThisVM,
            AboutThisItemViewModel aboutThisItemVM,
            string subject,
            ImmutableArray<string> brailleItemCodes,
            ImmutableArray<string> braillePassageCodes,
            string performanceItemDescription = "",
            string itemNames = "",
            string brailleItemNames = "")
        {
            ItemViewerServiceUrl = itemViewerServiceUrl;
            AccessibilityCookieName = accessibilityCookieName;
            IsPerformanceItem = isPerformanceItem;
            AboutThisItemVM = aboutThisItemVM;
            AccResourceGroups = accResourceGroups;
            MoreLikeThisVM = moreLikeThisVM;
            Subject = subject;
            BrailleItemCodes = brailleItemCodes;
            BraillePassageCodes = braillePassageCodes;
            PerformanceItemDescription = performanceItemDescription;
            ItemNames = itemNames;
            BrailleItemCodes = brailleItemCodes;
            BrailleItemNames = brailleItemNames;
        }
    }
}
