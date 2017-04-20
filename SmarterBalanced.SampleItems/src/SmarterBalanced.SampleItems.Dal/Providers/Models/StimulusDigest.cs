using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class StimulusDigest
    {
        public int BankKey { get; set; }
        public int ItemKey { get; set; }
        public List<Content> Contents { get; set; }
    }
}
