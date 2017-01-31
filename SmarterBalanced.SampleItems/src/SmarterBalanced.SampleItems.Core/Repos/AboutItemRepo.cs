using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class AboutItemRepo : ItemViewRepo
    {
        private readonly SampleItemsContext context;
        private readonly ILogger logger;

        public AboutItemRepo(SampleItemsContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            this.context = context;
            logger = loggerFactory.CreateLogger<AboutItemRepo>();
        }

        public ImmutableArray<InteractionType> GetAboutItemsViewModel()
        {
            var interactionTypes = context.InteractionTypes;
            return interactionTypes;
        }

        public string GetItemViewerUrl(string interactionTypeCode)
        {
            var itemDigest = context.ItemDigests
                .Where(i => i.Grade != GradeLevels.NA)
                .OrderBy(i => (int)i.Grade)
                .FirstOrDefault(item => item.InteractionType.Code == interactionTypeCode);
            return GetItemViewerUrl(itemDigest);
        }


    }
}
