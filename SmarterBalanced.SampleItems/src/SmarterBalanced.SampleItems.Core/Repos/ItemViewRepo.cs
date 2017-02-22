using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SmarterBalanced.SampleItems.Core.Translations;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public class ItemViewRepo : IItemViewRepo
    {
        private readonly SampleItemsContext context;
        private readonly ILogger logger;

        public ItemViewRepo(SampleItemsContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            logger = loggerFactory.CreateLogger<ItemViewRepo>();
        }

        public ItemDigest GetItemDigest(int bankKey, int itemKey)
        {
            return context.ItemDigests.SingleOrDefault(item => item.BankKey == bankKey && item.ItemKey == itemKey);
        }

        public ItemCardViewModel GetItemCardViewModel(int bankKey, int itemKey)
        {
            return context.ItemCards.SingleOrDefault(item => item.BankKey == bankKey && item.ItemKey == itemKey);
        }

        /// <summary>
        /// Constructs an itemviewerservice URL to access the 
        /// item corresponding to the given ItemDigest.
        /// </summary>
        protected string GetItemViewerUrl(ItemDigest digest)
        {
            string items;
            string baseUrl = context.AppSettings.SettingsConfig.ItemViewerServiceURL;
            if (digest == null)
            {
                return string.Empty;
            }

            if (digest.IsPerformanceItem)
            {
                items = string.Join(",", GetStimulusUrl(digest));
            }
            else
            {
                items = $"{digest.BankKey}-{digest.ItemKey}";
            }
   
                return $"{baseUrl}/item?=items{items}";
        }

        private List<ItemDigest> GetAssociatedStimulus(ItemDigest digest)
        {
            var associatedStimulus = digest.AssociatedStimulus;
            List<ItemDigest> associatedStimulusDigests = context.ItemDigests
            .Where(i => i.AssociatedStimulus == digest.AssociatedStimulus)
            .OrderBy(i => i.ItemKey).ToList();

            return associatedStimulusDigests;
        }

        private string[] GetStimulusUrl(ItemDigest digest)
        {
            var associatedStimulusDigests = GetAssociatedStimulus(digest)?.Select(d => $"{d.BankKey}-{d.ItemKey}").ToArray();
            return associatedStimulusDigests;
        }


        public ItemViewModel GetItemViewModel(
            int bankKey,
            int itemKey,
            string[] iSAAPCodes,
            Dictionary<string, string> cookiePreferences)
        {
            var itemDigest = GetItemDigest(bankKey, itemKey);
            var itemCardViewModel = GetItemCardViewModel(bankKey, itemKey);
            if (itemDigest == null || itemCardViewModel == null)
            {
                return null;
            }

            var aboutThisItem = new AboutThisItemViewModel(
                rubrics: itemDigest.Rubrics,
                itemCard: itemCardViewModel,
                targetDescription: itemDigest.CoreStandards?.TargetDescription,
                depthOfKnowledge: itemDigest.DepthOfKnowledge,
                commonCoreStandardsDescription: itemDigest.CoreStandards?.CommonCoreStandardsDescription);

            var groups = itemDigest.AccessibilityResourceGroups.ApplyPreferences(iSAAPCodes, cookiePreferences);

            var itemViewModel = new ItemViewModel(
                itemViewerServiceUrl: GetItemViewerUrl(itemDigest),
                accessibilityCookieName: context.AppSettings.SettingsConfig.AccessibilityCookie,

                accResourceGroups: groups,
                moreLikeThisVM: GetMoreLikeThis(itemDigest),
                aboutThisItemVM: aboutThisItem);

            return itemViewModel;
        }

        private MoreLikeThisColumn ToColumn(IEnumerable<ItemCardViewModel> itemCards, GradeLevels grade)
        {
            string label = grade.ToDisplayString();
            var column = new MoreLikeThisColumn(
                label: label, itemCards: itemCards.ToImmutableArray());

            return column;
        }

        /// <summary>
        /// Gets up to 3 items same grade, grade above, and grade below. All items 
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="subject"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public MoreLikeThisViewModel GetMoreLikeThis(ItemDigest itemDigest)
        {
            var subjectCode = itemDigest.Subject.Code;
            var claimCode = itemDigest.Claim?.Code;
            var grade = itemDigest.Grade;
            var itemKey = itemDigest.ItemKey;
            var bankKey = itemDigest.BankKey;

            var matchingSubjectClaim = context.ItemCards.Where(i => i.SubjectCode == subjectCode && i.ClaimCode == claimCode);
            int numExpected = context.AppSettings.SettingsConfig.NumMoreLikeThisItems;

            var comparer = new MoreLikeThisComparer(subjectCode, claimCode);
            GradeLevels gradeBelow = grade.GradeBelow();
            GradeLevels gradeAbove = grade.GradeAbove();

            var cardsGradeBelow = context.ItemCards
                .Where(i => i.Grade == gradeBelow)
                .OrderBy(i => i, comparer)
                .Take(numExpected);

            var cardsSameGrade = context.ItemCards
                .Where(i => i.Grade == grade && i.ItemKey != itemKey)
                .OrderBy(i => i, comparer)
                .Take(numExpected);

            var cardsGradeAbove = context.ItemCards
                .Where(i => i.Grade == gradeAbove)
                .OrderBy(i => i, comparer)
                .Take(numExpected);

            var moreLikeThisVM = new MoreLikeThisViewModel(
                ToColumn(cardsGradeBelow, gradeBelow),
                ToColumn(cardsSameGrade, grade),
                ToColumn(cardsGradeAbove, gradeAbove)
                );

            return moreLikeThisVM;
        }

    }

}
