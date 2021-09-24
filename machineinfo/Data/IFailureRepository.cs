using System.Threading.Tasks;
using machineinfo.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using machineinfo.ViewModels;

namespace machineinfo.Data
{
    public interface IFailureRepository
    {
        Task<IEnumerable<FailureVM>> GetFailuresAsync();
        int Create(Failure failure, List<IFormFile> files);
        Task<FailureVM> GetFailureDetailsAsync(int? id);
        Task<Failure> GetFailureByIDAsync(int? id);
        int Update(int? id, Failure failure);
        void Resolve(int? id);
        void Delete(int? id);
        IEnumerable<Machine> GetMachines();
    }
}