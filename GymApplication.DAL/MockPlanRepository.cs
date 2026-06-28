using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Classes;
using GymApplication.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL
{
    public class MockPlanRepository
    {
        public void Add(Plan p)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(Plan entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Plan p)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Plan entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Plan>> GetAllAsync()
        {
            List<Plan> plans = new List<Plan>()
           {
              new(){PlanName = "Test"}
           };
            return plans;
        }

        public Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Plan?> GetByIdAsync(int id)
        {
            var mockplan = new Plan() { PlanName = "mock plan with id 3" };
            return mockplan;
        }

        public Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Plan>> GetPlanWithMembers()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Plan p)
        {
            throw new NotImplementedException();
        }

        
    }
}
