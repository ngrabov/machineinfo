using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Dapper;
using machineinfo.Models;
using Microsoft.AspNetCore.Http;

namespace machineinfo.Controllers
{
    public class FailuresController : Controller
    {
        private readonly ILogger<FailuresController> _logger;
        private IDbConnection db;

        public FailuresController(ILogger<FailuresController> logger, IDbConnection db)
        {
            _logger = logger;
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            var query = "SELECT * FROM failures";
            db.Open();
            var failures = await db.QueryAsync<Failure>(query);
            db.Close();
            db.Dispose();
            return View(failures);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description,Priority,Status,EntryTime,MachineId")]Failure failure, IFormFile file)
        {
            try
            {
                db.Open();
                
                var query = "INSERT INTO Failures (Name, Description, Priority, Status, EntryTime, MachineId) VALUES (@Name, @Description, @Priority, @Status, current_timestamp, @MachineId)";
                
                var parameters = new DynamicParameters();
                parameters.Add("Name", failure.Name);
                parameters.Add("Description", failure.Description);
                parameters.Add("Priority", failure.Priority);
                parameters.Add("Status", failure.Status);
                parameters.Add("EntryTime", System.DateTime.Now);
                parameters.Add("MachineId", failure.MachineId);
                
                await db.ExecuteAsync(query, parameters);
                db.Dispose();
                db.Close();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Bad model state");
            }
            ModelState.AddModelError("", "Bad model state.");
            return View(failure);
        }
    }
}