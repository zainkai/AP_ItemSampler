using CsvHelper;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public class BrailleManifestReader
    {
        /// <summary>
        /// Reads the braille file manifest
        /// </summary>
        /// <param name="brailleFile">Path to the braille manifest</param>
        /// <returns></returns>
        public static ImmutableArray<BrailleFileInfo> GetBrailleFileInfo(string brailleFile)
        {
            List<BrailleFileInfo> fileInfo = new List<BrailleFileInfo>();
            using (var inputStream = new FileStream(brailleFile, FileMode.Open))
            using (var reader = new StreamReader(inputStream))
            using (var csv = new CsvReader(reader))
            {
                csv.ReadHeader();
                while (csv.Read())
                {
                    var subject = csv.GetField<string>("Subject");
                    var grade = csv.GetField<int>("Grade");
                    var itemId = csv.GetField<int>("ItemOrStimId");
                    var brailleType = csv.GetField<string>("UebCode");
                    fileInfo.Add(
                        new BrailleFileInfo(
                            itemKey: itemId,
                            subject: subject,
                            grade: GradeLevelsUtils.FromString(grade.ToString()),
                            brailleType: $"TDS_BT_{brailleType}"
                        )
                    );
                }
            }
            return fileInfo.ToImmutableArray();
        }
    }
}
