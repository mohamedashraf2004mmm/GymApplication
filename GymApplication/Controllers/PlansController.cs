using GymApplication.DAL.Data.DbContextss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using GymApplication.DAL.Repositories;

namespace GymApplication.BLL.Controllers;

public class PlansController : Controller
{
    private IPlanRespository planrepo;

    public PlansController(IPlanRespository repo)
    {
        planrepo = repo;
    }

    //Now the controller does not create the object 

    //index action
    //GET BaseURL/Plans/Index ==> listing all plans
    //details action
    //GET BaseURL/Plans/Details ==> single page
    public async Task<IActionResult> Index()
    {
        var plans = await planrepo.GetAllAsync();
        return View(plans);
    }

    public async Task<IActionResult> Details(int id)
    {
        var myplan = await planrepo.GetByIdAsync(id);
        if (myplan is null) return RedirectToAction(nameof(Index));
        else return View(myplan);
    }
}
