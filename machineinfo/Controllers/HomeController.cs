using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using machineinfo.Models;
using System.Data;
using machineinfo.Data;
using System.Threading.Tasks;

namespace machineinfo.Controllers
{
    
    public class HomeController : Controller
    {
        private IHomeRepository homeRepository;

        public HomeController(IHomeRepository homeRepository)
        {
            this.homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var failures = await homeRepository.GetFailuresByPriorityAsync();
            return View(failures);
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
