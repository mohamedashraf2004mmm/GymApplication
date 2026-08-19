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
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct:ct);
            var activeMembers = await _unitOfWork.GetRepository<MemberShip>().CountAsync(m => m.EndDate > now , ct);

            var totalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct:ct);

            var upcomingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate > now);
            var ongoingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.StartDate <= now && s.EndDate >= now);
            var completedSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s => s.EndDate < now);

            return new AnalyticsViewModel()
            {
                TotalMembers = totalMembers,
                ActiveMembers = activeMembers,
                TotalTrainers = totalTrainers,
                UpcomingSessions = upcomingSessions,
                OngoingSessions = ongoingSessions,
                CompletedSessions = completedSessions
            };

        }
    }
}
