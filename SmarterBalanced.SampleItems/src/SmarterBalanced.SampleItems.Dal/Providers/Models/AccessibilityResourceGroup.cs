using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class AccessibilityResourceGroup
    {
        public string Label { get; }
        public int Order { get; }
        public ImmutableArray<AccessibilityResource> AccessibilityResources { get; } 

        public AccessibilityResourceGroup(
            string label,
            int order,
            ImmutableArray<AccessibilityResource> accessibilityResources)
        {
            Label = label;
            Order = order;
            AccessibilityResources = accessibilityResources;
        }
    }
}
