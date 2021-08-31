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
            var failure = await db.QueryAsync<Failure>(query);
            db.Close();
            db.Dispose();
            return View(failure);
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
            catch(System.Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(failure);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var query = "SELECT * FROM failures WHERE FailureID = @Id";
            var failure = await db.QuerySingleOrDefaultAsync<Failure>(query, new {id});
            var name = "SELECT Name FROM Machines WHERE MachineId = '" + @failure.MachineId + "'";
            var mac = await db.QuerySingleOrDefaultAsync<Machine>(name, new{failure.MachineId});
            ViewData["MachineName"] = mac.Name;
            return View(failure);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            db.Open();

            var query = "SELECT * FROM failures WHERE FailureId = @id";
            var failure = await db.QuerySingleOrDefaultAsync<Failure>(query, new{id});

            db.Close();
            db.Dispose();
            return View(failure);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id, Failure failure)
        {
            if(id == null) return NotFound();
            db.Open();
            var query = "UPDATE failures SET Name = @Name, Description = @Description, Priority = @Priority, Status = @Status, MachineId = @MachineId WHERE FailureId = '" + @id + "'";

            var param = new DynamicParameters();
            param.Add("Name", failure.Name);
            param.Add("Description", failure.Description);
            param.Add("Priority", failure.Priority);
            param.Add("Status", failure.Status);
            param.Add("MachineId", failure.MachineId);

            await db.ExecuteAsync(query, param);
            db.Close();
            db.Dispose();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Resolve(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            db.Open();

            var query = "UPDATE failures SET Status = '1' WHERE FailureId = '" + @id + "'";
            await db.ExecuteAsync(query, new{id});
            db.Close();
            db.Dispose();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var query = "DELETE FROM failures WHERE FailureId = @Id";
            await db.ExecuteAsync(query, new {id});
            return RedirectToAction(nameof(Index));
        }
    }
}