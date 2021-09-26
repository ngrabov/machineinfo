using machineinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using machineinfo.ViewModels;

namespace machineinfo.Repositories
{
    public interface IMachineRepository
    {
        Task<IEnumerable<Machine>> GetMachinesAsync();
        Task<int> Create(Machine machine);
        Task<IEnumerable<FailureVM>> GetMachineByIDAsync(int? id);
        Task<Machine> MachineToUpdateAsync(int? id);
        Task<int> Update(int? id, Machine machine);
        Task Delete(int? id);
    }
}