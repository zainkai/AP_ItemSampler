using SmarterBalanced.SampleItems.Dal.Models;
using Gen = SmarterBalanced.SampleItems.Dal.Models.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class AccessibilityResourceTranslation
    {
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
            var things = generatedFamily.SingleSelectResource.Select(r => r.ToAccessibilityResource(globalResources));

            var selection = new AccessibilityResourceFamily
            {
                Codes = generatedFamily.Subject.Select(s => s.Code).ToList(),
                Grades = generatedFamily.Grade,
                
            };

            return selection;
        }

        public static AccessibilityResource ToAccessibilityResource(this Gen.AccessibilityResourceFamilySingleSelectResource generatedResource, IList<AccessibilityResource> globalResources)
        {
            // sort it out
            if (generatedResource.Disabled == null)
            {
                // filter out selections from the matching global resource
            }
            else
            {
                // include only selections from the matching global resource
            }

            return null;
        }
    }
}
