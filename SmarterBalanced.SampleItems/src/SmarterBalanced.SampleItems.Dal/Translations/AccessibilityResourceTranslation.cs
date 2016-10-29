using SmarterBalanced.SampleItems.Dal.Providers.Models;
using Gen = SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class AccessibilityResourceTranslation
    {
        public static IList<AccessibilityResource> ToAccessibilityResources(this Gen.Accessibility generatedAccessibility)
        {
            var accessibilityResources = generatedAccessibility.MasterResourceFamily
                .OfType<Gen.AccessibilitySingleSelectResource>()
                .Where(r => r.Selection != null)
                .Select(r => r.ToAccessibilityResource())
                .ToList();

            return accessibilityResources;
        }

        public static AccessibilityResource ToAccessibilityResource(this Gen.AccessibilitySingleSelectResource generatedResource)
        {
            var text = generatedResource.Text.First();

            var resource = new AccessibilityResource
            {
                Code = generatedResource.Code,
                DefaultSelection = generatedResource.DefaultSelection as string,
                Description = text.Description,
                Label = text.Label,
                Order = Convert.ToInt32(generatedResource.Order),
                Selections = generatedResource.Selection.Select(s => s.ToAccessibilitySelection()).ToList()
            };

            return resource;
        }

        public static AccessibilitySelection ToAccessibilitySelection(this Gen.AccessibilitySingleSelectResourceSelection generatedSelection)
        {
            var text = generatedSelection.Text.First();

            var selection = new AccessibilitySelection
            {
                Code = generatedSelection.Code,
                Label = text.Label,
                Order = Convert.ToInt32(generatedSelection.Order)
            };

            return selection;
        }



        public static AccessibilityResourceFamily ToAccessibilityResourceFamily(this Gen.AccessibilityResourceFamily generatedFamily, IList<AccessibilityResource> globalResources)
        {
            var resources = generatedFamily.SingleSelectResource
                .Select(r => r.ToAccessibilityResource(globalResources.FirstOrDefault(gr => gr.Code == r.Code)))
                .ToList();

            var selection = new AccessibilityResourceFamily
            {
                Codes = generatedFamily.Subject.Select(s => s.Code).ToList(),
                Grades = generatedFamily.Grade,
                Resources = resources
            };

            return selection;
        }

        public static AccessibilityResource ToAccessibilityResource(this Gen.AccessibilityResourceFamilySingleSelectResource generatedResource, AccessibilityResource globalResource)
        {
            if (globalResource == null)
                throw new ArgumentNullException(nameof(globalResource));

            List<AccessibilitySelection> familySelections;
            if (generatedResource.Disabled == null)
            {
                familySelections = globalResource.Selections
                    .Where(s => generatedResource.Selection.Any(gens => gens.Code == s.Code))
                    .ToList();
            }
            else
            {
                familySelections = globalResource.Selections
                    .Where(s => generatedResource.Selection != null && generatedResource.Selection.All(gens => gens.Code != s.Code))
                    .ToList();
            }

            var completeResource = new AccessibilityResource
            {
                Code = globalResource.Code,
                DefaultSelection = generatedResource.DefaultSelection as string,
                Description = globalResource.Description,
                Label = globalResource.Label,
                Order = globalResource.Order,
                Selections = familySelections
            };
            return completeResource;
        }
    }
}
