using System.Threading.Tasks;
using machineinfo.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using machineinfo.ViewModels;

namespace machineinfo.Repositories
{
    public interface IFailureRepository
    {
        Task<IEnumerable<FailureVM>> GetFailuresAsync();
        Task<int> Create(Failure failure, List<IFormFile> files);
        Task<FailureVM> GetFailureDetailsAsync(int? id);
        Task<Failure> GetFailureByIDAsync(int? id);
        Task<int> Update(int? id, Failure failure);
        Task Resolve(int? id);
        Task Delete(int? id);
        IEnumerable<Machine> GetMachines();
    }
}