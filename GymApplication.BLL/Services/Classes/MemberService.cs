using GymApplication.BLL.Services.Interfaces;
using GymApplication.BLL.ViewModels;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberrepo;

        public MemberService(IGenericRepository<Member> memberrepo)
        {
            this._memberrepo = memberrepo;
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
