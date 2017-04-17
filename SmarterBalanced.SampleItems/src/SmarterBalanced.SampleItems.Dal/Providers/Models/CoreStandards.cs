using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class CoreStandards
    {
        public string TargetDescription { get; }
        public string TargetId { get; }
        public string TargetIdLabel { get; }
        public string CommonCoreStandardsId { get; }
        public string CommonCoreStandardsDescription { get; }
        public string ClaimId { get; }

        public CoreStandards(
            string targetDescription,
            string targetId,
            string targetIdLabel,
            string commonCoreStandardsId,
            string commonCoreStandardsDescription,
            string claimId)
        {
            TargetDescription = targetDescription;
            TargetId = targetId;
            TargetIdLabel = targetIdLabel;
            CommonCoreStandardsId = commonCoreStandardsId;
            CommonCoreStandardsDescription = commonCoreStandardsDescription;
            ClaimId = claimId;
        }

        public static CoreStandards Create(
          string targetDescription = "",
          string targetId = "",
          string targetIdLabel = "",
          string commonCoreStandardsId = "",
          string commonCoreStandardsDescription = "",
          string claimId = "")
        {
            return new CoreStandards(
                targetDescription: targetDescription,
                targetId: targetId,
                targetIdLabel:  targetIdLabel,
                commonCoreStandardsId: commonCoreStandardsId,
                commonCoreStandardsDescription: commonCoreStandardsDescription,
                claimId: claimId);
        }

        public CoreStandards WithTargetCCSSDescriptions(
         string targetDescription = "",
         string commonCoreStandardsDescription = "")
        {
            return new CoreStandards(
                targetDescription: targetDescription,
                targetId: TargetId,
                targetIdLabel: TargetIdLabel,
                commonCoreStandardsId: CommonCoreStandardsId,
                commonCoreStandardsDescription: commonCoreStandardsDescription,
                claimId : ClaimId);
        }


    }
}
