using System.Linq;
using System.Collections.Immutable;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class AccessibilityResource
    {
        /// <summary>
        /// ID for this accessibility resource.
        /// </summary>
        public string ResourceCode { get; }

        /// <summary>
        /// ID of the current AccessibilitySelection.
        /// </summary>
        public string CurrentSelectionCode { get; }

        public int Order { get; }
        public string DefaultSelection { get; }
        public ImmutableArray<AccessibilitySelection> Selections { get; }
        public string Label { get; }
        public string Description { get; }
        public bool Disabled { get; }
        public string ResourceTypeId { get; }

        public AccessibilityResource(
            string resourceCode,
            string currentSelectionCode,
            int order,
            string defaultSelection,
            ImmutableArray<AccessibilitySelection> selections,
            string label,
            string description,
            bool disabled,
            string resourceType)
        {
            ResourceCode = resourceCode;
            CurrentSelectionCode = currentSelectionCode;
            Order = order;
            DefaultSelection = defaultSelection;
            Selections = selections;
            Label = label;
            Description = description;
            Disabled = disabled;
            ResourceTypeId = resourceType;
        }

        public AccessibilityResource WithCurrentSelection(string currentSelectionCode)
        {
            var newResource = new AccessibilityResource(
                resourceCode: ResourceCode,
                currentSelectionCode: currentSelectionCode,
                order: Order,
                defaultSelection: DefaultSelection,
                selections: Selections,
                label: Label,
                description: Description,
                disabled: Disabled,
                resourceType: ResourceTypeId);

            return newResource;
        }

        public AccessibilityResource WithSelections(ImmutableArray<AccessibilitySelection> selections)
        {
            var newResource = new AccessibilityResource(
                resourceCode: ResourceCode,
                currentSelectionCode: CurrentSelectionCode,
                order: Order,
                defaultSelection: DefaultSelection,
                selections: selections,
                label: Label,
                description: Description,
                disabled: Disabled,
                resourceType: ResourceTypeId);

            return newResource;
        }

        public AccessibilityResource ToDisabled()
        {
            var newSelections = Selections
                .Select(sel => sel.WithDisabled(true))
                .ToImmutableArray();

            var newResource = new AccessibilityResource(
                resourceCode: ResourceCode,
                currentSelectionCode: CurrentSelectionCode,
                order: Order,
                defaultSelection: DefaultSelection,
                selections: newSelections,
                label: Label,
                description: Description,
                disabled: true,
                resourceType: ResourceTypeId);

            return newResource;
        }

        public static AccessibilityResource Create(
            string resourceCode = "",
            string currentSelectionCode = "",
            int order = -1,
            string defaultSelection = "",
            ImmutableArray<AccessibilitySelection> selections = default(ImmutableArray<AccessibilitySelection>),
            string label = "",
            string description = "",
            bool disabled = false,
            string resourceType = "")
        {
            var resource = new AccessibilityResource(
                resourceCode: resourceCode,
                currentSelectionCode: currentSelectionCode,
                order: order,
                defaultSelection: defaultSelection,
                selections: selections,
                label: label,
                description: description,
                disabled: disabled,
                resourceType: resourceType);

            return resource;
        }

        public static AccessibilityResource Create(XElement element)
        {
            var selections = element.Elements("Selection")
                       .Select(x => AccessibilitySelection.Create(x))
                       .ToImmutableArray();

            var defaultSelection = (string)element.Element("DefaultSelection");
            defaultSelection = string.IsNullOrEmpty(defaultSelection)
                ? selections.FirstOrDefault()?.SelectionCode
                : defaultSelection;

            var resourceType = (string)element.Element("ResourceType") ?? string.Empty;

            var textElem = element.Element("Text");

            return new AccessibilityResource(
                resourceCode: (string)element.Element("Code"),
                currentSelectionCode: defaultSelection,
                order: (int)element.Element("Order"),
                defaultSelection: defaultSelection,
                label: (string)textElem.Element("Label"),
                description: (string)textElem.Element("Description"),
                disabled: false,
                selections: selections,
                resourceType: resourceType);
        }
    }
}
