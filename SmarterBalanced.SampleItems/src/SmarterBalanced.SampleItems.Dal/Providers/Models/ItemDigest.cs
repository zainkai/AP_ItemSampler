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
        public int? AssociatedStimulus { get; set; }

        public Lazy<List<AccessibilityResource>> ApplicableAccessibilityResources { get; }

        public ItemDigest() { }

        public ItemDigest(IList<AccessibilityResource> globalResources, IList<AccessibilityResourceFamily> resourceFamilies)
        {
            ApplicableAccessibilityResources = new Lazy<List<AccessibilityResource>>(
                () => BuildApplicableAccessibilityResources(globalResources, resourceFamilies), isThreadSafe: true);
        }

        private AccessibilityResource DisableNonApplicableAccessibility(AccessibilityResource globalResource, List<AccessibilityResource> itemResources)
        {
            AccessibilityResource resource;
            if (itemResources.Where(r => (r.Code == globalResource.Code)).Any())
            {
                AccessibilityResource applicableResource = itemResources.Where(r => r.Code == globalResource.Code).First();
                List<AccessibilitySelection> selections = new List<AccessibilitySelection>();
                foreach (AccessibilitySelection selection in globalResource.Selections)
                {
                    if (applicableResource.Selections.Select(r => r.Code == selection.Code).Any())
                    {
                        AccessibilitySelection enabledSelection = selection.Clone();
                        enabledSelection.Disabled = false;
                        selections.Add(enabledSelection);
                    }
                    else
                    {
                        AccessibilitySelection disabledSelection = selection.Clone();
                        disabledSelection.Disabled = true;
                        selections.Add(disabledSelection);
                    }
                }
                resource = globalResource.DeepClone();
                resource.Disabled = false;
                resource.Selections = selections;
            }
            else
            {
                resource = globalResource.DeepClone();
                resource.Disabled = true;
                foreach(AccessibilitySelection selection in resource.Selections)
                {
                    selection.Disabled = true;
                }
            }
            return resource;
        } 

        private List<AccessibilityResource> BuildApplicableAccessibilityResources(IList<AccessibilityResource> globalResources, IList<AccessibilityResourceFamily> resourceFamilies)
        {
            var localResources = new List<AccessibilityResource>();
            //Get all accessibilty options for a specific item based off of its family.
            List<AccessibilityResourceFamily> applicableResourceFamilies;
            if (Grade == "NA")
            {
                applicableResourceFamilies = resourceFamilies.Where(s => (s.Codes.Contains(Subject))).ToList();
            }
            else
            {
                var enumGrade = Enum.Parse(typeof(GradeType), Grade);
                applicableResourceFamilies = resourceFamilies.Where(s => (s.Codes.Contains(Subject)
                    && (s.Grades.ToList().Contains((GradeType)enumGrade)))).ToList();
            }

            //TODO: Should we throw an exception or default to the global accommodations for an item if it does not belong to a family
            if (applicableResourceFamilies.Count != 1)
            {
                throw new Exception($"Item ID: {ItemKey} Bank: {BankKey} does not belong to any accessibility resource families.");
            }

            foreach(AccessibilityResource globalResource in globalResources)
            {
                localResources.Add(DisableNonApplicableAccessibility(globalResource, applicableResourceFamilies.First().Resources));
            }
            return localResources;
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
