using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.DalTests.TranslationsTests
{
    public class SampleItemTranslationTests
    {
        Claim Claim1, Claim2;
        Subject Math, Ela;
        InteractionType ItEla, ItMath;

        [Fact]
        public void TestToSampleItems()
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
            metadata.Metadata.InteractionType = "2";
            metadata.Metadata.SubjectCode = "MATH";
            metadata.Metadata.MaximumNumberOfPoints = 2;
            metadata.Metadata.StandardPublications = new List<StandardPublication>();
            metadata.Metadata.StandardPublications.Add(
                new StandardPublication
                {
                    PrimaryStandard = "SBAC-ELA-v1:3-L|4-6|6.SL.2",
                    Publication = "SupportedPubs"
                });

            contents.Item.ItemKey = testItemKey;
            contents.Item.ItemBank = testItemBank;
            contents.Item.Contents = new List<Content>();
            var placeholderText = new RubricPlaceHolderText
            {
                RubricPlaceHolderContains = new string[0],
                RubricPlaceHolderEquals = new string[0]
            };

            var accessibilityType = new AccessibilityType();
            accessibilityType.Id = "Acc1Type";
            accessibilityType.Label = "Accessibility 1";
            accessibilityType.Order = 1;

            var settings = new SettingsConfig
            {
                SupportedPublications = new string[] { "SupportedPubs" },
                AccessibilityTypes = new List<AccessibilityType>() { accessibilityType },
                InteractionTypesToItem = new Dictionary<string, string>(),
                DictionarySupportedItemTypes = new List<string>()
            };

            AppSettings appSettings = new AppSettings
            {
                SettingsConfig = settings,
                RubricPlaceHolderText = placeholderText
            };
            ItemDigest digest = ItemDigestTranslation.ToItemDigest(metadata, contents, appSettings);
            var digests = ImmutableArray.Create(digest);

            List<AccessibilityResource> Resources = new List<AccessibilityResource>
            {
                AccessibilityResource.Create(
                    resourceCode: "ACC1",
                    order: 1,
                    disabled: false,
                    defaultSelection: "ACC1_SEL1",
                    currentSelectionCode:  "ACC1_SEL1",
                    label: "Accessibility 1",
                    description: "Accessibility Selection One",
                    resourceType: "Acc1Type",
                    selections: ImmutableArray.Create(
                        new AccessibilitySelection(
                            code: "ACC1_SEL1",
                            order: 1,
                            disabled: false,
                            label: "Selection 1",
                            hidden: false))),
                AccessibilityResource.Create(
                    resourceCode: "ACC2",
                    order: 2,
                    disabled: false,
                    defaultSelection: "ACC2_SEL2",
                    currentSelectionCode:  "ACC2_SEL2",
                    label: "Accessibility 2",
                    description: "Accessibility Selection Two",
                    resourceType: "Acc2Type",
                    selections: ImmutableArray.Create(
                        new AccessibilitySelection(
                            code: "ACC2_SEL1",
                            order: 1,
                            disabled: false,
                            label: "Selection 1",
                            hidden: false),
                        new AccessibilitySelection(
                            code: "ACC2_SEL2",
                            order: 2,
                            disabled: false,
                            label: "Selection 2",
                            hidden: false)))
            };

            ImmutableArray<AccessibilityFamilyResource> noPartialResources = ImmutableArray.Create<AccessibilityFamilyResource>();
            var subjectMath = ImmutableArray.Create("MATH");
            AccessibilityFamily noPartialResourcesFamily = new AccessibilityFamily(
            subjects: subjectMath,
            grades: GradeLevels.Grade5,
            resources: noPartialResources);

            MergedAccessibilityFamily resourceFamily = AccessibilityResourceTranslation.MergeGlobalResources(noPartialResourcesFamily, Resources);
            var resourceFamilies = ImmutableArray.Create(resourceFamily);

            Claim1 = new Claim("claim1", "1", "ELA Claim 1");
            Claim2 = new Claim("claim2", "2", "3");
            ItMath = new InteractionType("2", "EQ", "", 2);
            ItEla = new InteractionType("1", "WER", "", 1);
            Math = Subject.Create("MATH", "Mathematics", "Math", ImmutableArray.Create(Claim2), ImmutableArray.Create(ItMath.Code));
            Ela = Subject.Create("ELA", "English", "ELA", ImmutableArray.Create(Claim1), ImmutableArray.Create(ItEla.Code));
            var interactionTypes = ImmutableArray.Create(ItEla, ItMath);
            var subjects = ImmutableArray.Create(Ela, Math);

            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringToStandardIdentifier("SBAC-ELA-v1:3-L|4-6|6.SL.2");
            var coreStandardsRowCcss = ImmutableArray.Create(
            CoreStandardsRow.Create(
                subjectCode: "ELA",
                key: "3-l|4-6|6.SL.2",
                name: "",
                description: "CCSS Desc",
                levelType: "CCSS",
                identifier: identifier));

            var coreStandardsRowTarget = ImmutableArray.Create(
               CoreStandardsRow.Create(
                   subjectCode: "ELA",
                   key: "4-6|3-6",
                   name: "",
                   description: "Target Desc",
                   levelType: "Target",
                   identifier: identifier));

            CoreStandardsXml coreStandardsXml = new CoreStandardsXml(coreStandardsRowTarget, coreStandardsRowCcss);
            var itemPatch = ImmutableArray.Create(new ItemPatch());
            var items = SampleItemTranslation.ToSampleItems(digests, resourceFamilies, interactionTypes, subjects, coreStandardsXml, itemPatch, appSettings);

            Assert.NotNull(items);
            Assert.Equal(items[0].AccessibilityResourceGroups[0].AccessibilityResources[0].Label, Resources[0].Label);
            Assert.Equal(items[0].AccessibilityResourceGroups[0].AccessibilityResources[0].Order, Resources[0].Order);
            Assert.Equal(items[0].AccessibilityResourceGroups[0].AccessibilityResources[0].ResourceCode, Resources[0].ResourceCode);
            Assert.Equal(items[0].AccessibilityResourceGroups[0].AccessibilityResources[0].ResourceTypeId, Resources[0].ResourceTypeId);
            Assert.Equal(items[0].SufficentEvidenceOfClaim, metadata.Metadata.SufficientEvidenceOfClaim);
            Assert.Equal(items[0].TargetAssessmentType, metadata.Metadata.TargetAssessmentType);
            Assert.Equal(items[0].Grade, GradeLevelsUtils.FromString(digest.GradeCode));
            Assert.Equal(items[0].ItemKey, digest.ItemKey);
            Assert.Equal(items[0].BankKey, digest.BankKey);
            Assert.Equal(items[0].CoreStandards.CommonCoreStandardsDescription, coreStandardsRowCcss[0].Description);
            Assert.Equal(items[0].CoreStandards.TargetDescription, coreStandardsRowTarget[0].Description);
            Assert.Equal(items[0].InteractionType.Code, ItMath.Code);
            Assert.Equal(items[0].InteractionType.Label, ItMath.Label);
            Assert.Equal(items[0].Subject.Code, digest.SubjectCode);
        }
    }
}
