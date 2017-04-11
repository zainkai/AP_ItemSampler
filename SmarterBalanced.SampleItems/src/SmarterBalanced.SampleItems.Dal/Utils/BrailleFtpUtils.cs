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
        public static ImmutableArray<string> GetItemBrailleCodes(IEnumerable<AccessibilityResourceGroup> groups)
        {
            List<string> brailleCodes = new List<string>();
            foreach (AccessibilityResourceGroup group in groups)
            {
                foreach (AccessibilityResource res in group.AccessibilityResources)
                {

                    if (res.ResourceCode == "BrailleType")
                    {
                        foreach (AccessibilitySelection sel in res.Selections)
                        {
                            brailleCodes.Add(sel.SelectionCode);
                        }
                    }

                }
            }
            return brailleCodes.ToImmutableArray();
        }

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

        public static async Task<ImmutableArray<string>> AvailableItemBraille(FtpClient ftpClient,
            IEnumerable<string> codes,
            int itemKey,
            string subject,
            string grade)
        {
            string directory = GetItemDirectoryPath(itemKey, subject, grade);
            var matchedCodes = await AvailableBraille(ftpClient, codes, directory);
            return matchedCodes;
        }

        public static async Task<ImmutableArray<string>> AvailablePassageBraille(
            FtpClient ftpClient,
            IEnumerable<string> codes,
            int passageKey,
            string subject,
            string grade)
        {
            string directory = GetPassageDirectoryPath(passageKey, subject, grade);
            var matchedCodes = await AvailableBraille(ftpClient, codes, directory);
            return matchedCodes;
        }

        /// <summary>
        /// Uses the passed in FTP client to list the files in the specified directory and match them with braille accessibility codes.
        /// </summary>
        /// <param name="ftpClient"></param>
        /// <param name="codes"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        private static async Task<ImmutableArray<string>> AvailableBraille(
            FtpClient ftpClient,
            IEnumerable<string> codes,
            string directory)
        {
            try
            {
                await ftpClient.ChangeWorkingDirectoryAsync(directory);
                var files = await ftpClient.ListFilesAsync();
                var fileNames = files.Select(f => f.Name).ToList();
                return MatchCodes(fileNames, codes);
            }
            catch (Exception e)
            {
                //Directory does not exist
                //Return an empty list
                return ImmutableArray<string>.Empty;
            }
        }

        private static ImmutableArray<string> MatchCodes(IEnumerable<string> fileNames, IEnumerable<string> codes)
        {
            List<string> matchedCodes = new List<string>();
            foreach (string code in codes)
            {
                var brailleType = GetBrailleTypeFromCode(code).ToLower();
                if (brailleType == "")
                {
                    //If there is no braille type to not check for a file
                    continue;
                }
                bool exists = fileNames.Where(t => Regex.IsMatch(t, $"(?i){brailleType}")).Any();
                if (exists)
                {
                    matchedCodes.Add(code);
                }
            }
            return matchedCodes.ToImmutableArray();
        }


        private static string GetItemDirectoryPath(int id, string subject, string grade)
        {
            return $"/~sbacpublic/Public/PracticeAndTrainingTests/2016-2017_PracticeAndTrainingBrailleFiles/{subject}/{grade}/item-{id}/";
        }

        public static string BuildBrailleFilePath(SampleItem item, string code)
        {
            var subject = item.Subject.Code;
            var grade = item.Grade.IndividualGradeToNumString();
            var itemId = item.ItemKey;
            return $"~sbacpublic/Public/PracticeAndTrainingTests/2016-2017_PracticeAndTrainingBrailleFiles/{subject}/{grade}/item-{itemId}/item_{itemId}_enu_{code}.brf";
        }

        private static string GetPassageDirectoryPath(int id, string subject, string grade)
        {
            return $"/~sbacpublic/Public/PracticeAndTrainingTests/2016-2017_PracticeAndTrainingBrailleFiles/{subject}/{grade}/stim-{id}/";
        }
    }
}
