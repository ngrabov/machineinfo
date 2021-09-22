using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using machineinfo.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using machineinfo.Data;
using machineinfo.ViewModels;
using System.Linq;

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
                    
                    fileURLs += wbp + " ";
                }
                failure.fileURLs = fileURLs;
                failureRepository.Create(failure, files);
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
            
            var q = "SELECT Failures.FailureId, Failures.Name, Failures.Priority, Failures.Description, Failures.Status, Failures.fileURLs, Machines.MachineName FROM Failures JOIN Machines ON Machines.MachineId = Failures.MachineId WHERE Failures.FailureID = " + @id;
            var vm = await db.QuerySingleOrDefaultAsync<FailureVM>(q);
            return View(vm);
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

            db.Dispose();
            return View(failure);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id, Failure failure, List<IFormFile> files)
        {
            try
            {
                if(id == null) return NotFound();
                db.Open();

                string fileURLs = "";
                foreach(var file in files)
                {
                    string wbp = Path.GetFileName(file.FileName);

                    using(var stream = System.IO.File.Create("./wwwroot/" + wbp))
                    {
                        await file.CopyToAsync(stream);
                    }
                    
                    fileURLs += wbp + " ";
                }
                failure.fileURLs = fileURLs;
                var query = "UPDATE failures SET Name = @Name, Description = @Description, Priority = @Priority, Status = @Status, MachineId = @MachineId, fileURLs = @fileURLs WHERE FailureId = '" + @id + "'";

                var param = new DynamicParameters();
                param.Add("Name", failure.Name);
                param.Add("Description", failure.Description);
                param.Add("Priority", failure.Priority);
                param.Add("Status", failure.Status);
                param.Add("MachineId", failure.MachineId);
                param.Add("fileURLS", failure.fileURLs);

                var query2 = "SELECT MachineId FROM machines";
                var machines = await db.QueryAsync<Machine>(query2);

                int c = 0;
                foreach(var v in machines)
                {
                    if(failure.MachineId == v.MachineId)
                    {
                        c++;
                    }
                }
                if(c == 0)
                {
                    return Content("No matching Machine found for the selected ID.");
                } 

                await db.ExecuteAsync(query, param);
                db.Dispose();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("Error", "No match");
            }
            return View(failure);
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