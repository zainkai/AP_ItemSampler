using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class ItemIdentifier
    {
        public string ItemName { get; }
        public int ItemKey { get; }
        public int BankKey { get; }

        public ItemIdentifier(
            string itemName,
            int bankKey,
            int itemKey
            
            )
        {
            ItemName = itemName;
            BankKey = bankKey;
            ItemKey = itemKey;
        }
    }
}
