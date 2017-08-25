using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Core.ScoreGuide.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmarterBalanced.SampleItems.Core.ScoreGuide
{
    public interface IScoringRepo
    {
        ScoringGuideViewModel GetScoringGuideViewModel();
        ItemCardViewModel GetItemCard(int bankKey, int itemKey);
        List<ItemCardViewModel> GetItemCards(GradeLevels gradeLevels, string[] subject, string[] techType, bool braille);
        AboutThisItemViewModel GetAboutThisItem(int itemBank, int itemKey);

    }
}