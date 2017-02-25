using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using Microsoft.Extensions.Logging;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class AboutItemsController : Controller
    {
        private readonly IAboutItemsRepo repo;
        private readonly AppSettings appSettings;
        private readonly ILogger logger;

        public AboutItemsController(IAboutItemsRepo aboutItemRepo, AppSettings settings, ILoggerFactory loggerFactory)
        {
            repo = aboutItemRepo;
            appSettings = settings;
            logger = loggerFactory.CreateLogger<AboutItemsController>();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var viewModel = repo.GetAboutItemsViewModel();
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult GetItemUrl(string interactionTypeCode)
        {
            var itemUrl = repo.GetItemViewerUrl(interactionTypeCode);
            return Json(itemUrl);
        }

    }

}
