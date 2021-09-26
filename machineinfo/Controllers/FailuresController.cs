using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using machineinfo.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using machineinfo.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using machineinfo.Services;

namespace machineinfo.Controllers
{
    public class FailuresController : Controller
    {
        private IFailureRepository failureRepository;
        private IFailureService service;

        public FailuresController(IFailureRepository failureRepository, IFailureService service)
        {
            this.failureRepository = failureRepository;
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await service.GetFailuresAsync());
        }

        public IActionResult Create()
        {
            Populate();
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Create([Bind("Name,Description,Priority,Status,EntryTime,MachineId,Stream,fileURLs")]Failure failure, List<IFormFile> files)
        {
            var m = await service.Create(failure, files);
            if(m == 0)
            {
                return View(failure);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            return View(await service.GetFailureDetailsAsync(id));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Populate();
            return View(await service.GetFailureByIDAsync(id));
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id, Failure failure, List<IFormFile> files)
        {
            if(id == null) return NotFound();
           
                var n = await service.Update(id, failure, files);

                if(n == 0)
                {
                    return View(failure);
                }
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Resolve(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            service.Resolve(id);
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private void Populate(object selectedMachine = null)
        {
            var query = service.GetMachines();
            ViewBag.Machine = new SelectList(query, "MachineId", "MachineName", selectedMachine);
        }
    }
}