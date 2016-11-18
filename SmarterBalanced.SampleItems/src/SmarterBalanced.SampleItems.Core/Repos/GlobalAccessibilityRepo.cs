using SmarterBalanced.SampleItems.Dal.Providers;
using System;
using System.Collections.Generic;
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
            return context.AppSettings;
        }

        /// <summary>
        /// Constructs a GlobalAccessibilityViewModel with ISAAP codes
        /// as the default selected values if ISAAPCode has a value.
        /// </summary>
        /// <param name="iSAAPCode"></param>
        /// <returns></returns>
        public Task<GlobalAccessibilityViewModel> GetGlobalAccessibilityViewModelAsync(string iSAAPCode)
        {
            return Task.Run(() =>
            {
                List<AccessibilityResource> globalAccResources = new List<AccessibilityResource>(context.GlobalAccessibilityResources);

                List<AccessibilityResourceViewModel> accResourceViewModels;
                if (!string.IsNullOrEmpty(iSAAPCode))
                {
                    accResourceViewModels = globalAccResources.ToAccessibilityResourceViewModels(iSAAPCode);
                }
                else
                {
                    accResourceViewModels = globalAccResources.ToAccessibilityResourceViewModels();
                }

                return new GlobalAccessibilityViewModel()
                {
                    AccessibilityResourceViewModels = accResourceViewModels
                };
            });
        }

        /// <summary>
        /// Generates an ISAAP code from a GlobalAccessibilityViewModel
        /// </summary>
        /// <param name="globalAccessibilityViewModel"></param>
        /// <returns>a ISAAP code string.</returns>
        public Task<string> GetISAAPCodeAsync(GlobalAccessibilityViewModel globalAccessibilityViewModel)
        {
            if (globalAccessibilityViewModel?.AccessibilityResourceViewModels == null)
            {
                throw new Exception("Invalid accessibility");
            }

            return Task.Run(() =>
            {
                return globalAccessibilityViewModel?.AccessibilityResourceViewModels.ToISAAP();
            });
        }
    }
}
