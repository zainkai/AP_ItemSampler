using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemViewRepo repo;
        private readonly ILogger logger;

        public ItemController(IItemViewRepo itemViewRepo, ILoggerFactory loggerFactory)
        {
            repo = itemViewRepo;
            logger = loggerFactory.CreateLogger<ItemController>();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            logger.LogDebug($"{nameof(Index)} redirect to itemssearch");

            return RedirectToActionPermanent("Index", "itemsSearch");
        }

        /// <summary>
        /// Returns an ItemDigest given a bankKey and itemKey, setting
        /// ISAAP based on URL or cookie if URL ISAAP not specified.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <param name="iSAAP"></param>
        public IActionResult Details(int? bankKey, int? itemKey, string iSAAP)
        {
            if (!bankKey.HasValue || !itemKey.HasValue)
            {
                logger.LogWarning($"{nameof(Details)} null param(s) for {bankKey} {itemKey}");
                return BadRequest();
            }

            string cookieName = repo.AppSettings.SettingsConfig.AccessibilityCookie;
            string cookieValue = Request?.Cookies[cookieName] ?? string.Empty;
            string[] isaapCodes = string.IsNullOrEmpty(iSAAP) ? new string[0] : iSAAP.Split(';');
            
            ItemViewModel itemViewModel = repo.GetItemViewModel(bankKey.Value, itemKey.Value, isaapCodes, cookieValue);
            if (itemViewModel == null)
            {
                logger.LogWarning($"{nameof(Details)} invalid item {bankKey} {itemKey}");
                return BadRequest();
            }

            return View(itemViewModel);
        }
    }

}
