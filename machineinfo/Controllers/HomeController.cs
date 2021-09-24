using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using machineinfo.Models;
using machineinfo.Data;
using System.Threading.Tasks;
using machineinfo.ViewModels;
using System.Linq;

namespace machineinfo.Controllers
{
    
    public class HomeController : Controller
    {
        private IHomeRepository homeRepository;

        public HomeController(IHomeRepository homeRepository)
        {
            this.homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 10;
            var failures = await homeRepository.GetFailuresByPriorityAsync();
            return View(PaginatedList<FailureVM>.Create(failures, pageNumber ?? 1, pageSize));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
