using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gen = SmarterBalanced.SampleItems.Dal.Xml.Models;


namespace SmarterBalanced.SampleItems.Core.Repos
{
    public class SampleItemsSearchRepo : ISampleItemsSearchRepo
    {
        private readonly SampleItemsContext context;
        private readonly ILogger logger;

        public SampleItemsSearchRepo(SampleItemsContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            logger = loggerFactory.CreateLogger<SampleItemsSearchRepo>();
        }

        public IList<ItemCardViewModel> GetItemCards()
        {
            return context.ItemCards.ToList();
        }

        public IList<ItemCardViewModel> GetItemCards(ItemsSearchParams parms)
        {
            int itemId;
            if (int.TryParse(parms.ItemId, out itemId))
            {
                var item = context.ItemCards.FirstOrDefault(i => i.ItemKey == itemId);
                if (item == null)
                    return new List<ItemCardViewModel>();
                else
                    return new List<ItemCardViewModel> { item };
            }

            var query = context.ItemCards.Where(i => i.Grade != GradeLevels.NA);
            if (parms.Grades != GradeLevels.All && parms.Grades != GradeLevels.NA)
                query = query.Where(i => GradeLevelsUtils.Contains(parms.Grades, i.Grade));

            if (parms.Subjects != null && parms.Subjects.Any())
                query = query.Where(i => parms.Subjects.Contains(i.SubjectCode));

            if (parms.InteractionTypes.Any())
                query = query.Where(i => parms.InteractionTypes.Contains(i.InteractionTypeCode));

            if (parms.ClaimIds.Any())
                query = query.Where(i => parms.ClaimIds.Contains(i.ClaimCode));

            return query.OrderBy(i => i.SubjectCode).ThenBy(i => i.Grade).ThenBy(i => i.ClaimCode).ToList();
        }

        public ItemsSearchViewModel GetItemsSearchViewModel()
        {
            return new ItemsSearchViewModel
            {
                InteractionTypes = context.InteractionTypes,
                Subjects = context.Subjects
            };
        }
    }
}
