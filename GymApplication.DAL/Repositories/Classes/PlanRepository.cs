using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GymApplication.DAL.Data.DbContextss;
using GymApplication.DAL.Data.Models;
using GymApplication.DAL.Repositories.Interfaces;

namespace GymApplication.DAL.Repositories.Classes
{
    public class PlanRepository : GenericRepository<Plan> , IPlanRespository
    {
       

        public PlanRepository(GymDbContext dbcontext) : base(dbcontext)
        {
        
        }

        public Task<IEnumerable<Plan>> GetPlanWithMembers()
        {
            throw new NotImplementedException();
        }

       
    }
}
