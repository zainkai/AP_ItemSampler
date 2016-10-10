using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Interfaces;
using SmarterBalanced.SampleItems.Core.Infrastructure;
using SmarterBalanced.SampleItems.Dal.Models;

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
