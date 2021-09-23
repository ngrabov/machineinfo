using machineinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using machineinfo.ViewModels;

namespace machineinfo.Data
{
    public interface IMachineRepository
    {
        Task<IEnumerable<Machine>> GetMachinesAsync();
        void CreateAsync(Machine machine);
        Task<IEnumerable<FailureVM>> GetMachineByIDAsync(int? id);
        Task<Machine> MachineToUpdateAsync(int? id);
        void UpdateAsync(int? id, Machine machine);
        void DeleteAsync(int? id);
    }
}