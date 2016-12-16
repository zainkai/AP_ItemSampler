using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    /// <summary>
    /// Collection of known attributes of a Smarter Balanced item
    /// </summary>
    public sealed class ItemDigest
    {
        public int BankKey { get; set; }
        public int ItemKey { get; set; }
        public string ItemType { get; set; }

        public string TargetId { get; set; }
        public string CommonCoreStandardsId { get; set; }

        public GradeLevels Grade { get; set; }
        public Subject Subject { get; set; }
        public Claim Claim { get; set; }
        public InteractionType InteractionType { get; set; }

        public List<AccessibilityResource> AccessibilityResources { get; set; }
        public string TargetAssessmentType { get; set; }
        public string SufficentEvidenceOfClaim { get; set; }
        public int? AssociatedStimulus { get; set; }

    }
}
