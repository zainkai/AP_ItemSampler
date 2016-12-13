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
    public class ItemDigest
    {
        public int BankKey { get; set; }
        public int ItemKey { get; set; }
        public string Subject { get; set; }
        public GradeLevels Grade { get; set; }
        public string SufficentEvidenceOfClaim { get; set; }
        public string TargetAssessmentType { get; set; }
        public string InteractionTypeCode { get; set; }
        public string InteractionTypeLabel { get; set; }
        public int? AssociatedStimulus { get; set; }
        public List<AccessibilityResource> AccessibilityResources { get; set; }
        public string ItemType { get; set; }
        public string ClaimId { get; set; }
        public string TargetId { get; set; }
        public string CommonCoreStandardsId { get; set;}
        public string Name { get; set; }
    }
}
