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

        public MemberService(IGenericRepository<Member> memberrepo)
        {
            this._memberrepo = memberrepo;
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
    }
}
