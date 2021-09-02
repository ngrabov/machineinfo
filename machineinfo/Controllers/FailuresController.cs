using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using machineinfo.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace machineinfo.Controllers
{
    public class FailuresController : Controller
    {
        private IDbConnection db;

        public FailuresController(IDbConnection db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            var query = "SELECT * FROM failures";
            db.Open();
            var failure = await db.QueryAsync<Failure>(query);
            db.Dispose();
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
                db.Open();
                string fileURLs = "";
                foreach(var file in files)
                {
                    string ext = file.FileName;
                    string pth = Path.GetTempFileName();
                    string nx = Path.GetFileNameWithoutExtension(ext);
                    string wbp = Path.GetFileName(ext);

                    using(var stream = System.IO.File.Create("./wwwroot/" + wbp))
                    {
                        await file.CopyToAsync(stream);
                    }
                    
                    fileURLs += wbp + " ";
                }
                failure.fileURLs = fileURLs;
                var query = "INSERT INTO Failures (Name, Description, Priority, Status, EntryTime, MachineId, fileURLs) VALUES (@Name, @Description, @Priority, @Status, current_timestamp, @MachineId, @fileURLs)";
                
                db.Execute(query, new[]{
                    new{Name = failure.Name, Description = failure.Description, Priority = failure.Priority, 
                    Status = failure.Status, EntryTime = System.DateTime.Now, MachineId = failure.MachineId, fileURLs = failure.fileURLs}
                });

                db.Dispose();
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
                    string ext = file.FileName;
                    string pth = Path.GetTempFileName();
                    string nx = Path.GetFileNameWithoutExtension(ext);
                    string wbp = Path.GetFileName(ext);

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