using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Core.ScoreGuide;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    [Route("ScoringGuide")]
    public class ScoringGuideController : Controller
    {
        private readonly IScoringRepo scoringRepo;
        private readonly ILogger logger;
        public ScoringGuideController(IScoringRepo scoringRepo, ILoggerFactory loggerFactory)
        {
            this.scoringRepo = scoringRepo;
            logger = loggerFactory.CreateLogger<ScoringGuideController>();

        }

        [HttpGet("ScoringGuideViewModel")]
        [EnableCors("AllowAllOrigins")]
        public IActionResult ScoringGuideViewModel()
        {
            var vm = scoringRepo.GetScoringGuideViewModel();
            return Json(vm);
        }

        [HttpGet("AboutThisItem")]
        [EnableCors("AllowAllOrigins")]
        public IActionResult AboutThisItem(int? bankKey, int? itemKey)
        {
            if (!bankKey.HasValue || !itemKey.HasValue)
            {
                logger.LogWarning($"{nameof(AboutThisItem)} null param(s), given {bankKey} {itemKey}");
                return BadRequest();
            }

            AboutThisItemViewModel aboutThis;
            try
            {
                aboutThis = scoringRepo.GetAboutThisItem(bankKey.Value, itemKey.Value);
            }
            catch (Exception e)
            {
                logger.LogWarning($"{nameof(AboutThisItem)} invalid request: {e.Message}");
                return BadRequest();
            }

            if (aboutThis == null)
            {
                logger.LogWarning($"{nameof(AboutThisItem)} incorrect param(s), given {bankKey} {itemKey}");
                return BadRequest();
            }

            return Json(aboutThis);

        }

        [HttpGet("Search")]
        [EnableCors("AllowAllOrigins")]
        public IActionResult Search(GradeLevels gradeLevels, string[] subject, string[] techType, bool braille)
        {
            var items = scoringRepo.GetItemCards(gradeLevels, subject, techType, braille);
            return Json(items);
        }
    }

}
