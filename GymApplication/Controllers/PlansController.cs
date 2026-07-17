using GymApplication.BLL.Services.Interfaces;
using GymApplication.BLL.ViewModels;
using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Classes;
using GymApplication.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApplication.BLL.Controllers;

public class PlansController : Controller
{
    //private IGenericRepository<Plan> _planRepo;
    private readonly IPlanService _planService;

    public PlansController(IPlanService planService)
    {
        this._planService = planService;
    }

    //Now the controller does not create the object 

    //index action
    //GET BaseURL/Plans/Index ==> listing all plans
    //details action
    //GET BaseURL/Plans/Details ==> single page
    public async Task<IActionResult> Index()
    {
        var plans = await _planService.GetAllPlansAsync();
        return View(plans);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id , CancellationToken ct)
    {
         var plan = await _planService.GetPlanByIdAsync(id , ct);
        if(plan is null)
        {
            TempData["ErrorMessage"] = "Plan not found";
            return RedirectToAction(nameof(Index));
        }
        return View(plan);
    }

    [HttpGet]
    public async Task<IActionResult>Edit(int planId , CancellationToken ct)
    {
        var plan = await _planService.GetPlanToUpdateAsync(planId , ct);
        if(plan is null)
        {
            TempData["ErrorMessage"] = "Plan can not be edited";
            return RedirectToAction(nameof(Index));
        }
        return View(plan);
    }

    [HttpPost]
    public async Task<IActionResult>Edit(int id , UpdatePlanViewModel model , CancellationToken ct = default)
    {
        if (!ModelState.IsValid)return View(model);

        var result = await _planService.UpdatePlanAsync(id , model , ct);
        if (result)
        {
            TempData["SuccessMessage"] = "Plan Updated Successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = "Can not update plan";
        return View(model);
    }
}
