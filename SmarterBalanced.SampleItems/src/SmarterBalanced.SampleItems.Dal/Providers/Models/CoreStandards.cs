using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class CoreStandards
    {
        public Target Target { get; }
        public string CommonCoreStandardsId { get; }
        public string CommonCoreStandardsDescription { get; }
        public string ClaimId { get; }
        public string Publication { get; }
        public string Subject { get; }

        public CoreStandards(
            Target target,
            string commonCoreStandardsId,
            string commonCoreStandardsDescription,
            string claimId,
            string publication,
            string subject)
        {
            Target = target;
            CommonCoreStandardsId = commonCoreStandardsId;
            CommonCoreStandardsDescription = commonCoreStandardsDescription;
            ClaimId = claimId;
            Publication = publication;
            Subject = subject;
        }

        public static CoreStandards Create(
          Target target,
          string commonCoreStandardsId = "",
          string commonCoreStandardsDescription = "",
          string claimId = "",
          string publication = "",
          string subject = "")
        {
            return new CoreStandards(
                target: target,
                commonCoreStandardsId: commonCoreStandardsId,
                commonCoreStandardsDescription: commonCoreStandardsDescription,
                claimId: claimId,
                publication: publication,
                subject: subject);
        }

        public CoreStandards WithTargetCCSSDescriptions(
         string targetDescription = "",
         string commonCoreStandardsDescription = "")
        {
            return new CoreStandards(
                target: Target.WithDescription(targetDescription),
                commonCoreStandardsId: CommonCoreStandardsId,
                commonCoreStandardsDescription: commonCoreStandardsDescription,
                claimId : ClaimId,
                publication: Publication,
                subject: Subject);
        }

        
    }
}
