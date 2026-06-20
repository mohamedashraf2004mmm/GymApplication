using GymApplication.DAL.Repositories;
using GymApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL
{
    public class MockPlanRepository : IPlanRespository
    {
        public void Add(Plan p)
        {
            throw new NotImplementedException();
        }

        public void Delete(Plan p)
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

        public async Task<Plan?> GetByIdAsync(int id)
        {
            var mockplan = new Plan() { PlanName = "mock plan with id 3" };
            return mockplan;
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
