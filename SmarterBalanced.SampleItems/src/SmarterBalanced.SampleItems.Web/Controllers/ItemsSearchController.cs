using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;

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
            IList<ItemDigest> items = sampleItemsSearchRepo.GetItemDigests();
            return View(items);
        }

        [HttpGet]
        public IActionResult Search(string terms, GradeLevels gradeLevels, string[] subjects, string claimType)
        {
            var items = sampleItemsSearchRepo.GetItemDigests(terms, gradeLevels, subjects, claimType);

            return Json(items);
        }
    }
}
