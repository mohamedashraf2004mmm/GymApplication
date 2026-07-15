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
        private readonly IGenericRepository<Member> _memberrepo;
        private readonly IGenericRepository<MemberShip> _membershiprepo;
        private readonly IGenericRepository<Plan> _planrepo;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public MemberService(IGenericRepository<Member> memberrepo , IGenericRepository<MemberShip>membershiprepo ,
            IGenericRepository<Plan> planrepo , IGenericRepository<HealthRecord>HealthRecordRepo ,
            IGenericRepository<Booking> BookingRepo)
        {
            this._memberrepo = memberrepo;
            this._membershiprepo = membershiprepo;
            this._planrepo = planrepo;
            _healthRecordRepo = HealthRecordRepo;
            _bookingRepo = BookingRepo;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            //check email
            var emailexist = await _memberrepo.AnyAsync(x => x.email == model.Email, ct);
            //check phone
            var phoneexist = await _memberrepo.AnyAsync(x => x.Phone == model.Phone, ct);

            if (emailexist || phoneexist) return false;

            var member = new Member()
            {
                name = model.Name,
                email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    Note = model.HealthRecordViewModel.Note!
                }
            };

          var result = await  _memberrepo.AddAsync(member); //returns no of affected rows
            return result > 0;

            //email or phone exist return false

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct = default)
        {
            var members = await _memberrepo.GetAllAsync(ct:ct);
           

            if (!members.Any()) return [];

            //List<MemberViewModel> membersViewModel = new List<MemberViewModel>();

            //foreach (var member in members)
            //{
            //    var MemberViewModel = new MemberViewModel()
            //    {
            //        Name = member.name,
            //        Email = member.email,
            //        Gender = member.Gender.ToString(),
            //        Phone = member.Phone,
            //        Photo = member.Photo,
            //        Id = member.Id,
            //    };
            //    membersViewModel.Add(MemberViewModel);
            //}
            //return membersViewModel;

            var membersviewmodel = members.Select(m => new MemberViewModel()
            {
                Name = m.name,
                Email = m.email,
                Gender = m.Gender.ToString(),
                Phone = m.Phone,
                Photo = m.Photo,
                Id = m.Id,
            });
            return membersviewmodel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _memberrepo.GetByIdAsync(MemberId , ct) ;
            if (member == null) return null;

            var model = new MemberViewModel()
            {
                Name = member.name,
                Phone = member.Phone,
                Email = member.email,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address?.City}"
            };
            //var memberships = await _membershiprepo.GetAllAsync();
            //var aciveMembership = memberships.FirstOrDefault(x => x.MemberId == MemberId && x.EndDate > DateTime.Now);

            var aciveMemberShip = await _membershiprepo.FirstOrDefaultAsync(x => x.MemberId == MemberId && x.EndDate > DateTime.Now) ;

            if(aciveMemberShip is not null)
            {
                var activePlan = await _planrepo.GetByIdAsync(aciveMemberShip.PlanId, ct);

                model.MemberShipStartDate = aciveMemberShip.CreatedAt.ToString();
                model.MemberShipEndDate = aciveMemberShip.EndDate.ToString();

                model.PlanName = activePlan?.PlanName;
            }
            return model;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int MemberId, CancellationToken ct = default)
        {
            var record = await _healthRecordRepo.FirstOrDefaultAsync(x => x.MemberId == MemberId, ct:ct);
            if (record is null) return null;
            else
                return new HealthRecordViewModel()
                {
                    Weight = record.Weight,
                    Height = record.Height,
                    Note = record.Note,
                    BloodType = record.BloodType,
                };


        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int MemberId, CancellationToken ct = default)
        {
            var member = await _memberrepo.GetByIdAsync(MemberId, ct);
            if (member == null) return null;

            else return new MemberToUpdateViewModel()
            {
                Name = member.name,
                Phone = member.Phone,
                Email = member.email,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street,
                Photo = member.Photo
            };
        }

        public async Task<bool> RemoveMember(int memberId, CancellationToken ct = default)
        {
            var member = await _memberrepo.GetByIdAsync(memberId, ct);
            if(member == null) return false;

            var hasFutureBookings = await _bookingRepo.AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now , ct);
            if (hasFutureBookings) return false;

            var result = await _memberrepo.DeleteAsync(member, ct);
            return result > 0;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
           var member = await _memberrepo.GetByIdAsync(id, ct);
            if(member == null) return false;

            var emailExist = await _memberrepo.AnyAsync(m => m.email == model.Email && m.Id != id , ct);
            var phoneExist = await _memberrepo.AnyAsync(m => m.Phone == model.Phone && m.Id != id , ct);

            if(emailExist || phoneExist) return false;

            member.email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;

            var result = await _memberrepo.UpdateAsync(member , ct);
            return result > 0;




        }
    }
}
