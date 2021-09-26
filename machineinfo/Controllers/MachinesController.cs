using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using machineinfo.Models;
using machineinfo.Services;

namespace machineinfo.Controllers
{
    public class MachinesController : Controller
    {
        private IMachineService service;

        public MachinesController(IMachineService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await service.GetMachinesAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("MachineName")]Machine machine)
        {
            var k = await service.Create(machine);
            if(k == 0) return View(machine);
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            return View(await service.GetMachineByIDAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await service.MachineToUpdateAsync(id));
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id, Machine machine)
        {
            if(id == null) return NotFound();

            var j = await service.Update(id, machine);
            if(j == 0) return View(machine);
            else 
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            
            service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}