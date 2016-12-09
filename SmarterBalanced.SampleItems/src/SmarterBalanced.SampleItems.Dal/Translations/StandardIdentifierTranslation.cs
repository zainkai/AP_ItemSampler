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
                throw new ArgumentException("The standards string must not be null or empty");
            }

            string claim, publication;
            StandardIdentifier standardIdentifier = new StandardIdentifier();

            string[] parts = standards.Split('|');

            //The part of the string before the first pipe should always contain the publication and claim
            string[] publicationAndClaim = parts[0].Split(':');

            if(publicationAndClaim.Length < 2)
            {
                throw new ArgumentException("The standards string does not contain a publication and claim spearated by a colon.");
            }
            publication = publicationAndClaim[0];
            claim = publicationAndClaim[1];

            if (!string.IsNullOrEmpty(claim))
            {
                standardIdentifier.Claim = claim;
            }

            //Depending on the standard there are different fields in different orders
            //separated by pipes as listed above
            switch (publication)
            {
                case "SBAC-ELA-v1":
                    //The ELA string should be split into 3 parts
                    if (parts.Length == 3)
                    {
                        standardIdentifier.Target = parts[1];
                        standardIdentifier.CommonCoreStandard = parts[2];
                    }
                    break;
                case "SBAC-MA-v4":
                    //The math v4 string should be split into 5 parts
                    if (parts.Length == 5)
                    {
                        standardIdentifier.Target = parts[2];
                        standardIdentifier.CommonCoreStandard = parts[4];
                        standardIdentifier.ContentDomain = parts[1];
                        standardIdentifier.Emphasis = parts[3];
                    }
                    break;
                case "SBAC-MA-v5":
                    //The math v5 string should be split into 5 parts
                    if (parts.Length == 5)
                    {
                        standardIdentifier.Target = parts[2];
                        standardIdentifier.CommonCoreStandard = parts[4];
                        standardIdentifier.ContentDomain = parts[1];
                        standardIdentifier.Emphasis = parts[3];
                    }
                    break;
                case "SBAC-MA-v6":
                    //The math v6 string should be split into 4 parts
                    if (parts.Length == 4)
                    {
                        standardIdentifier.Target = parts[3];
                        standardIdentifier.ContentCategory = parts[1];
                        standardIdentifier.TargetSet = parts[2];
                    }
                    break;
                default:
                    break;
            }
            return standardIdentifier;
        }
    }
}
