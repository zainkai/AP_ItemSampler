﻿using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class ItemViewModel
    {
        public string ItemViewerServiceUrl { get; }

        public string AccessibilityCookieName { get; }

        public AboutThisItemViewModel AboutThisItemVM { get; }

        public ImmutableArray<AccessibilityResourceGroup> AccResourceGroups { get; }

        public ItemViewModel(
            string itemViewerServiceUrl,
            string accessibilityCookieName,
            AboutThisItemViewModel aboutThisItemVM,
            ImmutableArray<AccessibilityResourceGroup> accResourceGroups)
        {
            ItemViewerServiceUrl = itemViewerServiceUrl;
            AccessibilityCookieName = accessibilityCookieName;
            AboutThisItemVM = aboutThisItemVM;
            AccResourceGroups = accResourceGroups;
        }
    }
}
