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

            string[] parts = standards.Split('|');

            //The part of the string before the first pipe should always contain the publication and claim
            string[] publicationAndClaim = parts[0].Split(':');

            if(publicationAndClaim.Length < 2)
            {
                throw new ArgumentException("The standards string does not contain a publication and claim spearated by a colon.");
            }
            publication = publicationAndClaim[0];
            claim = publicationAndClaim[1];

            if (string.IsNullOrEmpty(claim))
            {
                throw new ArgumentException("The standards string does not contain a claim.");
            }

            //Depending on the standard there are different fields in different orders
            //separated by pipes as listed above
            switch (publication)
            {
                case "SBAC-ELA-v1":
                    //The ELA string should be split into 3 parts
                    if (parts.Length == 3)
                    {
                        var target = parts[1];
                        var commonCoreStandard = parts[2];
                        return new StandardIdentifier(
                            claim,
                            target,
                            commonCoreStandard: commonCoreStandard
                            );
                    }
                    break;
                case "SBAC-MA-v1":
                    if(parts.Length == 5)
                    {
                        var target = parts[2];
                        var contentDomain = parts[1];
                        var commonCoreStandard = parts[4];
                        return new StandardIdentifier(
                            claim,
                            target,
                            commonCoreStandard: commonCoreStandard,
                            contentDomain: contentDomain
                            );
                    }
                    break;
                case "SBAC-MA-v4":
                    //The math v4 string should be split into 5 parts
                    if (parts.Length == 5)
                    {
                        var target = parts[2];
                        var standard = parts[4];
                        var contentDomain = parts[1];
                        var emphasis = parts[3];
                        return new StandardIdentifier(
                            claim,
                            target,
                            contentDomain,
                            emphasis: emphasis,
                            commonCoreStandard: standard
                            );
                    }
                    break;
                case "SBAC-MA-v5":
                    //The math v5 string should be split into 5 parts
                    if (parts.Length == 5)
                    {
                        var target = parts[2];
                        var commonCoreStandard = parts[4];
                        var contentDomain = parts[1];
                        var emphasis = parts[3];
                        return new StandardIdentifier(
                            claim,
                            target,
                            contentDomain: contentDomain,
                            emphasis: emphasis,
                            commonCoreStandard: commonCoreStandard
                            );
                    }
                    break;
                case "SBAC-MA-v6":
                    //The math v6 string should be split into 4 parts
                    if (parts.Length == 4)
                    {
                        var target = parts[3];
                        var category = parts[1];
                        var targetSet = parts[2];
                        return new StandardIdentifier(
                            claim,
                            target,
                            contentCategory: category,
                            targetSet: targetSet
                            );
                    }
                    break;
                default:
                    break;
            }
            return new StandardIdentifier(claim, null);
        }
        public static StandardIdentifier StandardKeyToIdentifier(string standardKey)
        {
            if (string.IsNullOrEmpty(standardKey))
            {
                throw new ArgumentException("The standards string must not be null or empty");
            }

            string[] parts = standardKey.Split('|');

            if (parts.Length != 3)
            {
                throw new ArgumentException("Standard does not have two parts |.");
            }

            return new StandardIdentifier(
                               claim: parts[0],
                               target: parts[1],
                               commonCoreStandard: parts[2]
                               );

        }
    }

}
