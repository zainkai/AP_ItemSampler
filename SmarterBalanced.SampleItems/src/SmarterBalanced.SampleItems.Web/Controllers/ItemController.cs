using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Interfaces;
using SmarterBalanced.SampleItems.Core.Infrastructure;
using SmarterBalanced.SampleItems.Dal.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class ItemController : Controller
    {
        private IItemViewRepo itemViewRepo;

        public ItemController(IItemViewRepo repo)
        {
            itemViewRepo = repo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns an ItemDigest given a bankKey and itemKey
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public IActionResult Details(int? bankKey, int? itemKey)
        {
            if(!bankKey.HasValue || !itemKey.HasValue)
            {
                return BadRequest();
            }

            ItemDigest item = itemViewRepo.GetItemDigest(bankKey.Value, itemKey.Value);
            if (item == null)
            {
                return BadRequest();
            }

            return View(item);
        }
    }
}
