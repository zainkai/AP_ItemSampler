﻿using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class BrowseItemsController : Controller
    {
        private readonly ISampleItemsSearchRepo sampleItemsSearchRepo;
        private readonly ILogger logger;

        public BrowseItemsController(ISampleItemsSearchRepo repo, ILoggerFactory loggerFactory )
        {
            sampleItemsSearchRepo = repo;
            logger = loggerFactory.CreateLogger<BrowseItemsController>();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = sampleItemsSearchRepo.GetItemsSearchViewModel();
            if(model == null)
            {
                return BadRequest();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Search(string itemID, GradeLevels gradeLevels, string[] subjects, string[] interactionTypes, string[] claims, bool performanceOnly, int[] targets)
        {
            var parms = new ItemsSearchParams(itemID, gradeLevels, subjects, interactionTypes, claims, performanceOnly, targets);
            var items = sampleItemsSearchRepo.GetItemCards(parms);
            return Json(items);
        }

        [HttpGet]
        public IActionResult ExportItems()
        {
            var baseUrl = Request.Host.ToString();
            var items = sampleItemsSearchRepo.GetSampleItemViewModels(baseUrl);
            return Json(items);
        }

        [HttpGet]
        public IActionResult Export()
        {
            var baseUrl = Request.Host.ToString();
            var items = sampleItemsSearchRepo.GetSampleItemViewModels(baseUrl);
            var csvStream = new MemoryStream();
            var writer = new StreamWriter(csvStream, System.Text.Encoding.UTF8);
            var csv = new CsvWriter(writer);

            csv.WriteRecords(items);
            writer.Flush();
            csvStream.Seek(0, SeekOrigin.Begin);

            return File(csvStream, "text/csv", "SIWItems.csv");
        }
    }
}
