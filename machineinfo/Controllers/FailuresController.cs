using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using machineinfo.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using machineinfo.Data;

namespace machineinfo.Controllers
{
    public class FailuresController : Controller
    {
        private IDbConnection db;
        private IFailureRepository failureRepository;

        public FailuresController(IDbConnection db, IFailureRepository failureRepository)
        {
            this.db = db;
            this.failureRepository = failureRepository;
        }

        public async Task<IActionResult> Index()
        {
            var failure = await failureRepository.GetFailuresAsync();
            return View(failure);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Create([Bind("Name,Description,Priority,Status,EntryTime,MachineId,Stream,fileURLs")]Failure failure, List<IFormFile> files)
        {
            try
            {
                string fileURLs = "";
                foreach(var file in files)
                {
                    string wbp = Path.GetFileName(file.FileName);

                    using(var stream = System.IO.File.Create("./wwwroot/" + wbp))
                    {
                        await file.CopyToAsync(stream);
                    }
                    
                    fileURLs += wbp + "|";
                }
                failure.fileURLs = fileURLs;
                var m = failureRepository.Create(failure, files);
                if(m == 0)
                {
                    return Content("There's already an active failure on the selected machine. Try again.");
                }
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
            
            var vm = await failureRepository.GetFailureDetailsAsync(id);
            return View(vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var failure = await failureRepository.GetFailureByIDAsync(id);
            return View(failure);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id, Failure failure, System.DateTime? conclusionTime, List<IFormFile> files)
        {
            try
            {
                if(id == null) return NotFound();

                string fileURLs = "";
                foreach(var file in files)
                {
                    string wbp = Path.GetFileName(file.FileName);

                    using(var stream = System.IO.File.Create("./wwwroot/" + wbp))
                    {
                        await file.CopyToAsync(stream);
                    }
                    
                    fileURLs += wbp + "|";
                }
                failure.fileURLs = fileURLs;
                failureRepository.Update(id, failure, conclusionTime);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("Error", "No match");
            }
            return View(failure);
        }

        public IActionResult Resolve(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            failureRepository.Resolve(id);
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            failureRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}