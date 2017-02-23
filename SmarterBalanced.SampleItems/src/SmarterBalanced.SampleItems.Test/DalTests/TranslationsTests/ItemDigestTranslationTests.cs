using SmarterBalanced.SampleItems.Dal.Exceptions;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Collections.Immutable;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;

namespace SmarterBalanced.SampleItems.Test.DalTests.TranslationsTests
{
    public class ItemDigestTranslationTests
    {
        //TODO: Add test for target
        /// <summary>
        /// Test translating a single ItemMetadata object and a single ItemContents object into an ItemDigest object
        /// </summary>
        [Fact(Skip ="TODO")]
        public void TestItemToItemDigest()
        {
            int testItemKey = 1;
            int testItemBank = 2;
            string testGrade = "5";

            ItemMetadata metadata = new ItemMetadata();
            ItemContents contents = new ItemContents();
            metadata.Metadata = new SmarterAppMetadataXmlRepresentation();
            contents.Item = new ItemXmlFieldRepresentation();
            metadata.Metadata.ItemKey = testItemKey;
            metadata.Metadata.GradeCode = testGrade;
            metadata.Metadata.TargetAssessmentType = "Test target string";
            metadata.Metadata.SufficientEvidenceOfClaim = "Test claim string";
            metadata.Metadata.InteractionType = "EQ";
            metadata.Metadata.SubjectCode = "MATH";
            metadata.Metadata.StandardPublications = new List<StandardPublication>();
            metadata.Metadata.StandardPublications.Add(
                new StandardPublication
                {
                    PrimaryStandard = "SBAC-ELA-v1:3-L|4-6|6.SL.2"
                });

            contents.Item.ItemKey = testItemKey;
            contents.Item.ItemBank = testItemBank;
            contents.Item.Contents = new List<Content>();

            var interactionTypes = new List<InteractionType>
            {
                new InteractionType(code: "EQ", label: "", description: "", order: 0)
            };

            var subjects = new List<Subject>
            {
                new Subject(
                    code: "MATH",
                    label: string.Empty,
                    shortLabel: string.Empty,
                    claims: ImmutableArray.Create<Claim>(),
                    interactionTypeCodes: ImmutableArray.Create<string>())
            };

            var placeholderText = new RubricPlaceHolderText
            {
                RubricPlaceHolderContains = new string[0],
                RubricPlaceHolderEquals = new string[0]
            };

            AppSettings appSettings = new AppSettings
                                {
                                    SettingsConfig = new SettingsConfig(),
                                    RubricPlaceHolderText = placeholderText
                                };
            

            ItemDigest digest = ItemDigestTranslation.ToItemDigest(metadata, contents, appSettings);
            Assert.Equal(testItemKey, digest.ItemKey);
            Assert.Equal(testItemBank, digest.BankKey);
            Assert.Equal(GradeLevels.Grade5, GradeLevelsUtils.FromString(digest.GradeCode));
            Assert.Equal("Test target string", digest.TargetAssessmentType);
            Assert.Equal("Test claim string", digest.SufficentEvidenceOfClaim);
            Assert.Equal("MATH", digest.SubjectCode);
            Assert.Equal("EQ", digest.InteractionTypeCode);
        }

        /// <summary>
        /// Tests that an exception is thrown if the ItemKey fields do not match for the 
        /// ItemMetadata and ItemContents objects passed to the ItemToItemDigest method.
        /// </summary>
        [Fact(Skip = "TODO")]
        public void TestItemToItemDigestInvalid()
        {
            ItemMetadata metadata = new ItemMetadata();
            ItemContents contents = new ItemContents();
            metadata.Metadata = new SmarterAppMetadataXmlRepresentation();
            contents.Item = new ItemXmlFieldRepresentation();
            metadata.Metadata.ItemKey = 1;
            metadata.Metadata.GradeCode = "7";
            metadata.Metadata.TargetAssessmentType = "Test target string";
            metadata.Metadata.SufficientEvidenceOfClaim = "Test claim string";
            metadata.Metadata.InteractionType = "EQ";
            metadata.Metadata.SubjectCode = "MATH";
            metadata.Metadata.StandardPublications = new List<StandardPublication>();
            metadata.Metadata.StandardPublications.Add(
                new StandardPublication
                {
                    PrimaryStandard = "SBAC-ELA-v1:3-L|4-6|6.SL.2"
                });

            contents.Item.ItemKey = 2;
            contents.Item.ItemBank = 3;
            contents.Item.Contents = new List<Content>();
            var exception = Assert.Throws(typeof(SampleItemsContextException), () => ItemDigestTranslation.ToItemDigest(metadata, contents, new AppSettings()));
        }


