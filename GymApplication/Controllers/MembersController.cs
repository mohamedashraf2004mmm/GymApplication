using GymApplication.BLL.Services.Interfaces;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymApplication.PL.Controllers
{
    public class MembersController : Controller
    {
        //private readonly IGenericRepository<Member> _memberrepo;
        // controller will not talk to repo anymore


        public IMemberService _memberService { get; }


        //Index GET BaseUrl / Members / Index
        // List all members

        public MembersController(IMemberService memberservice)
        {
            _memberService = memberservice;
        }

        //CRUD operations here

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMemberAsync(ct);
            return View(members);
        }

        //Details GET BaseUrl / Members / Details{id}
        // Show one member details

        //HealthRecordDetails GET BaseUrl / Members / HealthRecordDetails{id}
        //show one member health details


        #region Create Member

        //Get => show the form for u (empty form)
        //GET BaseUrl / Members / Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Post => Submit the form
        //Post BaseUrl / Members / Create{member}
        #endregion

        #region Edit Member
        //Get => show the form for u (pre filled form)
        //GET BaseUrl / Members / Edit{id}

        //Post => Submit the form
        //Post BaseUrl / Members / Edit{member}
        #endregion

        #region Delete Member
        //Get => show confirmition 
        //GET BaseUrl / Members / Delete{id}

        //Post => Confirm delete
        //Post BaseUrl / Members / Delete{id}
        #endregion
    }
}
