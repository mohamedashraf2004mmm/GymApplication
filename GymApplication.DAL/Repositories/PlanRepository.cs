using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;

namespace GymApplication.DAL.Repositories
{
    public class PlanRepository : IPlanRespository
    {
        public GymDbContext dbContext;

        public PlanRepository(GymDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }
        public void Add(Plan p)
        {
            dbContext.Plans.Add(p);
        }

        public void Delete(Plan p)
        {
            dbContext.Remove(p);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync()
        {
            return await dbContext.Plans.ToListAsync();
        }

        public async Task<Plan?> GetByIdAsync(int id)
        {
            return await dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        } 

        public Task UpdateAsync(Plan p)
        {
            throw new NotImplementedException();
        }
    }
}
