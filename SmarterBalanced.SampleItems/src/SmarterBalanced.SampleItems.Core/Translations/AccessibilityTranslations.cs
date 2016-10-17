using SmarterBalanced.SampleItems.Core.Repos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Translations
{
    public static class AccessibilityTranslations
    {
        public static string ToISSAP(this List<AccessibilityResourceViewModel> items)
        {
            return string.Join(";", items.Select(t => t.SelectedCode));
        }
    }
}
