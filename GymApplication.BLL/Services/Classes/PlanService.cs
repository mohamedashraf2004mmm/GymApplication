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
    public class PlanService : IPlanService
    {
        //private readonly IGenericRepository<Plan> _planRepo;
        //private readonly IGenericRepository<MemberShip> _memberShipsRepo;

        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct:ct);

            var models = plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.PlanName,
                Price = p.Price,
                Duration = p.DurationDays,
                Description = p.PlanDescription,
                IsActive = p.IsActive
            });
            return models;
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int planId , CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan is null) return null;
            else
                return new PlanViewModel()
                {
                    Id = plan.Id,
                    Name = plan.PlanName,
                    Price = plan.Price,
                    Duration = plan.DurationDays,
                    Description = plan.PlanDescription,
                    IsActive = plan.IsActive
                };
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if(plan is null || !plan.IsActive) return null;

            if (await HasActiveMemberShips(planId, ct)) return null;
            else
            {
                return new UpdatePlanViewModel()
                {
                    planName = plan.PlanName,
                    Price = plan.Price,
                    Description = plan.PlanDescription,
                    DurationDays = plan.DurationDays,
                };
            }
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return false;
            if ((await HasActiveMemberShips(id, ct)))return false;

            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.PlanDescription = model.Description;
            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }

        private async Task<bool>HasActiveMemberShips(int planId,CancellationToken ct = default)
        {
            return await _unitOfWork.GetRepository<MemberShip>().AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct); 
        }
    }
}
