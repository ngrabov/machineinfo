using System.Threading.Tasks;
using machineinfo.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using machineinfo.ViewModels;

namespace machineinfo.Services
{
    public interface IFailureService
    {
        Task<IEnumerable<FailureVM>> GetFailuresAsync();
        Task<int> Create(Failure failure, List<IFormFile> files);
        Task<FailureVM> GetFailureDetailsAsync(int? id);
        Task<Failure> GetFailureByIDAsync(int? id);
        Task<int> Update(int? id, Failure failure, List<IFormFile> files);
        Task Resolve(int? id);
        Task Delete(int? id);
        IEnumerable<Machine> GetMachines();
    }
}