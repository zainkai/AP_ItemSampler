using System;
using System.Collections.Generic;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Threading.Tasks;
using System.IO;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public interface IItemViewRepo
    {
        Task<Stream> GetFtpFile(int itemBank, int itemKey, string brailleCode);
        Task<ItemViewModel> GetItemViewModel(
            int bankKey,
            int itemKey,
            string[] iSAAPCodes,
            Dictionary<string, string> cookieValue);

        MoreLikeThisViewModel GetMoreLikeThis(SampleItem sampleItem);

        AboutThisItemViewModel GetAboutThisItemViewModel(SampleItem sampleItem);
    }
}
