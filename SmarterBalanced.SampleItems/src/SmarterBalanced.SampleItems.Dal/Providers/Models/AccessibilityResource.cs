using System.Collections.Generic;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class AccessibilityResource
    {
        public string SelectedCode { get; }
        public int Order { get; }
        public string DefaultSelection { get; }
        public ImmutableArray<AccessibilitySelection> Selections { get; }
        public string Label { get; }
        public string Description { get; }
        public bool Disabled { get; }
        public string ResourceType { get; }

        public AccessibilityResource(
            string selectedCode,
            int order,
            string defaultSelection,
            ImmutableArray<AccessibilitySelection> selections,
            string label,
            string description,
            bool disabled,
            string resourceType)
        {
            SelectedCode = selectedCode;
            Order = order;
            DefaultSelection = DefaultSelection;
            Selections = selections;
            Label = label;
            Description = description;
            Disabled = disabled;
            ResourceType = resourceType;
        }

        public static AccessibilityResource Create(
            string selectedCode = "",
            int order = -1,
            string defaultSelection = "",
            ImmutableArray<AccessibilitySelection> selections = default(ImmutableArray<AccessibilitySelection>),
            string label = "",
            string description = "",
            bool disabled = false,
            string resourceType = "")
        {
            var resource = new AccessibilityResource(
                selectedCode: selectedCode,
                order: order,
                defaultSelection: defaultSelection,
                selections: selections,
                label: label,
                description: description,
                disabled: disabled,
                resourceType: resourceType);

            return resource;
        }
    }
}
