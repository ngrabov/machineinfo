using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using machineinfo.Models;
using System.Threading.Tasks;
using machineinfo.ViewModels;
using machineinfo.Services;

namespace machineinfo.Controllers
{
    
    public class HomeController : Controller
    {
        private IHomeService service;

        public HomeController(IHomeService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 5;
            var failures = await service.GetFailuresByPriorityAsync();
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
