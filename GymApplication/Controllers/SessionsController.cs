using GymApplication.BLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymApplication.PL.Controllers
{
    [Authorize]
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

            if(result.success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.error;
            await PopulateDropdownsAsync();
            return View(model);

            //PopulateDropdownsAsync();
          
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult>Details(int id , CancellationToken ct)
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(id, ct);
            if (result.success)
            {
                return View(result.value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionToUpdate(id, ct);
            if (result.success) {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");
                return View(result.value);
            }
            
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model , CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");
                return View(model);
            }
            var result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Updated";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult>Delete(int id , CancellationToken ct)
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(id);
            if(result.success)
            {
                return View(result.value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _sessionService.DeleteSessionAsync(id);

            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] = result.success ? "Session deleted successfully" : result.error;
            return RedirectToAction(nameof(Index));

        }
        #endregion
        private async Task PopulateDropdownsAsync()
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDownAsync(), "Id", "CategoryName");
        }


    }
}
