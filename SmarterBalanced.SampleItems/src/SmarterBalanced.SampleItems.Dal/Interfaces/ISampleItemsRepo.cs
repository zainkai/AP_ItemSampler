using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Interfaces
{
    public interface ISampleItemsRepo : IDisposable
    {
        IEnumerable<ItemDigest> GetItemDigests();
        void Dispose();
    }
}
