using Microsoft.AspNetCore.Mvc.Infrastructure;
using machineinfo.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using machineinfo.ViewModels;

namespace machineinfo.Services
{
    public class HomeService : IHomeService
    {
        private readonly IActionContextAccessor modelState;
        private IHomeRepository repository;

        public HomeService(IHomeRepository repository, IActionContextAccessor modelState)
        {
            this.repository = repository;
            this.modelState = modelState;
        }

        public async Task<IEnumerable<FailureVM>> GetFailuresByPriorityAsync()
        {
            return await repository.GetFailuresByPriorityAsync();
        }
    }
}