using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Translations
{
    public static class ItemTranslations
    {
        public static AboutItemViewModel ToAboutItemViewModel(this ItemDigest digest)
        {
            return new AboutItemViewModel(
                    itemKey: digest.ItemKey,
                    commonCoreStandardsId: digest.CommonCoreStandardsId,
                    targetId: digest.TargetId,
                    grade: digest.Grade,
                    rubrics: digest.Rubrics
                );
        }
    }
}
