using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{

    public class AboutThisItemViewModel
    {
        public ItemCardViewModel ItemCardViewModel { get; }
        public ImmutableArray<Rubric> Rubrics { get; }
        public string TargetDescription { get; }
        public string DepthOfKnowledge { get; }
        public string CommonCoreStandardsDescription { get; }
        public string EducationalDifficulty { get; }
        public string EvidenceStatement { get; }

        public AboutThisItemViewModel(
            ImmutableArray<Rubric> rubrics,
            ItemCardViewModel itemCard,
            string targetDescription,
            string depthOfKnowledge,
            string commonCoreStandardsDescription,
            string educationalDifficulty,
            string evidenceStatement)
        {
            ItemCardViewModel = itemCard;
            Rubrics = rubrics;
            TargetDescription = targetDescription;
            DepthOfKnowledge = depthOfKnowledge;
            CommonCoreStandardsDescription = commonCoreStandardsDescription;
            EducationalDifficulty = educationalDifficulty;
            EvidenceStatement = evidenceStatement;
        }

        public static AboutThisItemViewModel Create(
          ImmutableArray<Rubric> rubrics = new ImmutableArray<Rubric>(),
          ItemCardViewModel itemCard = null,
          string targetDescription = "",
          string depthOfKnowledge = "",
          string commonCoreStandardsDescription = "",
          string educationalDifficulty = "",
          string evidenceStatement = "")
        {

            return new AboutThisItemViewModel
            (
                rubrics: rubrics,
                itemCard: itemCard,
                targetDescription: targetDescription,
                depthOfKnowledge: depthOfKnowledge,
                commonCoreStandardsDescription: commonCoreStandardsDescription,
                educationalDifficulty: educationalDifficulty,
                evidenceStatement: evidenceStatement
            );


        }

        public static AboutThisItemViewModel FromSampleItem(SampleItem sampleItem, ImmutableArray<ItemCardViewModel> itemCards)
        {
            if (sampleItem == null)
            {
                return null;
            }

            var itemCardViewModel = itemCards
                .FirstOrDefault(card => card.BankKey == sampleItem.BankKey && card.ItemKey == sampleItem.ItemKey);
            var aboutThisItemViewModel = Create(
                rubrics: sampleItem.Rubrics,
                itemCard: itemCardViewModel,
                targetDescription: sampleItem.CoreStandards?.Target.Descripton,
                depthOfKnowledge: sampleItem.DepthOfKnowledge,
                commonCoreStandardsDescription: sampleItem.CoreStandards?.CommonCoreStandardsDescription,
                educationalDifficulty: sampleItem.EducationalDifficulty,
                evidenceStatement: sampleItem.EvidenceStatement);

            return aboutThisItemViewModel;
        }
    }
}
