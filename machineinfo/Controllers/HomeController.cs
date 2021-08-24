﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using machineinfo.Models;
using System.Data;
using Dapper;
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

        public IActionResult Index()
        {/* 
            db.Execute("Insert into Employee (first_name, last_name, address) values ('John', 'Smith', '123 Duane St');"); 
            db.Query("Select first_name from Employee;"); */
            db.Open();
            db.Close();
            db.Dispose();
            return View();
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
