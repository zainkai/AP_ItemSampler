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
        public static IList<AccessibilityResource> ToAccessibilityResources(this IEnumerable<XElement> singleSelectResources)
        {
            IList<AccessibilityResource> accessibilityResources = singleSelectResources
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
                .ToList(); 
                        
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
            IEnumerable<AccessibilityResource> globalResources)
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
                        .ToAccessibilityResources(globalResources)))
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

        public static ImmutableArray<AccessibilityResource> ToAccessibilityResources(
            this IEnumerable<XElement> xmlFamilyResources,
            IEnumerable<AccessibilityResource> globalResources)
        {
            var partialResources = xmlFamilyResources
                .Select(r => ToAccessibilityResource(r))
                .ToImmutableArray();

            return partialResources.MergeAllWith(globalResources);
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

        public static AccessibilityResource MergeWith(this AccessibilityResource familyResource, AccessibilityResource globalResource)
        {
            if (familyResource == null)
                throw new ArgumentNullException(nameof(familyResource));
            else if (globalResource == null)
                throw new ArgumentNullException(nameof(globalResource));
            
            var newSelections = globalResource.Selections.Select(sel =>
            {
                var familySel = familyResource.Selections.SingleOrDefault(fs => fs.Code == sel.Code);
                var selDisabled = familyResource.Disabled || familySel == null;
                var label = string.IsNullOrEmpty(familySel?.Label) ? sel.Label : familySel.Label;

                var newSelection = new AccessibilitySelection(
                    code: sel.Code,
                    label: label,
                    order: sel.Order,
                    disabled: selDisabled);

                return newSelection;
            }).ToImmutableArray();
            
            // If the default select item is disabled, pick a different one 
            string newDefault;
            if (!familyResource.Disabled && globalResource.Selections.Any(s => s.Code == globalResource.DefaultSelection && s.Disabled))
            {
                newDefault = globalResource.Selections.FirstOrDefault(s => !s.Disabled)?.Code ?? globalResource.DefaultSelection;
            }
            else
            {
                newDefault = globalResource.DefaultSelection;
            }

            var newResource = new AccessibilityResource(
                code: globalResource.Code,
                selectedCode: globalResource.SelectedCode,
                order: globalResource.Order,
                defaultSelection: newDefault,
                selections: newSelections,
                label: globalResource.Label,
                description: globalResource.Description,
                disabled: globalResource.Disabled,
                resourceType: globalResource.ResourceType);

            return globalResource;
        }

    }

}
