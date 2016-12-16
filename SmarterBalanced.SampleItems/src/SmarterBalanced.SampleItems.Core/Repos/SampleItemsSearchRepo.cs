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
            return context.ItemCards.Where(i => i.Grade != GradeLevels.NA).ToList();
        }

        // TODO: what should terms search on?
        public IList<ItemCardViewModel> GetItemCards(GradeLevels grades, IList<string> subjects, string[] interactionTypes, string[] claimIds)
        {
            var query = context.ItemCards.Where(i => i.Grade != GradeLevels.NA);
            if (grades != GradeLevels.All && grades != GradeLevels.NA)
                query = query.Where(i => GradeLevelsUtils.Contains(grades, i.Grade));

            if (subjects != null && subjects.Any())
                query = query.Where(i => subjects.Contains(i.SubjectCode));

            if (interactionTypes.Any())
                query = query.Where(i => interactionTypes.Contains(i.InteractionTypeCode));

            if (claimIds.Any())
                query = query.Where(i => claimIds.Contains(i.ClaimCode));

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
