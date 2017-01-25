using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class AccessibilityResourceTranslation
    {
        public static ImmutableArray<AccessibilityResource> ToAccessibilityResources(this IEnumerable<XElement> singleSelectResources)
        {
            var accessibilityResources = singleSelectResources
                .Select(xs =>
                {
                    var selections = xs.Elements("Selection")
                        .Select(x => x.ToSelection())
                        .ToImmutableArray();

                    var defaultSelection = (string)xs.Element("DefaultSelection");
                    defaultSelection = string.IsNullOrEmpty(defaultSelection)
                        ? selections.FirstOrDefault()?.Code
                        : defaultSelection;

                    var resourceType = (string)xs.Element("ResourceType") ?? string.Empty;

                    var textElem = xs.Element("Text");

                    return new AccessibilityResource(
                        code: (string)xs.Element("Code"),
                        selectedCode: defaultSelection,
                        order: (int)xs.Element("Order"),
                        defaultSelection: defaultSelection,
                        label: (string)textElem.Element("Label"),
                        description: (string)textElem.Element("Description"),
                        disabled: xs.Element("Disabled") != null,
                        selections: selections,
                        resourceType: resourceType);
                })
                .Where(a => a.Selections.Any())
                .OrderBy(a => a.Order)
                .ToImmutableArray();

            return accessibilityResources;
        }

        public static AccessibilitySelection ToSelection(this XElement xmlSelection)
        {
            var selection = new AccessibilitySelection(
                    code: (string)xmlSelection.Element("Code"),
                    label: (string)xmlSelection.Element("Text").Element("Label"),
                    order: (int)xmlSelection.Element("Order"),
                    disabled: false);

            return selection;
        }

        public static ImmutableArray<AccessibilityResourceFamily> ToAccessibilityResourceFamilies(
            this IEnumerable<XElement> resourceFamilies,
            IList<AccessibilityResource> globalResources)
        {
            ImmutableArray<AccessibilityResourceFamily> families = resourceFamilies
                .Select(f => new AccessibilityResourceFamily(
                    subjects: f.Elements("Subject")
                        .Select(s => (string)s.Element("Code"))
                        .ToImmutableArray(),
                    grades: f.Elements("Grade")
                        .Select(g => g.Value)
                        .ToGradeLevels(),
                    resources: f.Elements("SingleSelectResource")
                        .Select(r => ToAccessibilityResource(r))
                        .MergeAllWith(globalResources)))
                .ToImmutableArray();

            return families;
        }

        public static AccessibilityResource ToAccessibilityResource(XElement elem)
        {
            var selectionsElem = elem.Elements("Selections");
            var selections = selectionsElem == null
                ? ImmutableArray<AccessibilitySelection>.Empty
                : selectionsElem.Select(s => s.ToSelection()).ToImmutableArray();

            var resource = new AccessibilityResource(
                code: (string)elem.Element("Code"),
                selectedCode: null,
                order: (int?)elem.Element("Order") ?? 0,
                defaultSelection: null,
                label: null,
                description: null,
                resourceType: null,
                disabled: elem.Element("Disabled") != null,
                selections: selections);

            return resource;
        }

        /// <summary>
        /// Translates Partial family resources with global resources to full set of family resources
        /// </summary>
        public static ImmutableArray<AccessibilityResource> MergeAllWith(
            this IEnumerable<AccessibilityResource> partialResources,
            IEnumerable<AccessibilityResource> globalResources)
        {
            var resources = globalResources.Select(globalResource =>
            {
                var familyResource = partialResources.FirstOrDefault(fr => fr.SelectedCode == globalResource.SelectedCode);
                if (familyResource == null)
                {
                    return globalResource;
                }
                else
                {
                    return familyResource.MergeWith(globalResource);
                }
            }).ToImmutableArray();

            return resources;
        }

        public static AccessibilitySelection MergeSelection(AccessibilitySelection sel, AccessibilityResource familyResource)
        {
            var familySel = familyResource.Selections.SingleOrDefault(fs => fs.Code == sel.Code);
            var selDisabled = familyResource.Disabled || familySel == null || familySel.Disabled;
            var label = string.IsNullOrEmpty(familySel?.Label) ? sel.Label : familySel.Label;

            var newSelection = new AccessibilitySelection(
                code: sel.Code,
                label: label,
                order: familySel?.Order ?? sel.Order,
                disabled: selDisabled);

            return newSelection;
        }

        public static AccessibilityResource MergeWith(this AccessibilityResource familyResource, AccessibilityResource globalResource)
        {
            if (familyResource == null)
                throw new ArgumentNullException(nameof(familyResource));
            else if (globalResource == null)
                throw new ArgumentNullException(nameof(globalResource));

            var newSelections = globalResource.Selections
                .Select(sel => MergeSelection(sel, familyResource))
                .ToImmutableArray();

            string explicitDefault = string.IsNullOrEmpty(familyResource.DefaultSelection)
                ? globalResource.DefaultSelection
                : familyResource.DefaultSelection;

            var matchingSelection = newSelections.FirstOrDefault(s => s.Code == explicitDefault);
            bool isDefaultInvalid = matchingSelection == null || matchingSelection.Disabled;

            string newDefault = isDefaultInvalid
                ? newSelections.FirstOrDefault(s => !s.Disabled)?.Code ?? string.Empty
                : explicitDefault;

            var newResource = new AccessibilityResource(
                code: globalResource.Code,
                selectedCode: globalResource.SelectedCode,
                order: globalResource.Order,
                defaultSelection: newDefault,
                selections: newSelections,
                label: globalResource.Label,
                description: globalResource.Description,
                disabled: familyResource.Disabled,
                resourceType: globalResource.ResourceType);

            return newResource;
        }

    }

}
