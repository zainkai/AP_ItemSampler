using System;
using System.Collections.Generic;
using System.Text;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using Microsoft.Extensions.Logging;
using System.Linq;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Core.ScoreGuide.Models;
using SmarterBalanced.SampleItems.Core.Repos;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Core.ScoreGuide
{
    public class ScoringRepo : AboutItemsRepo, IScoringRepo
    {
        private readonly SampleItemsContext context;
        private readonly ILogger logger;
        public ScoringRepo(SampleItemsContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            this.context = context;
            logger = loggerFactory.CreateLogger<ScoringRepo>();
        }

        public ScoringGuideViewModel GetScoringGuideViewModel()
        {
            return new ScoringGuideViewModel
            {
                InteractionTypes = context.InteractionTypes,
                Subjects = context.Subjects
            };
        }

        public ItemCardViewModel GetItemCard(int bankKey, int itemKey)
        {
            return context.ItemCards.FirstOrDefault(i => i.BankKey == bankKey && i.ItemKey == itemKey);
        }


        public List<ItemCardViewModel> GetItemCards(GradeLevels gradeLevels, string[] subject, string[] techType, bool braille)
        {
            var scoreParams = new ScoreSearchParams(gradeLevels, subject, techType, braille);
            return GetItemCards(scoreParams);
        }

        public List<ItemCardViewModel> GetItemCards(ScoreSearchParams scoreParams = null)
        {
            var query = context.ItemCards.Where(i => i.Grade != GradeLevels.NA && !i.BrailleOnlyItem);

            if (scoreParams == null)
            {
                return query.OrderBy(i => i.SubjectCode).ThenBy(i => i.Grade).ThenBy(i => i.ClaimCode).ToList();
            }

            if (scoreParams.Grades != GradeLevels.All && scoreParams.Grades != GradeLevels.NA)
            {
                query = query.Where(i => GradeLevelsUtils.Contains(scoreParams.Grades, i.Grade));
            }

            if (scoreParams.Subjects != null && scoreParams.Subjects.Any())
            {
                query = query.Where(i => scoreParams.Subjects.Contains(i.SubjectCode));
            }

            //TODO: what is CAT technology? filter? ignore?
            if (scoreParams.TechType.Any(t => t.ToLower() == "pt"))
            {
                query = query.Where(i => i.IsPerformanceItem);
            }

            return query.OrderBy(i => i.SubjectCode).ThenBy(i => i.Grade).ThenBy(i => i.ClaimCode).ToList();
        }

        public ImmutableArray<AboutThisItemViewModel> GetAboutAllItems()
        {
            return context.AboutAllItems;
        }
    }
}
