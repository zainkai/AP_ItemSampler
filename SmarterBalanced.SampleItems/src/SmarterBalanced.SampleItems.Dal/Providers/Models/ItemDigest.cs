using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    /// <summary>
    /// Flattened digest of an ItemMetadata object and an ItemContents object
    /// </summary>
    public class ItemDigest : IEquatable<ItemDigest>
    {
        [Display(Name="Bank")]
        public int BankKey { get; set; }

        [Display(Name = "Item Key")]
        public int ItemKey { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Grade")]
        public string Grade { get; set; }

        [Display(Name = "Claim")]
        public string Claim { get; set; }

        [Display(Name = "Target Assessment Type")]
        public string Target { get; set; }

        [Display(Name = "Interaction Type")]
        public string InteractionType { get; set; }

        [Display(Name = "AssociatedStimulus")]
        public int? AssociatedStimulus { get; }

        public List<AccessibilityResource> ApplicableAccessibilityResources { get; set; }

        public void SetApplicableAccessibilityResources(SampleItemsContext context)
        {
            //Get all accessibilty options for a specific item based off of its family.
            List<AccessibilityResourceFamily> applicableResourceFamilies;
            if (Grade == "NA")
            {
                applicableResourceFamilies = context.AccessibilityResourceFamilies.Where(s => (s.Codes.Contains(Subject))).ToList();
            }
            else
            {
                var enumGrade = Enum.Parse(typeof(GradeType), Grade);
                applicableResourceFamilies = context.AccessibilityResourceFamilies.Where(s => (s.Codes.Contains(Subject)
                    && (s.Grades.ToList().Contains((GradeType)enumGrade)))).ToList();
            }
            //Combine the resources for each resource family into one list
            foreach (AccessibilityResourceFamily resourceFamily in applicableResourceFamilies)
            {
                ApplicableAccessibilityResources = ApplicableAccessibilityResources.Union(resourceFamily.Resources).ToList();
            }

            //Get a list of all of the accessibility resources and selections that need to be disabled
            List<AccessibilityResource> disabledAccessibilityOptions = ApplicableAccessibilityResources.Intersect(context.GlobalAccessibilityResources).ToList();
            //set the disabled flag to true for all accessibility resources and selections that need to be disabled
            foreach (AccessibilityResource resouce in disabledAccessibilityOptions)
            {
                resouce.Disabled = true;
                foreach (AccessibilitySelection selection in resouce.Selections)
                {
                    selection.Disabled = true;
                }
            }
            ApplicableAccessibilityResources = ApplicableAccessibilityResources.Union(disabledAccessibilityOptions).ToList();
        }

        #region Helper Methods

        public override string ToString()
        {
            return $"{BankKey}-{ItemKey}";
        }
        public bool Equals(ItemDigest obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return ($"{BankKey}-{ItemKey}").GetHashCode();
        }

        #endregion

    }

}
