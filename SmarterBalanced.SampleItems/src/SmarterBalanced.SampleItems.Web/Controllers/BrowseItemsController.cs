using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class BrowseItemsController : Controller
    {
        private readonly ISampleItemsSearchRepo sampleItemsSearchRepo;
        private readonly ILogger logger;

        public BrowseItemsController(ISampleItemsSearchRepo repo, ILoggerFactory loggerFactory )
        {
            sampleItemsSearchRepo = repo;
            logger = loggerFactory.CreateLogger<BrowseItemsController>();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = sampleItemsSearchRepo.GetItemsSearchViewModel();
            if(model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Search(string itemID, GradeLevels gradeLevels, string[] subjects, string[] interactionTypes, string[] claims, bool performanceOnly)
        {
            var parms = new ItemsSearchParams(itemID, gradeLevels, subjects, interactionTypes, claims, performanceOnly);
            var items = sampleItemsSearchRepo.GetItemCards(parms);
            return Json(items);
        }

    }

}