        /// <summary>
        /// Test translating a collection of ItemMetadata objects and a collection of ItenContents objects
        /// into a collection of ItemDigest objects.
        /// </summary>
        [Fact(Skip = "TODO")]
        public void TestItemstoItemDigests()
        {
            int testItemCount = 3;
            List<ItemContents> contentsList = new List<ItemContents>();
            List<ItemMetadata> metadataList = new List<ItemMetadata>();
            IEnumerable<ItemDigest> digests;

            // Get a range of numbers from 50 to the number of items being tested.
            // Use the same numer for an item's key and bank to make it easy to validate that
            // ItemMetadata and ItemContents objects are being paired correctly.
            int[] itemKeys = Enumerable.Range(50, testItemCount).ToArray();
            int[] banksKeys = itemKeys;
            string testTarget = "Test target string";
            string testClaimEvidence = "Test claim string";
            string testInteractionType = "EQ";
            string testSubject = "MATH";

            int i;
            for (i = 0; i < testItemCount; i++)
            {
                ItemMetadata metadata = new ItemMetadata();
                ItemContents contents = new ItemContents();

                metadata.Metadata = new SmarterAppMetadataXmlRepresentation();
                contents.Item = new ItemXmlFieldRepresentation();

                //Test metadata attributes
                metadata.Metadata.ItemKey = itemKeys[i];
                metadata.Metadata.GradeCode = (itemKeys[i] % 9 + 3).ToString();
                metadata.Metadata.TargetAssessmentType = testTarget + itemKeys[i];
                metadata.Metadata.SufficientEvidenceOfClaim = testClaimEvidence + itemKeys[i];
                metadata.Metadata.InteractionType = testInteractionType;
                metadata.Metadata.SubjectCode = testSubject;
                metadata.Metadata.StandardPublications = new List<StandardPublication>();
                metadata.Metadata.StandardPublications.Add(
                    new StandardPublication
                    {
                        PrimaryStandard = "SBAC-ELA-v1:3-L|4-6|6.SL.2"
                    });

                //Test contents attributes
                contents.Item.ItemKey = itemKeys[i];
                contents.Item.ItemBank = banksKeys[i];
                contents.Item.Contents = new List<Content>();

                metadataList.Add(metadata);
                contentsList.Add(contents);
            }

            var interactionTypes = new List<InteractionType>
            {
                new InteractionType(code: testInteractionType, label: "", description: "", order: 0)
            };

            var subjects = new List<Subject>
            {
                new Subject(
                    code: testSubject,
                    label: string.Empty,
                    shortLabel: string.Empty,
                    claims: ImmutableArray.Create<Claim>(),
                    interactionTypeCodes: ImmutableArray.Create<string>())
            };

            var settings = new AppSettings
            {
                SettingsConfig = new SettingsConfig
                {
                    AccessibilityTypes = new List<AccessibilityType>()
                }
            };

            digests = ItemDigestTranslation.ToItemDigests(metadataList, contentsList, settings);

            Assert.Equal(itemKeys.Length, digests.Count());

            foreach(var digest in digests)
            {
                int id = digest.ItemKey;
                Assert.Equal(digest.ItemKey, digest.BankKey);
                Assert.Equal(GradeLevelsUtils.FromString((digest.ItemKey % 9 + 3).ToString()), GradeLevelsUtils.FromString(digest.GradeCode));
                Assert.Equal(testTarget + id, digest.TargetAssessmentType);
                Assert.Equal(testClaimEvidence + id, digest.SufficentEvidenceOfClaim);
                Assert.Equal(testInteractionType, digest.InteractionTypeCode);
                Assert.Equal(testSubject, digest.SubjectCode);
            }
        }
    }
}
