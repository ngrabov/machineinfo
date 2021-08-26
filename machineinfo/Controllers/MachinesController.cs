using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Threading.Tasks;
using machineinfo.Models;

namespace machineinfo.Controllers
{
    public class MachinesController : Controller
    {
        private readonly ILogger<MachinesController> _logger;
        private IDbConnection db;

        public MachinesController(ILogger<MachinesController> logger, IDbConnection db)
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

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var query = "SELECT * FROM Machines WHERE MachineId = @Id";
            var machine = await db.QuerySingleOrDefaultAsync<Machine>(query, new{ id });
            return View(machine);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            db.Open();

            var query = "SELECT * FROM machines WHERE MachineId = @id";
            var machine = await db.QuerySingleOrDefaultAsync<Machine>(query, new{id});

            db.Close();
            db.Dispose();
            return View(machine);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id, Machine machine)
        {
            if(id == null) return NotFound();
            try
            {
                db.Open();
                var query = "UPDATE Machines SET Name = @Name WHERE MachineId = '" + @id + "'";

                var param = new DynamicParameters();
                param.Add("Name", machine.Name);

                await db.ExecuteAsync(query, machine);
                db.Close();
                db.Dispose();
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(machine);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            db.Open();
            var query = "DELETE FROM failures WHERE MachineId = @id";
            await db.ExecuteAsync(query, new {id});
            var query2 = "DELETE FROM machines WHERE MachineId = @id";
            await db.ExecuteAsync(query2, new {id});
            db.Close();
            db.Dispose();
            return RedirectToAction(nameof(Index));
        }
    }
}