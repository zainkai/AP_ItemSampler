using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public class StandardIdentifierTranslation
    {
        /* 
         * Locate and parse the standard, claim, and target from the metadata
         * 
         * Claim and target are specified in one of the following formats:
         * SBAC-ELA-v1 (there is only one alignment for ELA, this is used for delivery)
         *     Claim|Assessment Target|Common Core Standard
         * SBAC-MA-v6 (Math, based on the blueprint hierarchy, primary alignment and does not go to standard level, THIS IS USED FOR DELIVERY, should be the same as SBAC-MA-v4)
         *     Claim|Content Category|Target Set|Assessment Target
         * SBAC-MA-v5 (Math, based on the content specifications hierarchy secondary alignment to the standard level)
         *     Claim|Content Domain|Target|Emphasis|Common Core Standard
         * SBAC-MA-v4 (Math, based on the content specifications hierarchy primary alignment to the standard level)
         *     Claim|Content Domain|Target|Emphasis|Common Core Standard
         */

        public static StandardIdentifier StandardStringtoStandardIdentifier(string standards)
        {
            if (string.IsNullOrEmpty(standards))
            {
                // TODO: log error
                return null;
            }

            string[] publicationAndKey = standards.Split(':');

            if (publicationAndKey.Length < 2)
            {
                return null;

                // TODO: log error instead
                //throw new ArgumentException("The standards string does not contain a publication and claim spearated by a colon.");
            }

            StandardIdentifier standardIdentifier = null;

            string publication = publicationAndKey[0];
            string[] parts = publicationAndKey[1].Split('|');

            switch (publication)
            {
                case "SBAC-ELA-v1":
                    standardIdentifier = ElaV1Standard(parts);
                    break;

                case "SBAC-MA-v1":
                    standardIdentifier = MaV4Standard(parts);
                    break;

                case "SBAC-MA-v4":
                    standardIdentifier = MaV4Standard(parts);
                    break;

                case "SBAC-MA-v5":
                    standardIdentifier = MaV5Standard(parts);
                    break;

                case "SBAC-MA-v6":
                    standardIdentifier = MaV6Standard(parts);
                    break;
            }

            if(standardIdentifier == null)
            {
                standardIdentifier = StandardIdentifier.Create(
                        claim: parts[0]);
            }

            return standardIdentifier;

        }

        public static StandardIdentifier ElaV1Standard(string[] parts)
        {
            if (parts == null || parts.Length != 3)
            {
                return null;
            }

            return StandardIdentifier.Create(
                    claim: parts[0],
                    target: parts[1],
                    commonCoreStandard: parts[2],
                    subjectCode: "ELA");
        }

        public static StandardIdentifier MaV1Standard(string[] parts)
        {
            if (parts == null || parts.Length != 5)
            {
                return null;
            }

            return StandardIdentifier.Create(
                    claim: parts[0],
                    target: parts[2],
                    commonCoreStandard: parts[4],
                    contentDomain: parts[1],
                    subjectCode: "MATH");
        }

        public static StandardIdentifier MaV4Standard(string[] parts)
        {
            if (parts == null || parts.Length != 5)
            {
                return null;
            }

            return StandardIdentifier.Create(
                    claim: parts[0],
                    target: parts[2],
                    contentDomain: parts[1],
                    emphasis: parts[3],
                    commonCoreStandard: parts[4],
                    subjectCode: "MATH");
        }

        public static StandardIdentifier MaV5Standard(string[] parts)
        {
            if (parts == null || parts.Length != 5)
            {
                return null;
            }

            return StandardIdentifier.Create(
                    claim: parts[0],
                    target: parts[2],
                    contentDomain: parts[1],
                    emphasis: parts[3],
                    commonCoreStandard: parts[4],
                    subjectCode: "MATH");
        }

        public static StandardIdentifier MaV6Standard(string[] parts)
        {
            if (parts == null || parts.Length != 4)
            {
                return null;
            }

            return StandardIdentifier.Create(
                    claim: parts[0],
                    target: parts[3],
                    contentCategory: parts[1],
                    targetSet: parts[2],
                    subjectCode: "MATH");
        }

        public static StandardIdentifier ElaCoreStandardToTarget(string[] parts)
        {
            if (parts == null || parts.Length != 2)
            {
                return null;
            }

            return StandardIdentifier.Create(
                    claim: parts[0],
                    target: parts[1],
                    subjectCode: "ELA");
        }

        public static StandardIdentifier ElaCoreStandardToCcss(string[] parts)
        {
            if (parts == null || parts.Length != 3)
            {
                return null;
            }

            return StandardIdentifier.Create(
                    claim: parts[0],
                    target: parts[1],
                    commonCoreStandard: parts[2],
                    subjectCode: "ELA");
        }

        public static StandardIdentifier MathCoreStandardToTarget(string[] parts)
        {
            if (parts == null || parts.Length != 3)
            {
                return null;
            }

            return StandardIdentifier.Create(
                    claim: parts[0],
                    target: parts[2],
                    contentDomain: parts[1],
                    subjectCode: "MATH");
        }

        public static StandardIdentifier MathCoreStandardToCcss(string[] parts)
        {
            if (parts == null || parts.Length != 5)
            {
                return null;
            }

            return StandardIdentifier.Create(
                    claim: parts[0],
                    target: parts[2],
                    commonCoreStandard: parts[4],
                    contentDomain: parts[1],
                    subjectCode: "MATH");
        }

        public static StandardIdentifier CoreStandardToIdentifier(CoreStandardsRow row)
        {
            if (row == null || string.IsNullOrEmpty(row.Key))
            {
                throw new ArgumentException("The standards string must not be null or empty");
            }

            string[] parts = row.Key.Split('|');

            StandardIdentifier identifier = null;

            if (row.SubjectCode == "ELA")
            {
                if(row.LevelType == "CCSS")
                {
                    identifier = ElaCoreStandardToCcss(parts);
                }
                else if(row.LevelType == "Target")
                {
                    identifier = ElaCoreStandardToTarget(parts);
                }
            }
            else if (row.SubjectCode == "MATH")
            {
                if (row.LevelType == "CCSS")
                {
                    identifier = MathCoreStandardToCcss(parts);
                }
                else if (row.LevelType == "Target")
                {
                    identifier = MathCoreStandardToTarget(parts);
                }
            }

            return identifier;
        }
    }

}
