using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using machineinfo.Data;
using System.Threading.Tasks;
using machineinfo.Models;

namespace machineinfo.Controllers
{
    public class MachinesController : Controller
    {
        private IDbConnection db;
        private IMachineRepository machineRepository;

        public MachinesController(IDbConnection db, IMachineRepository machineRepository)
        {
            this.db = db;
            this.machineRepository = machineRepository;
        }

        public async Task<IActionResult> Index()
        {
            var machines = await machineRepository.GetMachinesAsync();
            return View(machines);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("MachineName")]Machine machine)
        {
            try
            {
                machineRepository.CreateAsync(machine);
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
            var vm = await machineRepository.GetMachineByIDAsync(id);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var machine = await machineRepository.MachineToUpdateAsync(id);

            return View(machine);
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost(int? id, Machine machine)
        {
            if(id == null) return NotFound();
            try
            {
                machineRepository.UpdateAsync(id, machine);

                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(machine);
        }

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            
            machineRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}