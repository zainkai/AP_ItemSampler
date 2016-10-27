using Microsoft.AspNetCore.Mvc.Rendering;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Translations
{
    public static class AccessibilityTranslations
    {
        /// <summary>
        /// Translates a list of AccessibilityResourceViewModels into
        /// a comma-separated string of ISSAP codes.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>a comma-separated string of ISSAP codes.</returns>
        public static string ToISSAP(this List<AccessibilityResourceViewModel> items)
        {
            return string.Join(";", items.Select(t => t.SelectedCode));
        }

        /// <summary>
        /// Converts ISSAP format to list of strings
        /// </summary>
        /// <param name="ISSAPCode"></param>
        /// <returns>List of strings</returns>
        public static List<string> ToISSAPList(string ISSAPCode)
        {
            if (string.IsNullOrEmpty(ISSAPCode))
            {
                return new List<string>();
            }
            return ISSAPCode.Split(';').ToList();
        }

        /// <summary>
        /// Translates a List of AccessibilitySelections into a List of SelectListItems,
        /// setting default values if applicable.
        /// </summary>
        /// <param name="accessibilitySelections"></param>
        /// <returns>a List of SelectListItems.</returns>
        private static List<SelectListItem> ToSelectListItems(List<AccessibilitySelection> accessibilitySelections)
        {
            if(accessibilitySelections == null)
            {
                accessibilitySelections = new List<AccessibilitySelection>();
            }

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems = accessibilitySelections.OrderBy(t => t.Order).Select(t => new SelectListItem
            {
                Disabled = (t.Disabled.HasValue) ? t.Disabled.Value : false,
                Value = t.Code,
                Text = t.Label
            }).ToList();

            return selectListItems;
        }

        /// <summary>
        /// Translates a List of AccessibilityResources into a List of 
        /// AccessibilityResourceViewModels.
        /// </summary>
        /// <param name="accessibilityResources"></param>
        /// <returns>a List of AccessibilityResourceViewModels.</returns>
        public static List<AccessibilityResourceViewModel> ToAccessibilityResourceViewModels(List<AccessibilityResource> accessibilityResources)
        {
            List<AccessibilityResourceViewModel> accessibilityResourceViewModels = new List<AccessibilityResourceViewModel>();
            accessibilityResourceViewModels = accessibilityResources.Select(t => new AccessibilityResourceViewModel
            {
                SelectedCode = t.DefaultSelection,
                Label = t.Label,
                Description = t.Description,
                AccessibilityListItems = ToSelectListItems(t?.Selections),
                Disabled = (t.Disabled.HasValue) ? t.Disabled.Value : false
            }).ToList();

            return accessibilityResourceViewModels;
        }

        /// <summary>
        /// Translates a List of AccessibilityResources into a List of 
        /// AccessibilityResourceVIewModels with defaults set from the ISSAP code.
        /// </summary>
        /// <param name="accessibilityResources"></param>
        /// <param name="ISSAPCode"></param>
        /// <returns></returns>
        public static List<AccessibilityResourceViewModel> ToAccessibilityResourceViewModels(this List<AccessibilityResource> accessibilityResources, string ISSAPCode)
        {
            var accResourcesViewModels = ToAccessibilityResourceViewModels(accessibilityResources);
            var codes = ToISSAPList(ISSAPCode);

            foreach(var accResourceViewModel in accResourcesViewModels)
            {
                var accListItems = accResourceViewModel.AccessibilityListItems;
                var accListItem = accListItems.SingleOrDefault(t => codes.Contains(t.Value));
                if(accListItem != null)
                {
                    var selectedCode = accListItem.Value;
                    
                    accResourceViewModel.SelectedCode = selectedCode;
                    codes.Remove(selectedCode);
                }
            }

            return accResourcesViewModels;
        }

    }
}
