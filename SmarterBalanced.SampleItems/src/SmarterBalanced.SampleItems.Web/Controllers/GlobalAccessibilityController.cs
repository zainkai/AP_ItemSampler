using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;

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
        public async Task<IActionResult> Index()
        {
            string cookieName = repo.GetSettings().SettingsConfig.AccessibilityCookie;
            var iSAAP = Request.Cookies[cookieName];
            GlobalAccessibilityViewModel viewmodel = await repo.GetGlobalAccessibilityViewModelAsync(iSAAP);

            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(GlobalAccessibilityViewModel model)
        {
            string cookieName = repo.GetSettings().SettingsConfig.AccessibilityCookie;
            string iSAAP = await repo.GetISAAPCodeAsync(model);
            Response.Cookies.Append(cookieName, iSAAP);

            return RedirectToAction("Index");
        }

        public IActionResult Reset()
        {
            string cookieName = repo.GetSettings().SettingsConfig.AccessibilityCookie;
            Response.Cookies.Delete(cookieName);

            return RedirectToAction("Index");
        }

    }

}
