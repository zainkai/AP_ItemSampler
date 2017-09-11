using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Core.ScoreGuide;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Immutable;

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

        [HttpGet("AboutAllItems")]
        [EnableCors("AllowAllOrigins")]
        public IActionResult AboutAllItems()
        {
            ImmutableArray<AboutThisItemViewModel> aboutThis;
            try
            {
                aboutThis = scoringRepo.GetAboutAllItems();
            }
            catch (Exception e)
            {
                logger.LogWarning($"{nameof(AboutAllItems)} invalid request: {e.Message}");
                return BadRequest();
            }

            if (aboutThis == null)
            {
                logger.LogWarning($"{nameof(AboutAllItems)} unable to get about all items from context");
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
