using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class Rubric
    {
        public string Language { get; }
        public ImmutableArray<RubricEntry> RubricEntries { get; }
        public ImmutableArray<RubricSample> Samples { get; }

        public Rubric(string language, ImmutableArray<RubricEntry> rubrics, ImmutableArray<RubricSample> samples)
        {
            Language = language;
            RubricEntries = rubrics;
            Samples = samples;
        }

        
    }
}
