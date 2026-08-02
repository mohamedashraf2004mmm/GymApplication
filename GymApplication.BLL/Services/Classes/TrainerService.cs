using GymApplication.BLL.Services.Interfaces;
using GymApplication.BLL.ViewModels;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Classes;
using GymApplication.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        //private readonly IGenericRepository<Trainer> _trainerRepo;
        //private readonly IGenericRepository<Session> _sessionRepo;

        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct : ct);
            return trainers.Select(x => new TrainerViewModel()
            {
                Id = x.Id,
                Name = x.name,
                Email = x.email,
                Phone = x.Phone,
                Specialties = x.Speciality.ToString()
            });
        }
        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId , ct);
            if (trainer == null) return null;
            else
                return new TrainerViewModel()
                {
                    Name = trainer.name,
                    Email = trainer.email,
                    Phone = trainer.Phone,
                    Specialties = trainer.Speciality.ToString(),
                    DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                    Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
                };
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var phoneCheck = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone, ct);
            var emailCheck = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.email == model.Email, ct);

            if (phoneCheck || emailCheck) return false;

            var trainer = new Trainer()
            {
                name = model.Name,
                email = model.Email,
                Phone = model.Phone,
                Speciality = model.Specialties,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street,
                }
            };
            _unitOfWork.GetRepository<Trainer>().Add(trainer);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }



        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId);
            if (trainer == null) return null;
            else
                return new TrainerToUpdateViewModel()
                {
                    Name = trainer.name,
                    Email = trainer.email,
                    Phone = trainer.Phone,
                    BuildingNumber = trainer.Address.BuildingNumber,
                    Street = trainer.Address.Street,
                    City = trainer.Address.City,
                    Specialties = trainer.Speciality
                };
        }
        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId);
            if (trainer == null) return false;

            if (await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.email == model.Email && t.Id != trainerId, ct)) return false;
            if (await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct)) return false;

            trainer.email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Speciality = model.Specialties;
            trainer.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Trainer>().Update(trainer);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }

        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(trainerId);
            if (trainer == null) return false ;

            var hasFutureSessions = await _unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);
            if (hasFutureSessions) return false;

            _unitOfWork.GetRepository<Trainer>().Delete(trainer);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

    }
}
