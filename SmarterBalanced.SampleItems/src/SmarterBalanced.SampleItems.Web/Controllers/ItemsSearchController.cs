using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class ItemsSearchController : Controller
    {
        private readonly ISampleItemsSearchRepo sampleItemsSearchRepo;
        private readonly ILogger logger;

        public ItemsSearchController(ISampleItemsSearchRepo repo, ILoggerFactory loggerFactory )
        {
            sampleItemsSearchRepo = repo;
            logger = loggerFactory.CreateLogger<ItemsSearchController>();
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
        public IActionResult Search(string itemID, GradeLevels gradeLevels, string[] subjects, string[] interactionTypes, string[] claims)
        {
            var parms = new ItemsSearchParams(itemID, gradeLevels, subjects, interactionTypes, claims);
            var items = sampleItemsSearchRepo.GetItemCards(parms);
            return Json(items);
        }

        // TODO: Search for three items above and three items below given grade level
        [HttpGet]
        public IActionResult SearchLikeThis(string itemID, GradeLevels gradeLevels)
        {
            var parms = new ItemsSearchParams(itemID, gradeLevels, null, null, null);
            
            // TODO: Implement search repo method for more like this
            var items = new List<ItemCardViewModel>(); //sampleItemsSearchRepo.GetItemCardsLikeThis(parms);

            return Json(items);
        }
    }
}
