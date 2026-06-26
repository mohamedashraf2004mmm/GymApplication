using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.DAL.Repositories.Classes
{
    internal class MemberRepository : IMemberRepository
    {
        public GymDbContext dbContext;

        public MemberRepository(GymDbContext dbcontext)
        {
            dbContext = dbcontext;
        }
        public void Add(Member M)
        {
            dbContext.Members.Add(M);
        }

        public void Delete(Member M)
        {
            dbContext.Remove(M);
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await dbContext.Members.ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await dbContext.Members.FirstOrDefaultAsync(m => m.Id == id);
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
