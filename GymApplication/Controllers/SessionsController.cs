using GymApplication.BLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymApplication.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            this._sessionService = sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _sessionService.GetAllSessionAsync(ct);
            return View(sessions);
        }

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model, ct);

            if(result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Failed to Create Session";
            await PopulateDropdownsAsync();
            return View(model);

            //PopulateDropdownsAsync();
          
        }

        private async Task PopulateDropdownsAsync()
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDownAsync(), "Id", "CategoryName");
        }
        #endregion
    }
}
