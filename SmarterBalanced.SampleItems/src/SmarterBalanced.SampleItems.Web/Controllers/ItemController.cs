using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using System;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class ItemController : Controller
    {
        private IItemViewRepo repo;
        public ItemController(IItemViewRepo itemViewRepo)
        {
            repo = itemViewRepo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns an ItemDigest given a bankKey and itemKey, setting
        /// ISAAP based on URL or cookie if URL ISAAP not specified.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <param name="iSAAP"></param>
        public async Task<IActionResult> Details(int? bankKey, int? itemKey, string iSAAP)
        {
            if (!bankKey.HasValue || !itemKey.HasValue)
            {
                return BadRequest();
            } 

            if (string.IsNullOrEmpty(iSAAP))
            {
                string cookieName = repo.AppSettings.SettingsConfig.AccessibilityCookie;
                iSAAP = CookieManager.GetCookie(Request, cookieName);
            }

            ItemViewModel itemViewModel = await repo.GetItemViewModelAsync(bankKey.Value, itemKey.Value, iSAAP);
            if (itemViewModel == null)
            {
                return BadRequest();
            }

            return View(itemViewModel);
        }

        /// <summary>
        /// Resets item accessibility to global accessibility settings
        /// or default if global cookie does not exist.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public async Task<IActionResult> ResetToGlobalAccessibility(int? bankKey, int? itemKey)
        {
            if (!bankKey.HasValue || !itemKey.HasValue)
            {
                return BadRequest();
            }

            string cookieName = repo.AppSettings.SettingsConfig.AccessibilityCookie;
            string iSAAP = CookieManager.GetCookie(Request, cookieName);

            ItemViewModel itemViewModel = await repo.GetItemViewModelAsync(bankKey.Value, itemKey.Value, iSAAP);
            if (itemViewModel == null)
            {
                return BadRequest();
            }

            return PartialView("_LocalAccessibility", itemViewModel.LocalAccessibilityViewModel);
        }

    }

}
