using AutoMapper;
using GymApplication.BLL.Common;
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

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("EndDate must be after StartDate!");
            if (model.StartDate <= DateTime.Now) return Result.Validation("StartDate must be in future");
            if(model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity must be between 1 and 25");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if(trainer is null) return Result.NotFound("Trainer not found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if(category is null) return Result.NotFound("Category not found");

            var isValid = Enum.TryParse<Speciality>(category.CategoryName, true, out var categorySpeciality);
            if(!isValid || trainer.Speciality != categorySpeciality) return Result.Validation("Can not assign the session to this trainer");

            var session = _mapper.Map<CreateSessionViewModel, Session>(model);

            _unitOfWork.GetRepository<Session>().Add(session);
            return (await _unitOfWork.SaveChangesAsync() > 0) ? Result.OK() : Result.Fail("Failed to create session");
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

        public async Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategory(sessionId, ct);
            if (session is null)
            {
                return Result<SessionViewModel>.NotFound("Session not found");
            }
            else
            {
                var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
                mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(mappedSession.Id, ct);
                return Result<SessionViewModel>.OK(mappedSession);
            }

        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdate(int sessionId, CancellationToken ct = default)
        {
            // can not update completed or ongoing sessions
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            if(session is null)
            {
                return Result<UpdateSessionViewModel>.NotFound("Session was not found");
            }

            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Fail("Can not update an ongoing or completed session");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId,ct);
            if(bookingCount > 0) return Result<UpdateSessionViewModel>.Fail("Can not update booked session");

            var mappedSession = _mapper.Map<Session, UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.OK(mappedSession);

        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(result);
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            // can not update completed or ongoing sessions
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(id, ct);
            if (session == null)
            {
                return Result.NotFound("Session was not found");
            }

            if (model.EndDate <= model.StartDate) return Result.Validation("End date must be after start date");
            if (session.StartDate <= DateTime.Now) return Result.Fail("Can not edit a session that already started");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if (bookingCount > 0) return Result.Fail("Can not update booked session");

            if (model.StartDate < DateTime.Now) return Result.Validation("Start date must be in future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer is null) return Result.NotFound("Trainer not found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);

            var isValid = Enum.TryParse<Speciality>(category?.CategoryName, out var trainerSpec);
            if (!isValid || trainer.Speciality != trainerSpec) return Result.Validation("Can not assign the session to this trainer");

            _mapper.Map(model,session);
            session.UpdatedAt = DateTime.Now;

            _unitOfWork.SessionRepository.Update(session);
            var result = await _unitOfWork.SaveChangesAsync();

            return (result > 0) ? Result.OK() : Result.Fail("Failed to updated session");
        }
    }
}
