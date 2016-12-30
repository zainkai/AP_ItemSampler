using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using SmarterBalanced.SampleItems.Dal.Providers.Models;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class AccessibilityResourceTranslation
    {
        /// <summary>
        /// Parse XML to Global AccessbilityResources
        /// </summary>
        /// <param name="singleSelectResources"></param>
        /// <returns></returns>
        public static IList<AccessibilityResource> ToAccessibilityResources(this IEnumerable<XElement> singleSelectResources)
        {
            IList<AccessibilityResource> accessibilityResources = singleSelectResources
                .Select(a =>
                {
                    var selections = a.Elements("Selection").ToSelections();

                    var defaultSelection = (string)a.Element("DefaultSelection");
                    defaultSelection = string.IsNullOrEmpty(defaultSelection) ? 
                         selections.FirstOrDefault()?.Code : defaultSelection;

                    return new AccessibilityResource
                    {
                        Code = (string)a.Element("Code"),
                        Order = (int)a.Element("Order"),
                        DefaultSelection = defaultSelection,
                        Label = (string)a.Element("Text").Element("Label"),
                        Description = (string)a.Element("Text").Element("Description"),
                        Disabled = (a?.Element("Disabled") != null) ? true : false,
                        Selections = selections
                    };
                })
                .Where(a => a.Selections.Any())
                .ToList(); 
                        
            return accessibilityResources;
        }

        /// <summary>
        /// Parse XML Accessibilty Resource Selections to AccessibilitySelections
        /// </summary>
        /// <param name="xmlSelections"></param>
        /// <returns></returns>
        public static List<AccessibilitySelection> ToSelections(this IEnumerable<XElement> xmlSelections)
        {
            List<AccessibilitySelection> selections = xmlSelections
                .Select(s => new AccessibilitySelection
                {
                    Disabled = false,
                    Code = (string)s.Element("Code"),
                    Order = (int)s.Element("Order"),
                    Label = (string)s.Element("Text").Element("Label")
                })
                .ToList();

            return selections;
        }

        /// <summary>
        /// Parse XML resource family elements to AccessibilityResourceFamilies
        /// </summary>
        /// <param name="resourceFamilies"></param>
        /// <param name="globalResources"></param>
        /// <returns></returns>
        public static IList<AccessibilityResourceFamily> ToAccessibilityResourceFamilies(this IEnumerable<XElement> resourceFamilies, List<AccessibilityResource> globalResources)
        {
            List<AccessibilityResourceFamily> families = resourceFamilies
                .Select(f => new AccessibilityResourceFamily
                {
                    Subjects = f.Elements("Subject")
                        .Select(s => (string)s.Element("Code"))
                        .ToList(),
                    Grades = f.Elements("Grade")
                        .Select(g => g.Value)
                        .ToList()
                        .ToGradeLevels(),
                    Resources = f.Elements("SingleSelectResource").ToAccessibilityResources(globalResources)
                }).ToList();

            return families;
        }

        /// <summary>
        /// Parse XML Family accessibility resources and attach global resources to Accessiblity Resources
        /// </summary>
        /// <param name="xmlResources"></param>
        /// <param name="globalResources"></param>
        /// <returns></returns>
        public static List<AccessibilityResource> ToAccessibilityResources(this IEnumerable<XElement> xmlFamilyResources, List<AccessibilityResource> globalResources)
        {
            List<AccessibilityResource> partialResources = xmlFamilyResources
                .Select(r => new AccessibilityResource
                {
                    Code = (string)r.Element("Code"),
                    Disabled = (r.Element("Disabled") != null) ? true : false,
                    Selections = r.Elements("Selection")
                        .Select(s => new AccessibilitySelection
                        {
                            Code = (string)s.Element("Code"),
                            Label = (string)s.Element("Text")?.Element("Label")
                        })
                        .ToList()
                }).ToList();

            return partialResources.ToAccessibilityResources(globalResources);
        }

        /// <summary>
        /// Translates Partial family resources with global resources to full set of family resources
        /// </summary>
        /// <param name="partialResources"></param>
        /// <param name="globalResources"></param>
        /// <returns></returns>
        public static List<AccessibilityResource> ToAccessibilityResources(this List<AccessibilityResource> partialResources, List<AccessibilityResource> globalResources)
        {
            var resources = globalResources.Select(globalResource =>
            {
                var familyResource = partialResources.FirstOrDefault(fr => fr.Code == globalResource.Code);
                if (familyResource == null)
                {
                    return globalResource.DeepClone();
                }
                else
                {
                    return familyResource.ToAccessibilityResource(globalResource);
                }
            }).ToList();

            return resources;
        }

        /// <summary>
        /// Copies a Global AccessibilityResource based on Family Resource values and selections.
        /// </summary>
        /// <param name="partialResource"> Accessbility Family Resources </param>
        /// <param name="globalResource"> Global Accessiblity Resources </param>
        /// <returns></returns>
        public static AccessibilityResource ToAccessibilityResource(this AccessibilityResource partialResource, AccessibilityResource globalResource)
        {
            if (partialResource == null)
                throw new ArgumentNullException(nameof(partialResource));
            else if (globalResource == null)
                throw new ArgumentNullException(nameof(globalResource));

            AccessibilityResource resource = globalResource.DeepClone();

            // <Disabled /> means the entire resource is disabled
            bool isResourceDisabled = partialResource.Disabled;

            resource.Disabled = isResourceDisabled;

            foreach (AccessibilitySelection selection in resource.Selections)
            {
                AccessibilitySelection partialResourceSelection = partialResource.Selections?.SingleOrDefault(s => s.Code == selection.Code);
                selection.Disabled = (partialResourceSelection == null) || isResourceDisabled;
                selection.Label = (string.IsNullOrEmpty(partialResourceSelection?.Label)) ? selection.Label : partialResourceSelection?.Label;
            }

            // If the default select item is disabled, pick a different one 
            if (!isResourceDisabled && resource.Selections != null
                && resource.Selections.Any(s => s.Code == resource.DefaultSelection && s.Disabled))
            {
                resource.DefaultSelection = resource.Selections?.FirstOrDefault(s => !s.Disabled)?.Code;
            }

            return resource;
        }

    }

}
