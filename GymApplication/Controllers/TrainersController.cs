using GymApplication.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymApplication.PL.Controllers
{
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


    }
}
