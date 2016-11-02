using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using System;

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
        /// ISSAP based on URL or cookie if URL ISSAP not specified.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <param name="ISSAP"></param>
        public IActionResult Details(int? bankKey, int? itemKey, string ISSAP)
        {
            if (!bankKey.HasValue || !itemKey.HasValue)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(ISSAP))
            {
                string cookieName = repo.GetSettings().SettingsConfig.AccessibilityCookie;
                ISSAP = Request.Cookies[cookieName];
            }

            ItemViewModel item = repo.GetItemViewModel(bankKey.Value, itemKey.Value, ISSAP);

            if (item == null)
            {
                return BadRequest();
            }

            return View(item);
        }


        public IActionResult ResetLocalAccessibility()
        {
            throw new NotImplementedException();
        }

    }

}
