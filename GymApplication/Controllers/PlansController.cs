using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Classes;
using GymApplication.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApplication.BLL.Controllers;

public class PlansController : Controller
{
    private DAL.Repositories.Interfaces.GenericRepository<Plan> _planRepo;

    public PlansController(DAL.Repositories.Interfaces.GenericRepository<Plan> repo)
    {
        _planRepo = repo;
    }

    //Now the controller does not create the object 

    //index action
    //GET BaseURL/Plans/Index ==> listing all plans
    //details action
    //GET BaseURL/Plans/Details ==> single page
    public async Task<IActionResult> Index()
    {
        var plans = await _planRepo.GetAllAsync();
        return View(plans);
    }

    public async Task<IActionResult> Details(int id)
    {
        var myplan = await _planRepo.GetByIdAsync(id);
        if (myplan is null) return RedirectToAction(nameof(Index));
        else return View(myplan);
    }
}
