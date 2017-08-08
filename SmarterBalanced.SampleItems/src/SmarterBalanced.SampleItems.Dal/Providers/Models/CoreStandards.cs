using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class CoreStandards
    {
        public string TargetDescription { get; }
        public string TargetId { get; }
        public string TargetIdLabel { get; }
        public string TargetShortName { get; }
        public string CommonCoreStandardsId { get; }
        public string CommonCoreStandardsDescription { get; }
        public string ClaimId { get; }
        public string Publication { get; }
        public string Subject { get; }

        public CoreStandards(
            string targetDescription,
            string targetId,
            string targetIdLabel,
            string commonCoreStandardsId,
            string commonCoreStandardsDescription,
            string claimId,
            string publication,
            string subject)
        {
            TargetDescription = RemoveShortNameFromDescription(targetDescription);
            TargetId = targetId;
            TargetIdLabel = targetIdLabel;
            TargetShortName = TargetShortNameFromDesc(targetDescription);
            CommonCoreStandardsId = commonCoreStandardsId;
            CommonCoreStandardsDescription = commonCoreStandardsDescription;
            ClaimId = claimId;
            Publication = publication;
            Subject = subject;
        }

        public static CoreStandards Create(
          string targetDescription = "",
          string targetId = "",
          string targetIdLabel = "",
          string commonCoreStandardsId = "",
          string commonCoreStandardsDescription = "",
          string claimId = "",
          string publication = "",
          string subject = "")
        {
            return new CoreStandards(
                targetDescription: targetDescription,
                targetId: targetId,
                targetIdLabel:  targetIdLabel,
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
                targetDescription: targetDescription,
                targetId: TargetId,
                targetIdLabel: TargetIdLabel,
                commonCoreStandardsId: CommonCoreStandardsId,
                commonCoreStandardsDescription: commonCoreStandardsDescription,
                claimId : ClaimId,
                publication: Publication,
                subject: Subject);
        }

        /// <summary>
        /// Get the target's short name from the target description string.
        /// </summary>
        /// <param name="targetDesc">The target description</param>
        /// <returns>Target's short name or an empty string</returns>
        private string TargetShortNameFromDesc(string targetDesc)
        {
            if (!string.IsNullOrEmpty(targetDesc) && targetDesc.Contains(':'))
            {
                int colonLocation = targetDesc.IndexOf(':');
                string shortName = targetDesc.Substring(0, colonLocation);
                shortName = String.Join(" ",
                    shortName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
                return shortName;
            }
            return "";
        }

        private string RemoveShortNameFromDescription(string targetDesc)
        {
            if (!string.IsNullOrEmpty(targetDesc) && targetDesc.Contains(':'))
            {
                int colonLocation = targetDesc.IndexOf(':');
                if (targetDesc.Length >= colonLocation + 2)
                {
                    return targetDesc.Substring(colonLocation + 2);
                }
            }
            return targetDesc;
        }
    }
}
