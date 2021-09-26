using System.Collections.Generic;
using machineinfo.Repositories;
using System.Threading.Tasks;
using machineinfo.Models;
using machineinfo.ViewModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace machineinfo.Services
{
    public class MachineService: IMachineService
    {
        private readonly IActionContextAccessor modelState;
        private IMachineRepository repository;
        public MachineService(IActionContextAccessor modelState, IMachineRepository repository)
        {
            this.repository = repository;
            this.modelState = modelState;
        }

        public async Task<IEnumerable<Machine>> GetMachinesAsync()
        {
            return await repository.GetMachinesAsync();
        }

        public async Task<int> Create(Machine machine)
        {
            int k = 0;
            try
            {
                k = await repository.Create(machine);
                if(k == 0)
                {
                    modelState.ActionContext.ModelState.AddModelError("", "There's already a machine with the same name in the database. Try again.");
                }
            }
            catch
            {
                modelState.ActionContext.ModelState.AddModelError("", "Model error.");
            }
            return k;
        }

        public async Task<int> Update(int? id, Machine machine)
        {
            int j = 0;
            try
            {
                j = await repository.Update(id, machine);
                if(j == 0)
                {
                    modelState.ActionContext.ModelState.AddModelError("", "There's already a machine with the same name in the database. Try again.");
                }
            }
            catch(System.Exception ex)
            {
                modelState.ActionContext.ModelState.AddModelError("", ex.ToString());
            }
            return j;
        }

        public async Task<IEnumerable<FailureVM>> GetMachineByIDAsync(int? id)
        {
            return await repository.GetMachineByIDAsync(id);
        }

        public async Task<Machine> MachineToUpdateAsync(int? id)
        {
            return await repository.MachineToUpdateAsync(id);
        }

        public void Delete(int? id)
        {
            repository.Delete(id);
        }
    }
}