using System.Threading.Tasks;
using System.Collections.Generic;
using machineinfo.ViewModels;

namespace machineinfo.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<FailureVM>> GetFailuresByPriorityAsync();
    }
}