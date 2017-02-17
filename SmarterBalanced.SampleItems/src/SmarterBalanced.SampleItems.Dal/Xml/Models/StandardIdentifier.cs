using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class StandardIdentifier
    {
        public string SubjectCode { get; }
        public string Claim { get; }
        public string ContentDomain { get; }
        public string ContentCategory { get; }
        public string Target { get; }
        public string TargetSet { get; }
        public string Emphasis { get; }
        public string CommonCoreStandard { get; }

        public StandardIdentifier(
            string subjectCode,
            string claim,
            string target,
            string contentDomain = null,
            string contentCategory = null,
            string targetSet = null,
            string emphasis = null,
            string commonCoreStandard = null)
        {
            SubjectCode = subjectCode;
            Claim = claim;
            ContentDomain = contentDomain;
            ContentCategory = contentCategory;
            Target = target;
            TargetSet = targetSet;
            Emphasis = emphasis;
            CommonCoreStandard = commonCoreStandard;
        }

    }

    public class StandardIdentifierTargetComparer : IEqualityComparer<StandardIdentifier>
    {
        public static StandardIdentifierTargetComparer Instance { get; } = new StandardIdentifierTargetComparer();
        private StandardIdentifierTargetComparer() { }

        public bool Equals(StandardIdentifier x, StandardIdentifier y)
        {
            if(x.SubjectCode != y.SubjectCode)
            {
                return false;
            }

            if(x.SubjectCode == "MA") // TODO: Verify MA or MATH?
            {
                return x.Target == y.Target &&
                    x.Claim == y.Claim &&
                    x.ContentDomain == y.ContentDomain;
            }
            else if (x.SubjectCode == "ELA")
            {
                return x.Target == y.Target &&
                    x.Claim == y.Claim;
            }

            return false;
        }

        public int GetHashCode(StandardIdentifier obj)
        {
            return obj.GetHashCode();
        }
    }
    /// MATH:
    ///     CommonCoreStandard must match Claim, ContentDomain, Target, 
    ///         Emphasis, and CommonCoreStandard
    /// ELA:
    ///     Target must match Claim and Target
    ///     CommonCoreStandard must match Claim, Target, and CommonCoreStandard
    public class StandardIdentifierCcssComparer : IEqualityComparer<StandardIdentifier>
    {
        public static StandardIdentifierCcssComparer Instance { get; } = new StandardIdentifierCcssComparer();
        private StandardIdentifierCcssComparer() { }

        public bool Equals(StandardIdentifier x, StandardIdentifier y)
        {
            if (x.SubjectCode != y.SubjectCode)
            {
                return false;
            }

            if (x.SubjectCode == "MATH")
            {
                return x.Claim == y.Claim &&
                    x.ContentDomain == y.ContentDomain &&
                    x.Target == y.Target &&
                    x.Emphasis == y.Emphasis &&
                    x.CommonCoreStandard == y.CommonCoreStandard;
            }
            else if (x.SubjectCode == "ELA")
            {
                return x.Claim == y.Claim &&
                    x.Target == y.Target &&
                    x.CommonCoreStandard == y.CommonCoreStandard;
            }

            return false;
        }

        public int GetHashCode(StandardIdentifier obj)
        {
            return obj.GetHashCode();
        }
    }


}
