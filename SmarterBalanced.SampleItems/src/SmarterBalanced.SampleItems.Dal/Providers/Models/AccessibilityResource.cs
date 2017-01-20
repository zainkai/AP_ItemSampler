using System.Linq;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class AccessibilityResource
    {
        /// <summary>
        /// ID for this accessibility resource.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// ID of the current AccessibilitySelection.
        /// </summary>
        public string SelectedCode { get; }

        public int Order { get; }
        public string DefaultSelection { get; }
        public ImmutableArray<AccessibilitySelection> Selections { get; }
        public string Label { get; }
        public string Description { get; }
        public bool Disabled { get; }
        public string ResourceType { get; }

        public AccessibilityResource(
            string code,
            string selectedCode,
            int order,
            string defaultSelection,
            ImmutableArray<AccessibilitySelection> selections,
            string label,
            string description,
            bool disabled,
            string resourceType)
        {
            Code = code;
            SelectedCode = selectedCode;
            Order = order;
            DefaultSelection = DefaultSelection;
            Selections = selections;
            Label = label;
            Description = description;
            Disabled = disabled;
            ResourceType = resourceType;
        }

        public AccessibilityResource WithSelectedCode(string selectedCode)
        {
            var newResource = new AccessibilityResource(
                code: Code,
                selectedCode: selectedCode,
                order: Order,
                defaultSelection: DefaultSelection,
                selections: Selections,
                label: Label,
                description: Description,
                disabled: Disabled,
                resourceType: ResourceType);

            return newResource;
        }

        public AccessibilityResource ToDisabled()
        {
            var newSelections = Selections
                .Select(sel => sel.WithDisabled(true))
                .ToImmutableArray();

            var newResource = new AccessibilityResource(
                code: Code,
                selectedCode: SelectedCode,
                order: Order,
                defaultSelection: DefaultSelection,
                selections: newSelections,
                label: Label,
                description: Description,
                disabled: true,
                resourceType: ResourceType);

            return newResource;
        }

        public static AccessibilityResource Create(
            string code = "",
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
                code: code,
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
