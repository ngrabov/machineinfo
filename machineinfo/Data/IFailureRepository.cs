using System.Threading.Tasks;
using machineinfo.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace machineinfo.Data
{
    public interface IFailureRepository
    {
        Task<IEnumerable<Failure>> GetFailuresAsync();
        void Create(Failure failure, List<IFormFile> files);
    }
}