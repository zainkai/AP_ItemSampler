using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class GlobalAccessibilityController : Controller
    {
        private IGlobalAccessibilityRepo repo;

        public GlobalAccessibilityController(IGlobalAccessibilityRepo globalAccessibilityRepo)
        {
            repo = globalAccessibilityRepo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            //see if cookies exist, if not get default 
            //load cookie
            //get view model

            var viewmodel = repo.GetGlobalAccessibilityViewModel();
        
            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult Index(GlobalAccessibilityViewModel model)
        {
            //Get ISSAP
            string iSSAPCookie = repo.GetISSAPCode(model);
            //Save to Cookie

            //redirect to index
            return View();
        }
    }
}
