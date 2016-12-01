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
        /// a comma-separated string of ISAAP codes.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>a comma-separated string of ISAAP codes.</returns>
        public static string ToISAAP(this List<AccessibilityResourceViewModel> items)
        {
            if(items == null)
            {
                throw new ArgumentNullException();
            }
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
            {
                return new List<string>();
            }
            return iSAAPCode.Split(';').ToList();
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
                Disabled = t.Disabled,
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
        public static List<AccessibilityResourceViewModel> ToAccessibilityResourceViewModels(this List<AccessibilityResource> accessibilityResources)
        {
            List<AccessibilityResourceViewModel> accessibilityResourceViewModels = new List<AccessibilityResourceViewModel>();
            accessibilityResourceViewModels = accessibilityResources.Select(t => new AccessibilityResourceViewModel
            {
                SelectedCode = t.DefaultSelection,
                DefaultCode = t.DefaultSelection,
                Label = t.Label,
                Description = t.Description,
                AccessibilityListItems = ToSelectListItems(t?.Selections),
                Disabled = t.Disabled
            }).ToList();

            return accessibilityResourceViewModels;
        }

        /// <summary>
        /// Translates a List of AccessibilityResources into a List of 
        /// AccessibilityResourceVIewModels with defaults set from the ISAAP code.
        /// </summary>
        /// <param name="accessibilityResources"></param>
        /// <param name="iSAAPCode"></param>
        /// <returns>a List of AccessibilityResources.</returns>
        public static List<AccessibilityResourceViewModel> ToAccessibilityResourceViewModels(this List<AccessibilityResource> accessibilityResources, string iSAAPCode)
        {
            if (accessibilityResources == null)
            {
                throw new ArgumentNullException();
            }

            var accResourceViewModels = ToAccessibilityResourceViewModels(accessibilityResources);
            if (accResourceViewModels == null)
            {
                return accResourceViewModels;
            }

            var codes = ToISAAPList(iSAAPCode);

            foreach (var accResourceViewModel in accResourceViewModels)
            {
                var accListItems = accResourceViewModel.AccessibilityListItems;
                var accListItem = accListItems.SingleOrDefault(t => codes.Contains(t.Value));
                if (accListItem != null)
                {
                    var selectedCode = accListItem.Value;
                    accResourceViewModel.SelectedCode = selectedCode;
                }
                else
                {
                    accResourceViewModel.SelectedCode = accResourceViewModel.DefaultCode;
                }
            }

            return accResourceViewModels;
        }


    }
    
}
