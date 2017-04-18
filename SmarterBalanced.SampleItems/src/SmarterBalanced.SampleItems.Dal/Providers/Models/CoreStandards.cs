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
        public string Publication { get; }

        public CoreStandards(
            string targetDescription,
            string targetId,
            string targetIdLabel,
            string commonCoreStandardsId,
            string commonCoreStandardsDescription,
            string claimId,
            string publication)
        {
            TargetDescription = targetDescription;
            TargetId = targetId;
            TargetIdLabel = targetIdLabel;
            CommonCoreStandardsId = commonCoreStandardsId;
            CommonCoreStandardsDescription = commonCoreStandardsDescription;
            ClaimId = claimId;
            Publication = publication;
        }

        public static CoreStandards Create(
          string targetDescription = "",
          string targetId = "",
          string targetIdLabel = "",
          string commonCoreStandardsId = "",
          string commonCoreStandardsDescription = "",
          string claimId = "",
          string publication = "")
        {
            return new CoreStandards(
                targetDescription: targetDescription,
                targetId: targetId,
                targetIdLabel:  targetIdLabel,
                commonCoreStandardsId: commonCoreStandardsId,
                commonCoreStandardsDescription: commonCoreStandardsDescription,
                claimId: claimId,
                publication: publication);
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
                claimId : ClaimId,
                publication: Publication);
        }


    }
}
