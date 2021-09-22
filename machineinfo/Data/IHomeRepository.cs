using machineinfo.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace machineinfo.Data
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Failure>> GetFailuresByPriorityAsync();
    }
}