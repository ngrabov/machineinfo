﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using machineinfo.Models;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Npgsql;

namespace machineinfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IDbConnection db;

        public HomeController(ILogger<HomeController> logger, IDbConnection db)
        {
            _logger = logger;
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            db.Open();
            var query = "SELECT * FROM Machines";
            var machines = await db.QueryAsync<Machine>(query);
            db.Close();
            db.Dispose();
            return View(machines);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name")]Machine machine)
        {
            try
            {
                db.Open();
                var query = "INSERT INTO machines (Name) VALUES (@Name)";
                var param = new DynamicParameters();
                param.Add("Name", machine.Name);

                await db.ExecuteAsync(query, param);
                db.Dispose();
                db.Close();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Model error.");
            }
            ModelState.AddModelError("", "Model error.");
            return View(machine);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
