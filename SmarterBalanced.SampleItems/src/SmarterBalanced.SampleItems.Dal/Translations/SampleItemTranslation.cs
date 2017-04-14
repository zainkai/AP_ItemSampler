﻿using SmarterBalanced.SampleItems.Dal.Exceptions;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Text.RegularExpressions;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class SampleItemTranslation
    {
        /// <summary>
        /// Digests a collection of ItemMetadata objects and a collection of ItemContents objects into a collection of ItemDigest objects.
        /// Matches the ItemMetadata and ItemContents objects based on their ItemKey fields.
        /// </summary>
        public static ImmutableArray<SampleItem> ToSampleItems(
            ImmutableArray<ItemDigest> digests,
            IList<MergedAccessibilityFamily> resourceFamilies,
            IList<InteractionType> interactionTypes,
            IList<Subject> subjects,
            CoreStandardsXml standardsXml,
            IList<ItemPatch> patches,
            IList<BrailleFileInfo> brailleFileInfo,
            AppSettings settings
            )
        {
            var sampleItems = digests.Select(d =>
                ToSampleItem(
                    itemDigest: d,
                    standardsXml: standardsXml,
                    subjects: subjects,
                    interactionTypes: interactionTypes,
                    resourceFamilies: resourceFamilies,
                    patches: patches,
                    brailleFileInfo: brailleFileInfo,
                    settings: settings
                    ))
                .ToImmutableArray();

            return sampleItems;
        }

        /// <summary>
        /// Translates metadata, itemcontents and lookups to item digest
        /// </summary>
        public static SampleItem ToSampleItem(
            ItemDigest itemDigest,
            CoreStandardsXml standardsXml,
            IList<Subject> subjects,
            IList<InteractionType> interactionTypes,
            IList<MergedAccessibilityFamily> resourceFamilies,
            IList<ItemPatch> patches,
            IList<BrailleFileInfo> brailleFileInfo,
            AppSettings settings)
        {
            var supportedPubs = settings.SettingsConfig.SupportedPublications;
            var rubrics = GetRubrics(itemDigest, settings);
            StandardIdentifier identifier = StandardIdentifierTranslation.ToStandardIdentifier(itemDigest, supportedPubs);
            CoreStandards coreStandards = StandardIdentifierTranslation.CoreStandardFromIdentifier(standardsXml, identifier);

            var patch = patches.FirstOrDefault(p => p.ItemId == itemDigest.ItemKey);
            if (patch != null)
            {
                string claimNumber = Regex.Match(input: patch.Claim, pattern: @"\d+").Value;
                if (identifier == null)
                {
                    identifier = StandardIdentifier.Create(claim: claimNumber, target: patch.Target);
                }
                else
                {
                    identifier = identifier.WithClaimAndTarget(claimNumber, patch.Target);
                }

                coreStandards = StandardIdentifierTranslation.CoreStandardFromIdentifier(standardsXml, identifier);
                coreStandards = coreStandards.WithTargetCCSSDescriptions(patch.TargetDescription, patch.CCSSDescription);
            }

            var subject = subjects.FirstOrDefault(s => s.Code == itemDigest.SubjectCode);
            var interactionType = interactionTypes.FirstOrDefault(t => t.Code == itemDigest.InteractionTypeCode);
            var grade = GradeLevelsUtils.FromString(itemDigest.GradeCode);

            var claim = subject?.Claims.FirstOrDefault(t => t.ClaimNumber == identifier.ToClaimId());

            var family = resourceFamilies.FirstOrDefault(f =>
                f.Grades.Contains(grade) &&
                f.Subjects.Contains(itemDigest.SubjectCode));

            var fieldTestUseAttribute = itemDigest.ItemMetadataAttributes?.FirstOrDefault(a => a.Code == "itm_FTUse");
            var fieldTestUse = FieldTestUse.Create(fieldTestUseAttribute, itemDigest.SubjectCode);

            bool isPerformance = fieldTestUse != null && itemDigest.AssociatedPassage.HasValue;
            ImmutableArray<string> braillePassageCodes;
            ImmutableArray<string> brailleItemCodes = brailleFileInfo.Where
                (f => f.ItemKey == itemDigest.ItemKey)
                .Select(b => b.BrailleType).ToImmutableArray();

            if (itemDigest.AssociatedPassage.HasValue)
            {
                braillePassageCodes = brailleFileInfo
                    .Where(f => f.ItemKey == itemDigest.AssociatedPassage.Value)
                    .Select(b => b.BrailleType).ToImmutableArray();
            }
            else
            {
                braillePassageCodes = ImmutableArray.Create<string>();
            }

            bool aslSupported = AslSupported(itemDigest);
            var flaggedResources = family?.Resources
                .Select(r => r.ApplyFlags(
                    itemDigest,
                    interactionType?.Code, isPerformance, 
                    settings.SettingsConfig.DictionarySupportedItemTypes, 
                    brailleItemCodes,
                    claim,
                    aslSupported))
                .ToImmutableArray() ?? ImmutableArray<AccessibilityResource>.Empty;

            var groups = settings.SettingsConfig.AccessibilityTypes
                .Select(accType => GroupItemResources(accType, flaggedResources))
                .OrderBy(g => g.Order)
                .ToImmutableArray();

            string interactionTypeSubCat = "";
            settings.SettingsConfig.InteractionTypesToItem.TryGetValue(itemDigest.ToString(), out interactionTypeSubCat);


            SampleItem sampleItem = new SampleItem(
                itemType: itemDigest.ItemType,
                itemKey: itemDigest.ItemKey,
                bankKey: itemDigest.BankKey,
                targetAssessmentType: itemDigest.TargetAssessmentType,
                depthOfKnowledge: itemDigest.DepthOfKnowledge,
                sufficentEvidenceOfClaim: itemDigest.SufficentEvidenceOfClaim,
                associatedStimulus: itemDigest.AssociatedStimulus,
                aslSupported: aslSupported,
                allowCalculator: itemDigest.AllowCalculator,
                isPerformanceItem: isPerformance,
                accessibilityResourceGroups: groups,
                rubrics: rubrics,
                interactionType: interactionType,
                subject: subject,
                claim: claim,
                grade: grade,
                coreStandards: coreStandards,
                fieldTestUse: fieldTestUse,
                interactionTypeSubCat: interactionTypeSubCat,
                brailleItemCodes: brailleItemCodes,
                braillePassageCodes: braillePassageCodes);

            return sampleItem;
        }

        public static AccessibilityResourceGroup GroupItemResources(
            AccessibilityType accType,
            ImmutableArray<AccessibilityResource> resources)
        {
            var matchingResources = resources
                .Where(r => r.ResourceTypeId == accType.Id)
                .ToImmutableArray();

            var group = new AccessibilityResourceGroup(
                label: accType.Label,
                order: accType.Order,
                accessibilityResources: matchingResources);

            return group;
        }

        public static ImmutableArray<Rubric> GetRubrics(ItemDigest digest, AppSettings settings)
        {
            int? maxPoints = digest.MaximumNumberOfPoints;
            var rubrics = digest.Contents.Select(c => c.ToRubric(maxPoints, settings)).Where(r => r != null).ToImmutableArray();
            return rubrics;
        }

        /// <summary>
        /// Returns a Single Rubric from content and filters out any placeholder text
        /// </summary>
        public static Rubric ToRubric(
            this Content content,
            int? maxPoints,
            AppSettings appSettings)
        {
            if (appSettings == null || appSettings.RubricPlaceHolderText == null || appSettings.SettingsConfig == null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            var placeholder = appSettings.RubricPlaceHolderText;
            var languageToLabel = appSettings.SettingsConfig.LanguageToLabel;

            if (content == null ||
                content.RubricList == null ||
                content.RubricList.Rubrics == null ||
                content.RubricList.RubricSamples == null)
            {
                return null;
            }

            int scorePoint;
            var rubricEntries = content.RubricList.Rubrics
                .Where(r => !string.IsNullOrWhiteSpace(r.Value)
                    && int.TryParse(r.Scorepoint, out scorePoint)
                    && scorePoint <= maxPoints
                    && !placeholder.RubricPlaceHolderContains.Any(s => r.Value.Contains(s))
                    && !placeholder.RubricPlaceHolderEquals.Any(s => r.Value.Equals(s))).ToImmutableArray();

            Predicate<SampleResponse> pred = (r => string.IsNullOrWhiteSpace(r.SampleContent)
                                                     || placeholder.RubricPlaceHolderContains.Any(s => r.SampleContent.Contains(s))
                                                     || placeholder.RubricPlaceHolderEquals.Any(s => r.SampleContent.Equals(s)));

            content.RubricList.RubricSamples.ForEach(t => t.SampleResponses.RemoveAll(pred));

            var samples = content.RubricList.RubricSamples.Where(t => t.SampleResponses.Count() > 0).ToImmutableArray();
            if (rubricEntries.Length == 0 && samples.Length == 0)
            {
                return null;
            }

            string languangeLabel = (string.IsNullOrEmpty(content.Language)) ? string.Empty :
                                                languageToLabel[content.Language.ToUpper()];

            var rubric = new Rubric(languangeLabel, rubricEntries, samples);
            return rubric;
        }

        private static bool AslSupportedContents(List<Content> content)
        {
            if(content == null)
            {
                return false;
            }

            bool foundAslAttachment = content
             .Any(c => c.Attachments != null &&
                 c.Attachments.Any(a => !string.IsNullOrEmpty(a.Type) &&
                     a.Type.ToLower().Contains("asl")));

            return foundAslAttachment;
        }

        public static bool AslSupported(ItemDigest digest)
        {
            if (!digest.Contents.Any())
            {
                return digest.AslSupported ?? false;
            }

            bool foundAslAttachment = AslSupportedContents(digest.Contents);
            bool foundStimAslAttachment = AslSupportedContents(digest.StimulusDigest?.Contents);
            bool aslAttachment = foundAslAttachment || foundStimAslAttachment;

            bool aslSupported = (digest.AslSupported.HasValue) ? (digest.AslSupported.Value && aslAttachment) : aslAttachment;

            return aslSupported;
        }

    }
}
