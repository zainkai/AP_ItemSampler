using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        private List<AccessibilityResource> ApplicableAccessibilityResources
        {
            get
            {
                return applicableAccessibilityResources.Value;
            }
        }

        public Lazy<List<AccessibilityResource>> applicableAccessibilityResources = new Lazy<List<AccessibilityResource>>(() => GenerateAccssibilityResources(), true);

        private static List<AccessibilityResource> GenerateAccssibilityResources()
        {
            throw new NotImplementedException();
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
