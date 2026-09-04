using GymApplication.BLL.Services.Interfaces;
using GymApplication.BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApplication.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            this._trainerService = trainerService;
        }

        public async Task<IActionResult> Index(CancellationToken ct) => View(await _trainerService.GetAllTrainersAsync(ct));

        public async Task<IActionResult>Details(int id , CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpGet]
        public async Task<IActionResult> Create() => View();

        [HttpPost]
        public async Task<IActionResult>Create(CreateTrainerViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid)return View(model);

            var result = await _trainerService.CreateTrainerAsync(model , ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Failed to create Trainer";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult>Edit(int id , CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id);
            if(trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id , TrainerToUpdateViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _trainerService.UpdateTrainerDetailsAsync(id, model , ct);
            if(result) TempData["SuccessMessage"] = "Trainer Updated Successfully";
            else TempData["ErrorMessage"] = "Failed to Update Trainer";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _trainerService.RemoveTrainerAsync(id);
            if (result) TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else TempData["ErrorMessage"] = "Failed to delete Trainer";

            return RedirectToAction(nameof(Index));
        }
    }
}
