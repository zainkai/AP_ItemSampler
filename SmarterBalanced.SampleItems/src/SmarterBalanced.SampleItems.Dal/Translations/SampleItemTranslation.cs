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
                {
                    try
                    {
                        return ToSampleItem(
                           itemDigest: d,
                           standardsXml: standardsXml,
                           subjects: subjects,
                           interactionTypes: interactionTypes,
                           resourceFamilies: resourceFamilies,
                           patches: patches,
                           brailleFileInfo: brailleFileInfo,
                           settings: settings
                           );

                    } catch (Exception e)
                    {
                        throw new Exception($"Item {d.BankKey}-{d.ItemKey}", innerException: e);
                    }
                }).ToImmutableArray();

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
            var supportedPubs = settings.SbContent.SupportedPublications;
            var rubrics = GetRubrics(itemDigest, settings);       
            var brailleItemCodes = GetBrailleItemCodes(itemDigest.ItemKey, brailleFileInfo);
            var braillePassageCodes = GetBraillePassageCodes(itemDigest, brailleFileInfo);
            var interactionType = interactionTypes.FirstOrDefault(t => t.Code == itemDigest.InteractionTypeCode);
            var grade = GradeLevelsUtils.FromString(itemDigest.GradeCode);
            var patch = patches.FirstOrDefault(p => p.ItemId == itemDigest.ItemKey);
            var copiedItemPatch = patches.FirstOrDefault(p => p.BrailleCopiedId == itemDigest.ItemKey.ToString());
            var subject = subjects.FirstOrDefault(s => s.Code == itemDigest.SubjectCode);
            var depthOfKnowledge = itemDigest.DepthOfKnowledge;
            var itemType = itemDigest.ItemType;

            var fieldTestUseAttribute = itemDigest.ItemMetadataAttributes?.FirstOrDefault(a => a.Code == "itm_FTUse");
            var fieldTestUse = FieldTestUse.Create(fieldTestUseAttribute, itemDigest.SubjectCode);

            StandardIdentifier identifier = StandardIdentifierTranslation.ToStandardIdentifier(itemDigest, supportedPubs);
            CoreStandards coreStandards = StandardIdentifierTranslation.CoreStandardFromIdentifier(standardsXml, identifier);
            int? copiedFromItem = null;

            if (patch != null)
            {
                int tmp;
                copiedFromItem = int.TryParse(patch.BrailleCopiedId, out tmp) ? (int?)tmp : null;
                depthOfKnowledge = !string.IsNullOrEmpty(patch.DepthOfKnowledge) ? patch.DepthOfKnowledge : depthOfKnowledge;
                itemType = !string.IsNullOrEmpty(patch.ItemType) ? patch.ItemType : itemType;
                coreStandards = ApplyPatchToCoreStandards(identifier, coreStandards, standardsXml, patch);
            }

            if(copiedItemPatch != null)
            {
                var copyBrailleItemCodes = GetBrailleItemCodes(copiedItemPatch.ItemId, brailleFileInfo);
                brailleItemCodes = brailleItemCodes.Concat(copyBrailleItemCodes).Distinct().ToImmutableArray();
            }

            if (patch != null && !string.IsNullOrEmpty(patch.QuestionNumber))
            {
                fieldTestUse = ApplyPatchFieldTestUse(fieldTestUse, patch);
            }

            var claim = subject?.Claims.FirstOrDefault(t => t.ClaimNumber == coreStandards.ClaimId);
            bool brailleOnly = copiedFromItem.HasValue;
            bool isPerformance = fieldTestUse != null && itemDigest.AssociatedPassage.HasValue;

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

            var groups = GetAccessibilityResourceGroups(itemDigest, resourceFamilies, grade, 
                isPerformance, aslSupported, claim, interactionType, brailleItemCodes, settings);

            string interactionTypeSubCat = "";
            settings.SbContent.InteractionTypesToItem.TryGetValue(itemDigest.ToString(), out interactionTypeSubCat);

            SampleItem sampleItem = new SampleItem(
                itemType: itemType,
                itemKey: itemDigest.ItemKey,
                bankKey: itemDigest.BankKey,
                targetAssessmentType: itemDigest.TargetAssessmentType,
                depthOfKnowledge: depthOfKnowledge,
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
                braillePassageCodes: braillePassageCodes,
                brailleOnlyItem: brailleOnly,
                copiedFromitem: copiedFromItem,
                educationalDifficulty: itemDigest.EducationalDifficulty,
                evidenceStatement: itemDigest.EvidenceStatement,
                domain: identifier?.ContentDomain);

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

        public static ImmutableArray<string> GetBrailleItemCodes(int itemKey, IList<BrailleFileInfo> brailleFileInfo)
        {
            ImmutableArray<string> brailleItemCodes = brailleFileInfo.Where
                (f => f.ItemKey == itemKey)
                .Select(b => b.BrailleType).ToImmutableArray();

            return brailleItemCodes;
        }

        public static ImmutableArray<string> GetBraillePassageCodes(ItemDigest itemDigest, IList<BrailleFileInfo> brailleFileInfo)
        {
            ImmutableArray<string> braillePassageCodes;

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

            return braillePassageCodes;
        }

        public static ImmutableArray<AccessibilityResourceGroup> GetAccessibilityResourceGroups(
            ItemDigest itemDigest,
            IList<MergedAccessibilityFamily> resourceFamilies,
            GradeLevels grade,
            bool isPerformance,
            bool aslSupported,
            Claim claim,
            InteractionType interactionType,
            ImmutableArray<string> brailleItemCodes,
            AppSettings settings)
        {
            var family = resourceFamilies.FirstOrDefault(f =>
               f.Grades.Contains(grade) &&
               f.Subjects.Contains(itemDigest.SubjectCode));

            var flaggedResources = family?.Resources
             .Select(r => r.ApplyFlags(
                 itemDigest,
                 interactionType?.Code, isPerformance,
                 settings.SbContent.DictionarySupportedItemTypes,
                 brailleItemCodes,
                 claim,
                 aslSupported))
             .ToImmutableArray() ?? ImmutableArray<AccessibilityResource>.Empty;

            var groups = settings.SbContent.AccessibilityTypes
                .Select(accType => GroupItemResources(accType, flaggedResources))
                .OrderBy(g => g.Order)
                .ToImmutableArray();

            return groups;
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
            if (appSettings == null || appSettings.SbContent.RubricPlaceHolderText == null || appSettings.SbContent == null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            var placeholder = appSettings.SbContent.RubricPlaceHolderText;
            var languageToLabel = appSettings.SbContent.LanguageToLabel;

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

        private static CoreStandards ApplyPatchToCoreStandards(StandardIdentifier identifier, 
            CoreStandards coreStandards, 
            CoreStandardsXml standardsXml, 
            ItemPatch patch)
        {
            string claimNumber = Regex.Match(input: patch.Claim, pattern: @"\d+").Value;
            if (identifier == null)
            {
                identifier = StandardIdentifier.Create(claim: claimNumber, target: patch.Target);
            }
            else
            {
                string target = (!string.IsNullOrEmpty(patch.Target)) ? patch.Target : identifier.Target;
                claimNumber = (!string.IsNullOrEmpty(claimNumber)) ? claimNumber : identifier.Claim;
                identifier = identifier.WithClaimAndTarget(claimNumber, target);
            }

            string targetDesc = (!string.IsNullOrEmpty(patch.TargetDescription)) ? patch.TargetDescription : coreStandards?.Target.Descripton;
            string ccssDesc = (!string.IsNullOrEmpty(patch.CCSSDescription)) ? patch.CCSSDescription : coreStandards?.CommonCoreStandardsDescription;
            coreStandards = StandardIdentifierTranslation.CoreStandardFromIdentifier(standardsXml, identifier);
            coreStandards = coreStandards.WithTargetCCSSDescriptions(targetDesc, ccssDesc);

            return coreStandards;

        }

        private static FieldTestUse ApplyPatchFieldTestUse(FieldTestUse fieldTestUse, ItemPatch patch)
        {
            int patchQuestion;
            int.TryParse(patch.QuestionNumber, out patchQuestion);

            var newFieldTestUse = new FieldTestUse
            {
                Code = fieldTestUse?.Code,
                CodeYear = fieldTestUse?.CodeYear,
                QuestionNumber = patchQuestion,
                Section = fieldTestUse?.Section
            };

            return newFieldTestUse;
        }

        private static int? GetCopiedFromItem(string desc)
        {
            int val;
            if (string.IsNullOrEmpty(desc))
            {
                return null;
            }

            var match = Regex.Match(input: desc, pattern: @"^(?=.*\bCloned\b)(?=.*\b(\d{4,6})).*$");

            return match.Success && int.TryParse(match.Groups[1]?.Value, out val) ? (int?)val : null;
        }
    }
}
