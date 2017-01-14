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

        public static AccessibilitySelectionViewModel ToAccessibilitySelectionViewModel(this AccessibilitySelection selection)
        {
            var selectionVM = new AccessibilitySelectionViewModel(selection.Code, selection.Label, selection.Disabled);
            return selectionVM;
        }

        /// <summary>
        /// Translates a List of AccessibilityResources into a List of 
        /// AccessibilityResourceViewModels.
        /// </summary>
        /// <param name="accessibilityResources"></param>
        /// <returns>a List of AccessibilityResourceViewModels.</returns>
        public static List<AccessibilityResourceViewModel> ToAccessibilityResourceViewModels(this List<AccessibilityResource> accessibilityResources)
        {
            var accessibilityResourceViewModels =
                accessibilityResources
                    .Select(ar => new AccessibilityResourceViewModel
                        {
                            SelectedCode = ar.DefaultSelection,
                            DefaultCode = ar.DefaultSelection,
                            Label = ar.Label,
                            Description = ar.Description,
                            Selections = ar.Selections.Select(ToAccessibilitySelectionViewModel).ToList(),
                            Disabled = ar.Disabled,
                            ResourceType = (ar.ResourceType == "DesignatedSupport") ? "Designated Support" : ar.ResourceType,
                        })
                    .ToList();

            return accessibilityResourceViewModels;
        }

        /// <summary>
        /// Translates a List of AccessibilityResources into a List of 
        /// AccessibilityResourceVIewModels with defaults set from the ISAAP code.
        /// </summary>
        /// <param name="accessibilityResources"></param>
        /// <param name="iSAAPCode"></param>
        /// <returns>a List of AccessibilityResources.</returns>
        public static List<AccessibilityResourceViewModel> ToAccessibilityResourceViewModels(this List<AccessibilityResource> accessibilityResources, string[] codes)
        {
            if (accessibilityResources == null)
            {
                throw new ArgumentNullException(nameof(accessibilityResources));
            }

            var accResourceViewModels = ToAccessibilityResourceViewModels(accessibilityResources);

            foreach (var accResourceViewModel in accResourceViewModels)
            {
                var accListItems = accResourceViewModel.Selections;
                var accListItem = accListItems.FirstOrDefault(sel => codes.Contains(sel.Code));
                if (accListItem != null)
                {
                    var selectedCode = accListItem.Code;
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
