using Microsoft.AspNetCore.Mvc.Rendering;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Translations
{
    public static class AccessibilityTranslations
    {
        /// <summary>
        /// Translates a list of AccessibilityResourceViewModels into
        /// a comma-separated string of ISAAP codes.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>a comma-separated string of ISAAP codes.</returns>
        public static string ToISAAP(this List<AccessibilityResource> items)
        {
            if(items == null)
                throw new ArgumentNullException(nameof(items));

            return string.Join(";", items.Select(t => t.SelectedCode));
        }

        /// <summary>
        /// Converts ISAAP format to list of strings
        /// </summary>
        /// <param name="iSAAPCode"></param>
        /// <returns>List of strings</returns>
        public static List<string> ToISAAPList(string iSAAPCode)
        {
            if (string.IsNullOrEmpty(iSAAPCode))
                return new List<string>();

            return iSAAPCode.Split(';').ToList();
        }

        /// <summary>
        /// Translates a List of AccessibilityResources into a List of 
        /// AccessibilityResourceVIewModels with defaults set from the ISAAP code.
        /// </summary>
        /// <param name="accessibilityResources"></param>
        /// <param name="iSAAPCode"></param>
        /// <returns>a List of AccessibilityResources.</returns>
        private static ImmutableArray<AccessibilityResource> ApplyIsaap(ImmutableArray<AccessibilityResource> accessibilityResources, string[] codes)
        {
            if (accessibilityResources == null)
            {
                throw new ArgumentNullException(nameof(accessibilityResources));
            }

            List<AccessibilityResource> newResources = new List<AccessibilityResource>();

            foreach (var resource in accessibilityResources)
            {
                var newResource = resource.DeepClone();
                var accListItems = newResource.Selections;
                var accListItem = accListItems.FirstOrDefault(sel => codes.Contains(sel.Code));
                if (accListItem != null)
                {
                    var selectedCode = accListItem.Code;
                    newResource.SelectedCode = selectedCode;
                }
                else
                {
                    newResource.SelectedCode = newResource.DefaultSelection;
                }
                newResources.Add(newResource);
            }

            return newResources.ToImmutableArray();
        }

        public static ImmutableArray<AccessibilityResourceGroup> SetIsaap(this ImmutableArray<AccessibilityResourceGroup> resourceGroups, string[] codes)
        {
            List<AccessibilityResourceGroup> groups = new List<AccessibilityResourceGroup>();
            foreach(AccessibilityResourceGroup group in resourceGroups)
            {
                var newResources = ApplyIsaap(group.AccessibilityResources, codes);
                groups.Add(new AccessibilityResourceGroup(
                        label: group.Label,
                        order: group.Order,
                        accessibilityResources: newResources
                    ));
            }
            return groups.ToImmutableArray();
        }
    }
    
}
