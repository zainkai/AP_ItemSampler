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
            var resources = globalResources.Select(globalResource =>
            {
                var familyResource = generatedFamily.SingleSelectResource.FirstOrDefault(fr => fr.Code == globalResource.Code);
                if (familyResource == null)
                {
                    return globalResource;
                }
                else
                {
                    return familyResource.ToAccessibilityResource(globalResource);
                }
            }).ToList();

            var selection = new AccessibilityResourceFamily
            {
                Subjects = generatedFamily.Subject.Select(s => s.Code).ToList(),
                Grades = generatedFamily.Grade.ToGradeLevels(),
                Resources = resources
            };

            return selection;
        }

        public static AccessibilityResource ToAccessibilityResource(this Gen.AccessibilityResourceFamilySingleSelectResource generatedResource, AccessibilityResource globalResource)
        {
            if (globalResource == null)
                throw new ArgumentNullException(nameof(globalResource));

            // <Disabled /> means the entire resource is disabled
            bool isResourceDisabled = generatedResource.Disabled != null;
            AccessibilityResource resource = globalResource.DeepClone();

            resource.DefaultSelection = generatedResource.DefaultSelection as string;
            resource.Disabled = isResourceDisabled;

            if (isResourceDisabled)
            {
                foreach (var selection in resource.Selections)
                {
                    selection.Disabled = true;
                }
            }
            else
            {
                // Individual selections are disabled by not being included in family resource selections
                // If the family's selections contains this selection, enable the selection. Otherwise disable it.
                resource.Selections = globalResource.Selections.Select(s =>
                    s.CloneWithDisabled(
                        !generatedResource.Selection.Any(fs => fs.Code == s.Code))).ToList();
            }

            return resource;
        }
    }
}
