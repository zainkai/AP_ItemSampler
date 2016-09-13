using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Interfaces
{
    public interface ISampleItemsRepo: IDisposable
    {
        IEnumerable<ItemDigest> GetItemDigests();

        ItemDigest GetItemDigest(int bankKey, int itemKey);

        ItemDigest GetItemDigest(Func<ItemDigest, bool> predicate);

        IEnumerable<ItemDigest> GetItemDigests(Func<ItemDigest, bool> predicate);

        IEnumerable<ItemDigest> GetItemDigestsByBankKey(int bankKey);

        IEnumerable<ItemDigest> GetItemDigestsByItemKey(int itemKey);

        IEnumerable<ItemDigest> GetItemDigestsByMinGrade(int minGrade);

        IEnumerable<ItemDigest> GetItemDigestsByMaxGrade(int maxGrade);

        IEnumerable<ItemDigest> GetItemDigestsByTargetedGrade(int targetedGrade);

        IEnumerable<ItemDigest> GetItemDigestsByGradeBand(int minGrade, int maxGrade);

        IEnumerable<ItemDigest> GetItemDigestsBySubject(string subjectCode);

        void Dispose();
    }
}
