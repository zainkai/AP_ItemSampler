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

        public SampleItem GetSampleItem(int bankKey, int itemKey)
        {
            return context.SampleItems.SingleOrDefault(item => item.BankKey == bankKey && item.ItemKey == itemKey);
        }

        public ItemCardViewModel GetItemCardViewModel(int bankKey, int itemKey)
        {
            return context.ItemCards.SingleOrDefault(item => item.BankKey == bankKey && item.ItemKey == itemKey);
        }

        /// <summary>
        /// Constructs an itemviewerservice URL to access the 
        /// item corresponding to the given SampleItem.
        /// </summary>
        public string GetItemViewerUrl(SampleItem item)
        {
            string items;
            string baseUrl = context.AppSettings.SettingsConfig.ItemViewerServiceURL;
            if (item == null)
            {
                return string.Empty;
            }

            if (item.IsPerformanceItem)
            {
                items = string.Join(",", GetStimulusUrl(item));
            }
            else
            {
                items = $"{item.BankKey}-{item.ItemKey}";
            }

                return $"{baseUrl}/items?ids={items}";
        }

        /// <summary>
        /// Gets a list of items that share a stimulus with the given item.
        /// Given item is returned as the first element of the list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<SampleItem> GetAssociatedStimulus(SampleItem item)
        {
            var associatedStimulus = item.AssociatedStimulus;
            List<SampleItem> associatedStimulusItems = context.SampleItems
            .Where(i => i.AssociatedStimulus == item.AssociatedStimulus)
            .OrderBy(i => i.ItemKey).ToList();

            int itemIndex = associatedStimulusItems
                .FindIndex(i => i.BankKey == item.BankKey && i.ItemKey == item.ItemKey);
            SampleItem tmpItem = associatedStimulusItems[0];
            associatedStimulusItems[0] = item;
            associatedStimulusItems[itemIndex] = tmpItem;

            return associatedStimulusItems;
        }

        private string[] GetStimulusUrl(SampleItem item)
        {
            var associatedStimulusDigests = GetAssociatedStimulus(item)?.Select(d => $"{d.BankKey}-{d.ItemKey}").ToArray();
            return associatedStimulusDigests;
        }


        public ItemViewModel GetItemViewModel(
            int bankKey,
            int itemKey,
            string[] iSAAPCodes,
            Dictionary<string, string> cookiePreferences)
        {
            var sampleItem = GetSampleItem(bankKey, itemKey);
            var itemCardViewModel = GetItemCardViewModel(bankKey, itemKey);
            if (sampleItem == null || itemCardViewModel == null)
            {
                return null;
            }

            var aboutThisItem = new AboutThisItemViewModel(
                rubrics: sampleItem.Rubrics,
                itemCard: itemCardViewModel,
                targetDescription: sampleItem.CoreStandards?.TargetDescription,
                depthOfKnowledge: sampleItem.DepthOfKnowledge,
                commonCoreStandardsDescription: sampleItem.CoreStandards?.CommonCoreStandardsDescription);

            var groups = sampleItem.AccessibilityResourceGroups.ApplyPreferences(iSAAPCodes, cookiePreferences);

            var itemViewModel = new ItemViewModel(
                itemViewerServiceUrl: GetItemViewerUrl(sampleItem),
                accessibilityCookieName: context.AppSettings.SettingsConfig.AccessibilityCookie,
                isPerformanceItem: sampleItem.IsPerformanceItem,
                accResourceGroups: groups,
                moreLikeThisVM: GetMoreLikeThis(sampleItem),
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
        public MoreLikeThisViewModel GetMoreLikeThis(SampleItem sampleItem)
        {
            var subjectCode = sampleItem.Subject.Code;
            var claimCode = sampleItem.Claim?.Code;
            var grade = sampleItem.Grade;
            var itemKey = sampleItem.ItemKey;
            var bankKey = sampleItem.BankKey;

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
