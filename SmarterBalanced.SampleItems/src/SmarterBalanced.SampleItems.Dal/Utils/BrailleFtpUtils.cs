using CoreFtp;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Utils
{
    public class BrailleFtpUtils
    {
        public static string GetBrailleTypeFromCode(string code)
        {
            //Codes look like TDS_BT_TYPE except for the no braille code which looks like TDS_BT0
            var bt = code.Split('_');
            if (bt.Length != 3)
            {
                return string.Empty;
            }
            return bt[2];
        }
    }
}
