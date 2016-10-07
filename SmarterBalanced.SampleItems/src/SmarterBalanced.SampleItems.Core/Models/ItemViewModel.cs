using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Models
{
    public class ItemViewModel
    {
        public string ItemViewerServiceUrl { get; set; }

        public ItemDigest ItemDigest { get; set; }
    }
}
