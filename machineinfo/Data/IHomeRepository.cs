using System.Threading.Tasks;
using System.Collections.Generic;
using machineinfo.ViewModels;

namespace machineinfo.Data
{
    public interface IHomeRepository
    {
        Task<IEnumerable<FailureVM>> GetFailuresByPriorityAsync();
    }
}