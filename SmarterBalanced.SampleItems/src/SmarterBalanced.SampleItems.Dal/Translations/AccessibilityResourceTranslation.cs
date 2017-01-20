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

                    return new AccessibilityResource(
                        selectedCode: (string)xs.Element("Code"),
                        order: (int)xs.Element("Order"),
                        defaultSelection: defaultSelection,
                        label: (string)xs.Element("Text").Element("Label"),
                        description: (string)xs.Element("Text").Element("Description"),
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

        /// <summary>
        /// Parse XML Family accessibility resources and attach global resources to AccessibIlity Resources
        /// </summary>
        /// <param name="xmlResources"></param>
        /// <param name="globalResources"></param>
        /// <returns></returns>
        public static ImmutableArray<AccessibilityResource> ToAccessibilityResources(
            this IEnumerable<XElement> xmlFamilyResources,
            IEnumerable<AccessibilityResource> globalResources)
        {
            var partialResources = xmlFamilyResources
                .Select(r => new AccessibilityResource(
                    selectedCode: (string)r.Element("Code"),
                    order: (int)r.Element("Order"),
                    defaultSelection: null,
                    label: null,
                    description: null,
                    resourceType: null,
                    disabled: r.Element("Disabled") != null,
                    selections: r.Elements("Selection")
                        .Select(s => s.ToSelection())
                        .ToImmutableArray()))
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

        /// <summary>
        /// Copies a Global AccessibilityResource based on Family Resource values and selections.
        /// </summary>
        /// <param name="partialResource"> Accessbility Family Resources </param>
        /// <param name="resource"> Global Accessiblity Resources </param>
        /// <returns></returns>
        public static AccessibilityResource MergeWith(this AccessibilityResource partialResource, AccessibilityResource resource)
        {
            if (partialResource == null)
                throw new ArgumentNullException(nameof(partialResource));
            else if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            // <Disabled /> means the entire resource is disabled
            bool isResourceDisabled = partialResource.Disabled;

            // TODO: immutability
            //resource.Disabled = isResourceDisabled;

            //foreach (AccessibilitySelection selection in resource.Selections)
            //{
            //    AccessibilitySelection partialResourceSelection = partialResource.Selections.SingleOrDefault(s => s.Code == selection.Code);
            //    selection.Disabled = (partialResourceSelection == null) || isResourceDisabled;
            //    selection.Label = string.IsNullOrEmpty(partialResourceSelection?.Label) ? selection.Label : partialResourceSelection?.Label;
            //}

            //// If the default select item is disabled, pick a different one 
            //if (!isResourceDisabled && resource.Selections != null
            //    && resource.Selections.Any(s => s.Code == resource.DefaultSelection && s.Disabled))
            //{
            //    resource.DefaultSelection = resource.Selections.FirstOrDefault(s => !s.Disabled)?.Code;
            //}

            return resource;
        }

    }

}
