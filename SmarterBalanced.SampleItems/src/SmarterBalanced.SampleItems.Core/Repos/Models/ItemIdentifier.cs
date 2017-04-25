using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class ItemIdentifier
    {
        public string ItemName { get; }
        public string ItemKey { get; }
        public string BankKey { get; }

        public ItemIdentifier(
            string itemName,
            string itemKey,
            string bankKey
            )
        {
            ItemName = itemName;
            ItemKey = itemKey;
            BankKey = bankKey;
        }
    }
}
