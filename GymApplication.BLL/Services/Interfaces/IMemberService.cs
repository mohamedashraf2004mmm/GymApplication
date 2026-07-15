using GymApplication.BLL.ViewModels;
using GymApplication.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct = default);

        Task<bool>CreateMemberAsync(CreateMemberViewModel member , CancellationToken ct);

        Task<MemberViewModel?>GetMemberDetailsByIdAsync(int MemberId , CancellationToken ct = default);

        Task<HealthRecordViewModel?>GetMemberHealthRecordAsync(int MemberId , CancellationToken ct = default);

        Task<MemberToUpdateViewModel?>GetMemberToUpdateAsync(int MemberId , CancellationToken ct = default);

        Task<bool>UpdateMemberDetailsAsync(int id , MemberToUpdateViewModel model ,  CancellationToken ct = default);

        Task<bool>RemoveMember(int memberId , CancellationToken ct = default);

    }
}
