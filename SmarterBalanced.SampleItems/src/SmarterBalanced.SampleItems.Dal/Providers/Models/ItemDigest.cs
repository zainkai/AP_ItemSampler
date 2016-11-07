using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    /// <summary>
    /// Flattened digest of an ItemMetadata object and an ItemContents object
    /// </summary>
    public class ItemDigest : IEquatable<ItemDigest>
    {
        [Display(Name = "Bank")]
        public int BankKey { get; set; }

        [Display(Name = "Item Key")]
        public int ItemKey { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Grade")]
        public GradeLevels Grade { get; set; }

        [Display(Name = "Claim")]
        public string Claim { get; set; }

        [Display(Name = "Target Assessment Type")]
        public string Target { get; set; }

        [Display(Name = "Interaction Type")]
        public string InteractionTypeCode { get; set; }

        [Display(Name = "Interaction Type")]
        public string InteractionTypeLabel { get; set; }

        [Display(Name = "AssociatedStimulus")]
        public int? AssociatedStimulus { get; set; }

        public List<AccessibilityResource> AccessibilityResources { get; set; }

        public string Name { get; set; }
        public bool Equals(ItemDigest obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return ($"{BankKey}-{ItemKey}").GetHashCode();
        }

    }

}
