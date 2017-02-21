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
        public static ImmutableArray<AccessibilityResource> ToGlobalAccessibilityResources(
                this IEnumerable<XElement> singleSelectResources)
        {
            var accessibilityResources = singleSelectResources
                .Select(xs => AccessibilityResource.Create(xs))
                .Where(a => a.Selections.Any())
                .OrderBy(a => a.Order)
                .ToImmutableArray();

            return accessibilityResources;
        }

        public static ImmutableArray<AccessibilityFamily> ToAccessibilityResourceFamilies(
            this IEnumerable<XElement> elem)
        {
            var families = elem
                .Select(rf => AccessibilityFamily.Create(rf))
                .ToImmutableArray();

            return families;
        }

        /// <summary>
        /// Translates Partial family resources with global resources to full set of family resources
        /// </summary>
        private static MergedAccessibilityFamily MergeGlobalResources(
            this AccessibilityFamily family,
            IEnumerable<AccessibilityResource> globalResources)
        {
            var newResources = globalResources.Select(globalResource =>
            {
                var familyResource = family.Resources.FirstOrDefault(fr => fr.ResourceCode == globalResource.ResourceCode);
                if (familyResource == null)
                {
                    return globalResource;
                }
                else
                {
                    return MergeGlobalResource(familyResource, globalResource);
                }
            }).ToImmutableArray();

            var mergedFamily = new MergedAccessibilityFamily(
                subjects: family.Subjects,
                grades: family.Grades,
                resources: newResources);

            return mergedFamily;
        }

        private static AccessibilitySelection MergeSelection(AccessibilitySelection sel, AccessibilityFamilyResource familyResource)
        {
            var familySel = familyResource.Selections.SingleOrDefault(fs => fs.Code == sel.SelectionCode);
            var selDisabled = familyResource.Disabled || familySel == null;
            var label = string.IsNullOrEmpty(familySel?.Label) ? sel.Label : familySel.Label;

            var newSelection = new AccessibilitySelection(
                code: sel.SelectionCode,
                label: label,
                order: sel.Order,
                disabled: selDisabled);

            return newSelection;
        }

        private static AccessibilityResource MergeGlobalResource(AccessibilityFamilyResource familyResource, AccessibilityResource globalResource)
        {
            if (familyResource == null)
                throw new ArgumentNullException(nameof(familyResource));
            else if (globalResource == null)
                throw new ArgumentNullException(nameof(globalResource));

            var newSelections = globalResource.Selections
                .Select(sel => MergeSelection(sel, familyResource))
                .ToImmutableArray();

            var matchingSelection = newSelections.FirstOrDefault(s => s.SelectionCode == globalResource.DefaultSelection);
            bool isDefaultInvalid = matchingSelection == null || matchingSelection.Disabled;

            string newDefault = isDefaultInvalid
                ? newSelections.FirstOrDefault(s => !s.Disabled)?.SelectionCode ?? string.Empty
                : globalResource.DefaultSelection;

            var newResource = new AccessibilityResource(
                resourceCode: globalResource.ResourceCode,
                currentSelectionCode: globalResource.CurrentSelectionCode,
                order: globalResource.Order,
                defaultSelection: newDefault,
                selections: newSelections,
                label: globalResource.Label,
                description: globalResource.Description,
                disabled: familyResource.Disabled,
                resourceType: globalResource.ResourceType);

            return newResource;
        }

        public static ImmutableArray<MergedAccessibilityFamily> CreateMergedFamilies(XElement accessibilityXml)
        {
            ImmutableArray<AccessibilityResource> globalResources = accessibilityXml
                .Element("MasterResourceFamily")
                .Elements("SingleSelectResource")
                .ToGlobalAccessibilityResources();

            ImmutableArray<AccessibilityFamily> families = accessibilityXml
                .Elements("ResourceFamily")
                .ToAccessibilityResourceFamilies();

            ImmutableArray<MergedAccessibilityFamily> mergedFamilies = families
                .Select(f => MergeGlobalResources(f, globalResources))
                .ToImmutableArray();

            return mergedFamilies;
        }
    }

}
