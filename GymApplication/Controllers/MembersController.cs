using GymApplication.BLL.Services.Attachments;
using GymApplication.BLL.Services.Interfaces;
using GymApplication.BLL.ViewModels;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymApplication.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MembersController : Controller
    {
        private readonly IAttachmentService _attachmentService;

        //private readonly IGenericRepository<Member> _memberrepo;
        // controller will not talk to repo anymore


        public IMemberService _memberService { get; }


        //Index GET BaseUrl / Members / Index
        // List all members

        public MembersController(IMemberService memberservice , IAttachmentService attachmentService)
        {
            _memberService = memberservice;
            this._attachmentService = attachmentService;
        }

        //CRUD operations here

        #region Get Member photo
        [HttpGet]
        public async Task<IActionResult> Picture(int id)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id);
            if(member == null || string.IsNullOrEmpty(member.Photo))
                return NotFound();

          var result =  _attachmentService.GetFile(member.Photo, "MembersPhotos");
          if(result == null)return NotFound();

            return File(result.Value.stream, result.Value.contentType);


        }
        #endregion

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMemberAsync(ct);
            return View(members);
        }

        //Details GET BaseUrl / Members / Details{id}
        // Show one member details
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            //check if member is null => return index with message
            //else => return view data

            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);

        }

        //HealthRecordDetails GET BaseUrl / Members / HealthRecordDetails{id}

        //show one member health details
        //public async Task<IActionResult>HealthRecordDetails(int id , CancellationToken ct)
        //{
        //    //Get health record by member id
        //    //check if member is null => return index with message
        //    //else => return view data

        //}


        #region Create Member

        //Get => show the form for u (empty form)
        //GET BaseUrl / Members / Create
        [HttpGet]
        public IActionResult Create() => View();


        //Post => Submit the form
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }

            var result = await _memberService.CreateMemberAsync(model, ct);
            if (result) 
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed to create Member";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> HealthRecordDetails(int id , CancellationToken ct)
        {
            var result = await _memberService.GetMemberHealthRecordAsync(id, ct);
            if(result is null)
            {
                TempData["ErrorMessage"] = $"Health Record of member with id {id} is not found";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        //Post BaseUrl / Members / Create{member}
        #endregion

        #region Edit Member
        //Get => show the form for u (pre filled form)
        //GET BaseUrl / Members / Edit{id}
        [HttpGet]
        public async Task<IActionResult>EditMember(int id , CancellationToken ct)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member is not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        //Post => Submit the form
        //Post BaseUrl / Members / Edit{member}
        [HttpPost]
        public async Task<IActionResult>EditMember([FromRoute]int id , MemberToUpdateViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

           var result = await _memberService.UpdateMemberDetailsAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfully";
            else
                TempData["ErrorMessage"] = "Member Update Failed";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member
        //Get => show confirmition 
        //GET BaseUrl / Members / Delete{id}
        [HttpGet]
        public async Task<IActionResult>Delete(int id , CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if(member == null)
            {
                TempData["ErrorMessage"] = "Member do not exist";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id , CancellationToken ct)
        {
            var result = await _memberService.RemoveMember(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Member deleted successfully";
            else
                TempData["ErrorMessage"] = "Failed to delete member";
            return RedirectToAction(nameof(Index));
        }

        //Post => Confirm delete
        //Post BaseUrl / Members / Delete{id}
        #endregion
    }
}
