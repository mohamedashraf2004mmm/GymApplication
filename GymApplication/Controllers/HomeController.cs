using System.Diagnostics;
using System.Threading.Tasks;
using GymApplication.BLL.Services.Interfaces;
using GymApplication.DAL.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalyticsService _analyticsService;

        public HomeController(ILogger<HomeController> logger , IAnalyticsService analyticsSer)
        {
            _logger = logger;
            this._analyticsService = analyticsSer;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var analytics = await _analyticsService.GetAnalyticsDataAsync(ct);
            return View(analytics);
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
