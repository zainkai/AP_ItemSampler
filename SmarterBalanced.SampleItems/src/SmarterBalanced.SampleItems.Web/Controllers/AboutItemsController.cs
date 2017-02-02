﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

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

         public IActionResult GetItemUrl(string interactionType)
        {
            var itemUrl = repo.GetItemViewerUrl(interactionType);
            return Json(itemUrl);
        }
    }
}
