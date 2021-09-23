using machineinfo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using machineinfo.ViewModels;

namespace machineinfo.Data
{
    public interface IMachineRepository
    {
        Task<IEnumerable<Machine>> GetMachinesAsync();
        int Create(Machine machine);
        Task<IEnumerable<FailureVM>> GetMachineByIDAsync(int? id);
        Task<Machine> MachineToUpdateAsync(int? id);
        int Update(int? id, Machine machine);
        void Delete(int? id);
    }
}