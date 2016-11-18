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
        public async Task<IActionResult> Index()
        {
            var model = await sampleItemsSearchRepo.GetItemsSearchViewModelAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string terms, GradeLevels gradeLevels, string[] subjects, string[] interactionTypes)
        {
            var items = await sampleItemsSearchRepo.GetItemDigestsAsync(terms, gradeLevels, subjects, interactionTypes);

            return Json(items);
        }
    }
}
