using SmarterBalanced.SampleItems.Dal.Exceptions;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;

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
            AppSettings settings)
        {
            var sampleItems = digests.Select(d =>
                ToSampleItem(
                    itemDigest: d,
                    standardsXml: standardsXml,
                    subjects: subjects,
                    interactionTypes: interactionTypes,
                    resourceFamilies: resourceFamilies,
                    settings: settings))
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
            AppSettings settings)
        {
            var supportedPubs = settings.SettingsConfig.SupportedPublications;
            var rubrics = GetRubrics(itemDigest, settings);
            StandardIdentifier identifier = StandardIdentifierTranslation.ToStandardIdentifier(itemDigest, supportedPubs);
            var coreStandards = StandardIdentifierTranslation.CoreStandardFromIdentififer(standardsXml, identifier);
            var subject = subjects.FirstOrDefault(s => s.Code == itemDigest.SubjectCode);
            var interactionType = interactionTypes.FirstOrDefault(t => t.Code == itemDigest.InteractionTypeCode);
            var grade = GradeLevelsUtils.FromString(itemDigest.GradeCode);
            var claim = subject?.Claims.FirstOrDefault(t => t.ClaimNumber == identifier.ToClaimId());

            SampleItem sampleItem = SampleItem.Create
            (
                itemType: itemDigest.ItemType,
                itemKey: itemDigest.ItemKey,
                bankKey: itemDigest.BankKey,
                targetAssessmentType: itemDigest.TargetAssessmentType,
                depthOfKnowledge: itemDigest.DepthOfKnowledge,
                sufficentEvidenceOfClaim: itemDigest.SufficentEvidenceOfClaim,
                associatedStimulus: itemDigest.AssociatedStimulus,
                aslSupported: itemDigest.AslSupported,
                allowCalculator: itemDigest.AllowCalculator,
                isPerformanceItem: itemDigest.AssociatedPassage.HasValue,
                accessibilityResourceGroups: ImmutableArray.Create<AccessibilityResourceGroup>(),
                rubrics: rubrics,
                interactionType: interactionType,
                subject: subject,
                claim: claim,
                grade: grade,
                coreStandards: coreStandards
            );

            //todo: refactor
            sampleItem.AccessibilityResourceGroups = AccessibilityResourceTranslation.CreateAccessibilityGroups(
                sampleItem,
                resourceFamilies, 
                settings.SettingsConfig.AccessibilityTypes);

            return sampleItem;
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

  
    }
}
