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
    }
}