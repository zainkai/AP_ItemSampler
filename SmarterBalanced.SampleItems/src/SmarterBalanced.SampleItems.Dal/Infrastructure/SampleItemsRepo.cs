using SmarterBalanced.SampleItems.Dal.Context;
using SmarterBalanced.SampleItems.Dal.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Infrastructure
{
    public class SampleItemsRepo: ISampleItemsRepo
    {
        private ISampleItemsContext s_context;
        private bool s_disposed;

        /// <summary>
        /// Instantiate Context 
        /// </summary>
        public SampleItemsRepo() : this(new SampleItemsContext()) { }

        /// <summary>
        /// Constructor for repo
        /// </summary>
        /// <param name="context"></param>
        public SampleItemsRepo(ISampleItemsContext context)
        {
            s_context = context;
        }

        /// <summary>
        /// Get all ItemDigests with default order (BankKey, then ItemKey).
        /// </summary>
        /// <returns>
        /// An IEnumerable of ItemDigests
        /// </returns>
        public IEnumerable<ItemDigest> GetItemDigests()
        {
            return s_context.ItemDigests.OrderBy(t => t.BankKey)
                                        .ThenBy(t => t.ItemKey);
        }

        /// <summary>
        /// Get all ItemDigests matching the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>An IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigests(Func<ItemDigest, bool> predicate)
        {
            return GetItemDigests().Where(predicate);
        }
 
        /// <summary>
        /// Retreives the single specified ItemDigest.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>ItemDigest</returns>
        public ItemDigest GetItemDigest(Func<ItemDigest, bool> predicate)
        {
            return GetItemDigests().SingleOrDefault(predicate);
        }

        /// <summary>
        /// Get ItemDigest matching the specified identifier keys
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>ItemDigest</returns>
        public ItemDigest GetItemDigest(int bankKey, int itemKey)
        {
            return GetItemDigest(t => t.BankKey == bankKey && t.ItemKey == itemKey);
        }

        /// <summary>
        /// Get all ItemDigests matching the given bankKey.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <returns>IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigestsByBankKey(int bankKey)
        {
            return GetItemDigests(t => t.BankKey == bankKey);
        }

        /// <summary>
        /// Get all ItemDigests matching the given itemKey.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns>IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigestsByItemKey(int itemKey)
        {
            return GetItemDigests(t => t.ItemKey == itemKey);
        }

        /// <summary>
        /// Get all ItemDigests matching the given minGrade.
        /// </summary>
        /// <param name="minGrade"></param>
        /// <returns>IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigestsByMinGrade(int minGrade)
        {
            return GetItemDigests(t => t.MinGrade == minGrade);
        }

        /// <summary>
        /// Get all ItemDigests matching the given maxGrade.
        /// </summary>
        /// <param name="maxGrade"></param>
        /// <returns>IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigestsByMaxGrade(int maxGrade)
        {
            return GetItemDigests(t => t.MaxGrade == maxGrade);
        }

        /// <summary>
        /// Get all ItemDigests matching the given targetedGrade.
        /// </summary>
        /// <param name="targetedGrade"></param>
        /// <returns>IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigestsByTargetedGrade(int targetedGrade)
        {
            return GetItemDigests(t => t.TargetedGrade == targetedGrade);
        }

        /// <summary>
        /// Get all ItemDigests matching the given gradeBand (>= minGrade, =< maxGrade).
        /// </summary>
        /// <param name="minGrade"></param>
        /// <param name="maxGrade"></param>
        /// <returns>IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigestsByGradeBand(int minGrade, int maxGrade)
        {
            return GetItemDigests(t => t.MinGrade <= minGrade && t.MaxGrade >= maxGrade);
        }

        /// <summary>
        /// Get all ItemDigests with the given subject code
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <returns>IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigestsBySubject(string subjectCode)
        {
            return GetItemDigests(t => t.SubjectCode == subjectCode);
        }

        /// <summary>
        /// Dispose of the repo.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose of the repo.
        /// </summary>
        /// <param name="isDisposing"></param>
        private void Dispose(bool isDisposing)
        {
            throw new NotImplementedException();
        }


    }
}
