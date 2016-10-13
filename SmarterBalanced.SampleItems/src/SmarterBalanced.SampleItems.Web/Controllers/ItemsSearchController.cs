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
    }
}
