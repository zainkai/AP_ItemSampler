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
        /// <param name="iSSAP"></param>
        public IActionResult Details(int? bankKey, int? itemKey, string iSSAP)
        {
            if (!bankKey.HasValue || !itemKey.HasValue)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(iSSAP))
            {
                string cookieName = repo.GetSettings().SettingsConfig.AccessibilityCookie;
                iSSAP = Request.Cookies[cookieName];
            }

            ItemViewModel item = repo.GetItemViewModel(bankKey.Value, itemKey.Value, iSSAP);

            if (item == null)
            {
                return BadRequest();
            }

            return View(item);
        }


        public IActionResult ResetLocalAccessibility()
        {
            
            string cookieName = repo.GetSettings().SettingsConfig.AccessibilityCookie;
            var iSSAP = Request.Cookies[cookieName];
            
            // TODO: udpate model accessibility with issap

            throw new NotImplementedException();
        }

    }

}
