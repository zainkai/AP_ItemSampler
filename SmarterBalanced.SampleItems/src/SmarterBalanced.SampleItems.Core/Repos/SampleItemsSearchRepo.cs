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

        public IList<ItemDigest> GetItemDigests()
        {
            return context.ItemDigests.Where(i => i.Grade != GradeLevels.NA).ToList();
        }

        // TODO: what should terms search on?
        public IList<ItemDigest> GetItemDigests(GradeLevels grades, IList<string> subjects, string[] interactionTypes, string[] claimIds)
        {
            var query = context.ItemDigests.Where(i => i.Grade != GradeLevels.NA);
            if (grades != GradeLevels.All && grades != GradeLevels.NA)
                query = query.Where(i => GradeLevelsUtils.Contains(grades, i.Grade));

            if (subjects != null && subjects.Any())
                query = query.Where(i => subjects.Contains(i.SubjectId));

            if (interactionTypes.Any())
                query = query.Where(i => interactionTypes.Contains(i.InteractionTypeCode));

            if (claimIds.Any())
                query = query.Where(i => claimIds.Contains(i.Claim.Code));

            return query.OrderBy(i => i.SubjectId).ThenBy(i => i.Grade.ToString().Length).ThenBy(i => i.Grade.ToString()).ThenBy(i => i.ClaimId).ToList();
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
