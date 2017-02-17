using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class StandardIdentifier
    {
        public string Claim { get; }
        public string ContentDomain { get; }
        public string ContentCategory { get; }
        public string Target { get; }
        public string TargetSet { get; }
        public string Emphasis { get; }
        public string CommonCoreStandard { get; }
        public string SubjectCode { get; }

        public StandardIdentifier(
            string claim,
            string target,
            string contentDomain,
            string contentCategory,
            string targetSet,
            string emphasis,
            string commonCoreStandard,
            string subjectCode)
        {
            Claim = claim;
            ContentDomain = contentDomain;
            ContentCategory = contentCategory;
            Target = target;
            TargetSet = targetSet;
            Emphasis = emphasis;
            CommonCoreStandard = commonCoreStandard;
            SubjectCode = subjectCode;
        }

        public static StandardIdentifier Create(
            string claim = "",
            string target = "",
            string contentDomain = "",
            string contentCategory = "",
            string targetSet = "",
            string emphasis = "",
            string commonCoreStandard = "",
            string subjectCode = "")
        {
            return new StandardIdentifier(
            claim: claim,
            contentDomain:contentDomain,
            contentCategory: contentCategory,
            target: target,
            targetSet: targetSet,
            emphasis: emphasis,
            commonCoreStandard: commonCoreStandard,
            subjectCode: subjectCode);
        }
    }
}
