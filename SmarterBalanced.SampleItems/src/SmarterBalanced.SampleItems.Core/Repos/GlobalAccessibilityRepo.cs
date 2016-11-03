using SmarterBalanced.SampleItems.Dal.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Core.Translations;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public class GlobalAccessibilityRepo : IGlobalAccessibilityRepo
    {
        private SampleItemsContext context;
        public GlobalAccessibilityRepo(SampleItemsContext context)
        {
            this.context = context;
        }
        
        /// <summary>
        /// Gets AppSettings
        /// </summary>
        /// <returns></returns>
        public AppSettings GetSettings()
        {
            return context.AppSettings();
        }

        /// <summary>
        /// Constructs a GlobalAccessibilityViewModel with ISSAP codes
        /// as the default selected values if ISSAPCode has a value.
        /// </summary>
        /// <param name="iSSAPCode"></param>
        /// <returns></returns>
        public GlobalAccessibilityViewModel GetGlobalAccessibilityViewModel(string iSSAPCode)
        {
            List<AccessibilityResource> globalAccResources = new List<AccessibilityResource>(context.GlobalAccessibilityResources);

            List<AccessibilityResourceViewModel> accResourceViewModels;
            if (!string.IsNullOrEmpty(iSSAPCode))
            {
                accResourceViewModels = AccessibilityTranslations.ToAccessibilityResourceViewModels(globalAccResources, iSSAPCode);
            }
            else
            {
                accResourceViewModels = AccessibilityTranslations.ToAccessibilityResourceViewModels(globalAccResources);
            }

            return new GlobalAccessibilityViewModel()
            {
                AccessibilityResourceViewModels = accResourceViewModels
            };
        }

        /// <summary>
        /// Generates an ISSAP code from a GlobalAccessibilityViewModel
        /// </summary>
        /// <param name="globalAccessibilityViewModel"></param>
        /// <returns>a ISSAP code string.</returns>
        public string GetISSAPCode(GlobalAccessibilityViewModel globalAccessibilityViewModel)
        {
            if(globalAccessibilityViewModel?.AccessibilityResourceViewModels == null)
            {
                throw new Exception("Invalid accessibility");
            }

            return globalAccessibilityViewModel?.AccessibilityResourceViewModels.ToISSAP();
        }
    }
}
