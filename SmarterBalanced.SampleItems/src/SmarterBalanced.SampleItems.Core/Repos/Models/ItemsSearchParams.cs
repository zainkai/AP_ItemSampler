﻿using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class ItemsSearchParams
    {
        public string[] ClaimIds { get; }
        public GradeLevels Grades { get; }
        public string[] InteractionTypes { get; }
        public string ItemId { get; }
        public IList<string> Subjects { get; }
        public bool PerformanceOnly { get; }
        public int[] Targets { get; }

        public ItemsSearchParams(
            string itemId,
            GradeLevels grades,
            IList<string> subjects,
            string[] interactionTypes,
            string[] claimIds,
            bool performanceOnly,
            int[] targets)
        {
            ItemId = itemId;
            Grades = grades;
            Subjects = subjects;
            InteractionTypes = interactionTypes;
            ClaimIds = claimIds;
            PerformanceOnly = performanceOnly;
            Targets = targets;
        }
    }
}
