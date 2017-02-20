using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class CoreStandardsXml
    {
        public ImmutableArray<CoreStandardsRow> TargetRows { get; }
        public ImmutableArray<CoreStandardsRow> CcssRows { get; }  

        public CoreStandardsXml(ImmutableArray<CoreStandardsRow> targetRows, ImmutableArray<CoreStandardsRow> ccssRows)
        {
            TargetRows = targetRows;
            CcssRows = ccssRows;
        }

    }
}
