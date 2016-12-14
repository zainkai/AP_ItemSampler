using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class ItemPrimitive
    {
        public int BankKey { get; set; }
        public int ItemKey { get; set; }
        public string ClaimId { get; set; }
        public string TargetId { get; set; }
        public string SubjectId { get; set; }
        public string CommonCoreStandardsId { get; set; }
        public string ItemType { get; set; }
        public string TargetAssessmentType { get; set; }
        public string InteractionTypeCode { get; set; }
        public string SufficentEvidenceOfClaim { get; set; }
        public int? AssociatedStimulus { get; set; }
    }
}
