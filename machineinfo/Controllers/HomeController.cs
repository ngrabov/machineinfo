using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using machineinfo.Models;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using machineinfo.ViewModels;

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
            var query = "SELECT * FROM Failures WHERE Status = '0' ORDER BY Priority, EntryTime";
            var failures = await db.QueryAsync<Failure>(query);
            db.Close();
            db.Dispose();
            return View(failures);
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
