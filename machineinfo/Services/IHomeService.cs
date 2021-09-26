using System.Threading.Tasks;
using System.Collections.Generic;
using machineinfo.ViewModels;

namespace machineinfo.Services
{
    public interface IHomeService
    {
        Task<IEnumerable<FailureVM>> GetFailuresByPriorityAsync();
    }
}