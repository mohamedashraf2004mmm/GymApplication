using AutoMapper;
using GymApplication.BLL.Services.Attachments;
using GymApplication.BLL.Services.Interfaces;
using GymApplication.BLL.ViewModels;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
       private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        //private readonly IGenericRepository<Member> _memberrepo;
        //private readonly IGenericRepository<MemberShip> _membershiprepo;
        //private readonly IGenericRepository<Plan> _planrepo;
        //private readonly IGenericRepository<HealthRecord> _healthRecordRepo;
        //private readonly IGenericRepository<Booking> _bookingRepo;



        public MemberService(IUnitOfWork unitofwork, IMapper mapper , IAttachmentService attachmentService)
        {
            _unitOfWork = unitofwork;
            _mapper = mapper;
            this._attachmentService = attachmentService;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            //check email
            var emailexist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.email == model.Email, ct);
            //check phone
            var phoneexist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone, ct);

            if (emailexist || phoneexist) return false;

            //upload photo
         var storedPhotoName =   await _attachmentService.UploadAsync(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName, "MembersPhotos");
            if (string.IsNullOrWhiteSpace(storedPhotoName)) return false;


            var member = _mapper.Map<CreateMemberViewModel , Member>(model);
            member.Photo = storedPhotoName;
          
            //var result = await _unitOfWork.GetRepository<Member>().AddAsync(member); //returns no of affected rows
            //return result > 0;
            //email or phone exist return false

            _unitOfWork.GetRepository<Member>().Add(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result > 0) return true;
            else
            {
                //delete uploaded photo
                return false;
            }

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct:ct);
           

            if (!members.Any()) return [];

            var membersviewmodel = _mapper.Map<IEnumerable<Member> , IEnumerable<MemberViewModel>>(members);

            return membersviewmodel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId , ct) ;
            if (member == null) return null;

            var model = _mapper.Map<Member , MemberViewModel>(member);
            //var memberships = await _membershiprepo.GetAllAsync();
            //var aciveMembership = memberships.FirstOrDefault(x => x.MemberId == MemberId && x.EndDate > DateTime.Now);

            var aciveMemberShip = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(x => x.MemberId == MemberId && x.EndDate > DateTime.Now) ;

            if(aciveMemberShip is not null)
            {
                var activePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(aciveMemberShip.PlanId, ct);

                model.MemberShipStartDate = aciveMemberShip.CreatedAt.ToString();
                model.MemberShipEndDate = aciveMemberShip.EndDate.ToString();

                model.PlanName = activePlan?.PlanName;
            }
            return model;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int MemberId, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x => x.MemberId == MemberId, ct:ct);
            if (record is null) return null;
            else
                return _mapper.Map<HealthRecord, HealthRecordViewModel>(record);


        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (member == null) return null;

            else return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public async Task<bool> RemoveMember(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if(member == null) return false;

            var hasFutureBookings = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now , ct);
            if (hasFutureBookings) return false;

            _unitOfWork.GetRepository<Member>().Delete(member);
            return await _unitOfWork.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
           var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if(member == null) return false;

            var emailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.email == model.Email && m.Id != id , ct);
            var phoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id , ct);

            if(emailExist || phoneExist) return false;

            // We will ignore name and photo => will not be mapped

            _mapper.Map(model , member);
            member.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Member>().Update(member);
            return await _unitOfWork.SaveChangesAsync(ct) > 0;
        }
    }
}
