using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Braille
{
    public interface IBrailleRepo
    {
        string GetBrailleItemNames(SampleItem item);
        IList<SampleItem> GetAssociatedBrailleItems(SampleItem item);
        SampleItem GetAssoicatedBrailleItem(SampleItem item);

        Task<Stream> GetItemBrailleZip(int itemBank, int itemKey, string brailleCode);
        string GenerateBrailleZipName(int itemId, string brailleCode);
    }
}