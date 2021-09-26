using machineinfo.Repositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading.Tasks;
using System.Collections.Generic;
using machineinfo.ViewModels;
using Microsoft.AspNetCore.Http;
using machineinfo.Models;
using System.IO;

namespace machineinfo.Services
{
    public class FailureService : IFailureService
    {
        private readonly IActionContextAccessor modelState;
        private IFailureRepository repository;

        public FailureService(IActionContextAccessor modelState, IFailureRepository repository)
        {
            this.modelState = modelState;
            this.repository = repository;
        }

        public async Task<IEnumerable<FailureVM>> GetFailuresAsync()
        {
            return await repository.GetFailuresAsync();
        }

        public async Task<int> Create(Failure failure, List<IFormFile> files)
        {
            int m = 0;
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
                m = await repository.Create(failure, files);
                if(m == 0)
                {
                    modelState.ActionContext.ModelState.AddModelError("", "There's already an active failure on the selected machine. Try again.");
                }
            }
            catch(System.Exception e)
            {
                modelState.ActionContext.ModelState.AddModelError("", e.Message);
            }
            return m;
        }

        public async Task<FailureVM> GetFailureDetailsAsync(int? id)
        {
            return await repository.GetFailureDetailsAsync(id);
        }

        public async Task<Failure> GetFailureByIDAsync(int? id)
        {
            return await repository.GetFailureByIDAsync(id);
        }

        public async Task<int> Update(int? id, Failure failure, List<IFormFile> files)
        {
            int n = 0;
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
                n = await repository.Update(id, failure);

                if(n == 0)
                {
                    modelState.ActionContext.ModelState.AddModelError("","There's already an active failure on the selected machine. Try again.");
                }
            }
            catch
            {
                modelState.ActionContext.ModelState.AddModelError("Error", "No match");
            }
            return n;
        }

        public async Task Resolve(int? id)
        {
            await repository.Resolve(id);
        }

        public async Task Delete(int? id)
        {
            await repository.Delete(id);
        }

        public IEnumerable<Machine> GetMachines()
        {
            return repository.GetMachines();
        }
    }
}