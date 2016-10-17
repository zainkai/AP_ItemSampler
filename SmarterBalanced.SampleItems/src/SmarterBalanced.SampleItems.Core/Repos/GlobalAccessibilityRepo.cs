using SmarterBalanced.SampleItems.Dal.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Core.Translations;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public class GlobalAccessibilityRepo : IGlobalAccessibilityRepo
    {
        private SampleItemsContext context;
        public GlobalAccessibilityRepo(SampleItemsContext context)
        {
            this.context = context;
        }

        public GlobalAccessibilityViewModel GetGlobalAccessibilityViewModel()
        {
            throw new NotImplementedException();
        }

        public GlobalAccessibilityViewModel GetGlobalAccessibilityViewModel(string ISSAPCode)
        {
            throw new NotImplementedException();
        }

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
