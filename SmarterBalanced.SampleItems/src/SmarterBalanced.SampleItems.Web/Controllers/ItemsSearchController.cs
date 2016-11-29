using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class ItemsSearchController : Controller
    {
        private ISampleItemsSearchRepo sampleItemsSearchRepo;

        public ItemsSearchController(ISampleItemsSearchRepo repo)
        {
            sampleItemsSearchRepo = repo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = sampleItemsSearchRepo.GetItemsSearchViewModel();
            return View(model);
        }

        [HttpGet]
        public IActionResult Search(GradeLevels gradeLevels, string[] subjects, string[] interactionTypes)
        {
            var items = sampleItemsSearchRepo.GetItemDigests(gradeLevels, subjects, interactionTypes);
            return Json(items);
        }
    }
}
