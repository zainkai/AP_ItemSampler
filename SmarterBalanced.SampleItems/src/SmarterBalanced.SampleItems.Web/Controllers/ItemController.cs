using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;

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

            ItemViewModel item = itemViewRepo.GetItemViewModel(bankKey.Value, itemKey.Value);

            if (item == null)
            {
                return BadRequest();
            }

            return View(item);
        }
    }
}
