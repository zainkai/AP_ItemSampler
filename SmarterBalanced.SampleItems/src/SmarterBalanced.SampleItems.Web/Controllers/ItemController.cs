using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Interfaces;
using SmarterBalanced.SampleItems.Core.Infrastructure;
using SmarterBalanced.SampleItems.Dal.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class ItemController : Controller
    {
        private IItemViewRepo i_repo;

        public ItemController(ISampleItemsRepo repo)
        {
            i_repo = new ItemViewRepo(repo);
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
