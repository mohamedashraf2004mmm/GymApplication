using AutoMapper;
using GymApplication.BLL.Services.Interfaces;
using GymApplication.BLL.ViewModels.SessionViewModels;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Data.Models.Enums;
using GymApplication.DAL.Repositories.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork UnitOfWork , IMapper mapper)
        {
            _unitOfWork = UnitOfWork;
            this._mapper = mapper;
        }

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return false;
            if (model.StartDate <= DateTime.Now) return false;
            if(model.Capacity < 1 || model.Capacity > 25) return false;

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if(trainer is null) return false;

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if(category is null) return false;

            var isValid = Enum.TryParse<Speciality>(category.CategoryName, true, out var categorySpeciality);
            if(!isValid || trainer.Speciality != categorySpeciality) return false;

            var session = _mapper.Map<CreateSessionViewModel, Session>(model);

            _unitOfWork.GetRepository<Session>().Add(session);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainersAndCategoryAsync(ct: ct);
            if (sessions == null || !sessions.Any()) return null;

            var mappedSessions = sessions.Select(s => new SessionViewModel()
            {
                Id = s.Id,
                Capacity = s.Capacity,
                CategoryName = s.Category.CategoryName,
                TrainerName = s.Trainer.name,
                EndDate = s.EndDate,
                StartDate = s.StartDate,
            });

            foreach(var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }

            return mappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<Category> , IEnumerable<CategorySelectViewModel>>(result);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(result);
        }
    }
}
