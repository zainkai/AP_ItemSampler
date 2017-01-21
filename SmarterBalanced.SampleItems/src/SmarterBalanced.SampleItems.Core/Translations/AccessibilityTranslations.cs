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
        public static ImmutableArray<AccessibilityResourceGroup> ApplyPreferences(
            this ImmutableArray<AccessibilityResourceGroup> groups,
            string[] isaap,
            Dictionary<string, string> cookie)
        {
            if (isaap == null) throw new ArgumentNullException(nameof(isaap));
            if (groups == null) throw new ArgumentNullException(nameof(groups));
            if (cookie == null) throw new ArgumentNullException(nameof(cookie));

            if (isaap.Length != 0)
            {
                var isaapGroups = groups
                    .Select(g => g.WithResources(g.AccessibilityResources
                        .Select(r => r.ApplyIsaap(isaap))
                        .ToImmutableArray()))
                    .ToImmutableArray();

                return isaapGroups;
            }
            else
            {
                var cookieGroups = groups
                    .Select(g => g.WithResources(g.AccessibilityResources
                        .Select(r => r.ApplyCookie(cookie))
                        .ToImmutableArray()))
                    .ToImmutableArray();

                return cookieGroups;
            }
        }

        private static AccessibilityResource ApplyIsaap(this AccessibilityResource resource, string[] isaap)
        {
            var newSelection = resource.Selections.FirstOrDefault(sel => isaap.Contains(sel.Code));
            if (newSelection == null)
            {
                return resource;
            }

            var newResource = resource.WithSelectedCode(newSelection.Code);
            return newResource;
        }

        private static AccessibilityResource ApplyCookie(this AccessibilityResource resource, Dictionary<string, string> cookie)
        {
            string newSelectedCode;
            if (cookie.TryGetValue(resource.Code, out newSelectedCode))
            {
                var newResource = resource.WithSelectedCode(newSelectedCode);
                return newResource;
            }

            return resource;
        }
    }
    
}
