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

        /// <summary>
        /// MATH must match:
        ///      Claim, Content Domain, and Target
        /// ELA must match:
        ///     Claim and Target
        /// </summary>
        public bool Equals(StandardIdentifier x, StandardIdentifier y)
        {
            if(x.SubjectCode != y.SubjectCode)
            {
                return false;
            }

            if(x.SubjectCode == "MA") // TODO: Verify MA or MATH?
            {
                return x.Claim == y.Claim &&
                    x.ContentDomain == y.ContentDomain &&
                    x.Target == y.Target;
            }
            else if (x.SubjectCode == "ELA")
            {
                return x.Claim == y.Claim &&
                    x.Target == y.Target;
            }

            return false;
        }

        public int GetHashCode(StandardIdentifier obj)
        {
            return obj.GetHashCode();
        }
    }
 
    public class StandardIdentifierCcssComparer : IEqualityComparer<StandardIdentifier>
    {
        public static StandardIdentifierCcssComparer Instance { get; } = new StandardIdentifierCcssComparer();
        private StandardIdentifierCcssComparer() { }

        /// <summary>
        /// MATH must match:
        ///      Claim, Content Domain, Emphasis, CCSS and Target
        /// ELA must match:
        ///     Claim, CCSS, and Target
        /// </summary>
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
                    x.Emphasis == y.Emphasis &&
                    x.Target == y.Target &&
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
